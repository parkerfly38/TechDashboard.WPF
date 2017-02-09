using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechDashboard.ViewModels;
using TechDashboard.Models;
using TechDashboard.Data;

/**************************************************************************************************
 * Page Name    PartsListPage
 * Description: Parts List Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 11/02/2016   DCH     Add a scroll viewer to the parts grid, so you can scroll to the bottom of
 *                      the page to add a part if the grid fills the page.
 * 11/22/2016   DCH     Move screen layout to designer/XAML.  Format grid to make it more readable.
 * 12/01/2016   DCH     Add find textbox to search parts already on the ticket.
 * 12/04/2016   DCH     Add coverage checkboxes, for display
 * 02/03/2017   DCH     Capture additional errors with try/catch.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for PartsListPage.xaml
    /// </summary>
    public partial class PartsListPage : UserControl
    {
        PartsListPageViewModel _vm;

        Label _labelTitle;
        App_ScheduledAppointment _scheduledAppointment;

        App_WorkTicket _workTicket;     // dch rkl 12/04/2016 Need this for coverage checkboxes

        public PartsListPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new PartsListPageViewModel(workTicket);
            _scheduledAppointment = scheduledAppointment;

            // dch rkl 12/04/2016 Need this for coverage checkboxes
            _workTicket = workTicket;

            InitializeComponent();
            SetPageDisplay();
        }

        protected void SetPageDisplay()
        {
            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));
            gridMain.DataContext = _vm.ObservablePartsList;

            gridParts.ItemsSource = _vm.PartsList;
            gridParts.MouseDoubleClick += gridParts_MouseDoubleClick;
            gridParts.PreviewKeyDown += gridParts_PreviewKeyDown;       // dch rkl 11/01/2016 Enter to select ticket

            // dch rkl 12/04/2016 Set Coverage Checkboxes BEGIN
            // Warranty Repair
            if (_workTicket.DtlWarrantyRepair == "Y") { switchWarrRepair.IsChecked = true; }

            // Parts Coverred on Warranty
            bool bIsChkd = false;
            if (_workTicket.StatusDate != null && _workTicket.RepairItem.MfgPartsWarrantyPeriod != null)
            {
                TimeSpan tsDateDiff = _workTicket.RepairItem.MfgPartsWarrantyPeriod.Subtract(_workTicket.StatusDate);
                if (tsDateDiff.TotalDays > 0 && _workTicket.DtlWarrantyRepair == "Y")
                {
                    switchPartsCovWarr.IsChecked = true;
                    bIsChkd = true;
                }
            }
            if (_workTicket.StatusDate != null && _workTicket.RepairItem.MfgPartsWarrantyPeriod != null)
            {
                TimeSpan tsDateDiff = _workTicket.RepairItem.MfgPartsWarrantyPeriod.Subtract(_workTicket.StatusDate);
                if (tsDateDiff.TotalDays > 0 && _workTicket.DtlWarrantyRepair == "Y")
                {
                    switchPartsCovWarr.IsChecked = true;
                    bIsChkd = true;
                }
            }

            // Service Agreement Repair
            if (_workTicket.DtlCoveredOnContract == "Y") { switchSvcAgrRepair.IsChecked = true; }

            // Parts Covered on Service Agreement
            if (_workTicket.IsPreventativeMaintenance && _workTicket.ServiceAgreement.ArePreventativeMaintenancePartsCovered)
            {
                switchPartsCovSvcAgr.IsChecked = true;
            }
            else if (_workTicket.IsPreventativeMaintenance == false && _workTicket.IsServiceAgreementRepair
                && _workTicket.ServiceAgreement.ArePartsCovered)
            {
                switchPartsCovSvcAgr.IsChecked = true;
            }
            // dch rkl 12/04/2016 Set Coverage Checkboxes END

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

        // dch rkl 11/22/2016 selection of grid
        private void gridParts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridParts.SelectedIndex > -1)
            {
                App_RepairPart part = (App_RepairPart)gridParts.SelectedItem;
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Edit, _scheduledAppointment);
            }
        }

        private void gridParts_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (gridParts.SelectedIndex > -1 && e.Key == Key.Enter)
            {
                App_RepairPart part = (App_RepairPart)gridParts.SelectedItem;
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Edit, _scheduledAppointment);
            }
        }

        private void ButtonAddEditPart_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsAddPage(_vm.WorkTicket, _scheduledAppointment);
        }

        private void btnFindF_Click(object sender, RoutedEventArgs e)
        {
            // Find Next
            string findText = textFind.Text.Trim().ToLower();
            bool bFound = false;

            int j = 0;
            if (gridParts.SelectedIndex > -1) { j = gridParts.SelectedIndex + 1; }

            for (int i = j; i < gridParts.Items.Count; i++)
            {
                bFound = RowSearchCompare(i);
                if (bFound) { break; }
            }

            // If search did not start at top of grid, loop around to beginning of grid and search
            if (bFound == false && j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    bFound = RowSearchCompare(i);
                    if (bFound) { break; }
                }
            }
        }

        private bool RowSearchCompare(int i)
        {
            bool bMatch = false;
            string findText = textFind.Text.Trim().ToLower();

            try
            {
                App_RepairPart part = (App_RepairPart)gridParts.Items[i];
                if (part != null && (part.PartItemCode.ToLower().IndexOf(textFind.Text) > -1
                    || part.PartItemCodeDescription.ToLower().IndexOf(findText) > -1))
                {
                    object item = gridParts.Items[i];
                    gridParts.SelectedItem = item;
                    gridParts.ScrollIntoView(item);
                    bMatch = true;
                }
            }
            catch (Exception ex)
            {
                // dch rkl 02/03/2017 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsListPage.RowSearchCompare");
            }

            return bMatch;
        }
    }
}
