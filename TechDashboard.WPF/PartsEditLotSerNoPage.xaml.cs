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
using TechDashboard.Data;
using TechDashboard.Models;
using TechDashboard.ViewModels;

/**************************************************************************************************
 * Page Name    PartsEditLotSerNoPage
 * Description: Parts Edit Lot/Serial No Page Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 12/02/2016   DCH     Created
 * 01/16/2017   DCH     If the quantity is negative, allow them to select from a list of 
 *                      previously used Lot/Serial Numbers
 * 01/20/2017   DCH     Include Unit Cost on Grid
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for PartsEditLotSerNoPage.xaml
    /// </summary>
    public partial class PartsEditLotSerNoPage : UserControl
    {
        PartsEditLotSerNoPageViewModel _vm;
        App_WorkTicket _workTicket;
        App_RepairPart _part;
        PartsEditPage.PageMode _pageMode;
        App_ScheduledAppointment _scheduledAppointment;

        public PartsEditLotSerNoPage(App_WorkTicket workTicket, App_RepairPart part, PartsEditPage.PageMode pageMode,
            App_ScheduledAppointment scheduledAppointment)
        {
            _workTicket = workTicket;
            _part = part;
            _pageMode = pageMode;
            _scheduledAppointment = scheduledAppointment;

            _vm = new PartsEditLotSerNoPageViewModel(part, workTicket);

            InitializeComponent();

            SetPageLayout();
        }

        protected void SetPageLayout()
        {
            labelItemCode.Content = _part.PartItemCode;
            labelItemCodeDesc.Content = _part.PartItemCodeDescription;
            labelUM.Content = _part.UnitOfMeasure;
            labelQty.Content = _part.Quantity;

            gridLotSerNo.ItemsSource = _vm.SerialNumberList;

            // dch rkl 01/16/2017 if quantity is negative, allow them to enter the serial number
            if (_part.Quantity < 0)
            {
                gridLotSerNo.Columns[0].IsReadOnly = false;
            }
            else
            {
                gridLotSerNo.Columns[0].IsReadOnly = true;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel - return to parts edit page
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsEditPage(_workTicket, _part, _pageMode, _scheduledAppointment);          
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            // OK - accept selections and return to parts list page

            // Make sure quantity selected matches quantity for the part line
            List<String> lsLotQty = new List<string>();

            double dTotQty = 0;
            foreach (LotQavl avl in _vm.SerialNumberList)
            {
                dTotQty += avl.QtyUsed;

                if (avl.QtyUsed != 0)
                {
                    lsLotQty.Add(string.Format("{0}~{1}", avl.LotNo, avl.QtyUsed.ToString()));

                    // If Valuation of Serial No, can only allocate 1 per line
                    if (_part.Valuation == "6" && avl.QtyUsed > 1)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show(
                            "Qty Used cannot exceed 1 for Serialized Items", "Invalid Qty Used", MessageBoxButton.OK);
                        if (result == MessageBoxResult.OK)
                            return;
                    }

                    // Make sure Qty Used does not exceed Qty Avail
                    if (avl.QtyUsed > avl.QAvl)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show(
                            string.Format("Qty Used ({0}) for Lot/SerNo {1} exceeds Qty Available ({2}).", avl.QtyUsed, avl.LotNo, avl.QAvl), "Invalid Qty Used for Lot/SerNo", MessageBoxButton.OK);
                        if (result == MessageBoxResult.OK)
                            return;
                    }
                }
            }

            if (dTotQty != _part.Quantity)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(
                    string.Format("Quantity Used ({0}) does not match Part Quantity ({1})", dTotQty, _part.Quantity), "Invalid Qty Used", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            else
            {
                // Save Lot/Serial No to JT_TransactionImportDetail
                // Until the sync, lot/serial and qty are stored as a delimited string, such as:  LLLL~QQQ|LLLL~QQQ|LLLL~QQQ
                string lotSerNo = string.Join("|", lsLotQty);
                _vm.PartToEdit.LotSerialNo = lotSerNo;
                _vm.UpdatePartOnPartsList();

                // Return to Parts List
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsListPage(_workTicket, _scheduledAppointment);
            }
        }
    }
}
