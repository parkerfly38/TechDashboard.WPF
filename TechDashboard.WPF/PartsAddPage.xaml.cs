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
using TechDashboard.Models;
using TechDashboard.ViewModels;

/**************************************************************************************************
 * Page Name    PartsAddPage
 * Description: Parts Add Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 11/15/2016   DCH     Allow entry of miscellaneous parts - anything beginning with "*"
 * 11/27/2016   DCH     Make sure scrolling is enabled in list
 *                      Standardize Button Sizes and Colors
 *                      Cancel button should return to parts list, not ticket details
 * 01/20/2017   DCH     Format the parts search grid to match the parts page. Move content to XAML 
 *                      instead of code-behind.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for PartsAddPage.xaml
    /// </summary>
    public partial class PartsAddPage : UserControl
    {
        PartsAddPageViewModel _vm;
        App_ScheduledAppointment _scheduledAppointment;

        public PartsAddPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new PartsAddPageViewModel(workTicket);
            _vm.FilterItemList("ZXZXZXZXZX128391");
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();

            gridParts.ItemsSource = _vm.ItemList;
            gridParts.MouseDoubleClick += gridParts_MouseDoubleClick;
            gridParts.PreviewKeyDown += gridParts_PreviewKeyDown;       // dch rkl 11/01/2016 Enter to select ticket
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (searchBarText.Text == null || searchBarText.Text.Length == 0)
            {
                _vm.FilterItemList(null);
            }
            else
            {
                // dch rkl 11/15/2016 add miscellaneous part
                if (searchBarText.Text.Trim().Substring(0,1) == "*")
                {
                    CI_Item ciItem = new CI_Item();
                    ciItem.ItemCode = searchBarText.Text.Trim();
                    ciItem.ItemCodeDesc = "";
                    ciItem.StandardUnitOfMeasure = "EACH";
                    IM_ItemWarehouse imWhse = new IM_ItemWarehouse();
                    JT_EquipmentAsset jtAsst = new JT_EquipmentAsset();
                    App_Item appItem = new App_Item(ciItem, imWhse, jtAsst);                        
                    App_RepairPart part = new App_RepairPart(appItem, _vm.WorkTicket);
                    ContentControl contentArea = (ContentControl)this.Parent;
                    contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Add, _scheduledAppointment);
                }
                else
                {
                    _vm.FilterItemList(searchBarText.Text);
                }
            }
        }

        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            // dch rkl 11/27/2016 on Cancel, return to parts list page
            //contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
            contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
        }

        private void gridParts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridParts.SelectedIndex > -1)
            {                   
                App_RepairPart part = new App_RepairPart((App_Item)gridParts.SelectedItem, _vm.WorkTicket);
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Add, _scheduledAppointment);
            }
        }

        private void gridParts_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (gridParts.SelectedIndex > -1 && e.Key == Key.Enter)
            {
                App_RepairPart part = new App_RepairPart((App_Item)gridParts.SelectedItem, _vm.WorkTicket);
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Add, _scheduledAppointment);
            }
        }
    }
}
