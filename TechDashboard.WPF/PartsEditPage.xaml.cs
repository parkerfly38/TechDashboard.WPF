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
 * Page Name    PartsEditPage
 * Description: Parts Edit Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 11/02/2016   DCH     Handle when warehouse does not have any items (null value), for misc items
 * 11/21/2016   DCH     If part is purchased, do not allow edit of quantity
 * 11/22/2016   DCH     If chargeable is checked, allow unit price to be updated
 * 11/23/2016   DCH     If Misc Part, allow entry of Unit Cost and hide warehouse dropdown
 * 11/27/2016   DCH     Make sure Unit Cost is numeric
 * 12/01/2016   DCH     Include warehouse description in dropdown.  Warehouse comes from IM_Warehouse,
 *                      not IM_ItemWarehouse
 * 12/05/2016   DCH     Include App_RepairPart as shared object in page
 *                      Track Lot/Serial Number availability 
 *                      If Qty Shipped is > 0, cannot edit Warehoues or U/M
 *                      Remove lot selection from screen.  Lot/SerialNo selection is now done on
 *                      another screen.
 * 01/13/2017   DCH     Return the ID of the Inserted Part, so it can be used to save Lot/Serial 
 *                      Number allocation.
 *                      Make sure the lot/serial number qty available is >= quantity being added
 *                      before adding the part.
 * 01/13/2017   DCH     Move all of the page layout to the .xaml page instead of generating it
 *                      within the code.
 * 01/13/2017   DCH     If extended description exists, display button to view/edit it
 * 01/18/2017   DCH     If Is Chargeable = true, set printable to true and disable 
 * 01/23/2017   DCH     Make the unit of measure a dropdown list instead of a text field.
 * 01/23/2017   DCH     Allow entry of combo-box item for U/M for Misc Items.
 * 01/23/2017   DCH     If SO_SalesOrderDetail.JT158_WTBillFlag is set to "R" or "B", do not allow 
 *                      edit of the part.
 * 01/25/2017   BK      Allow ItemCodeDesc to be editable
 * 01/27/2017   BK      Forcing CI_Options rules
 * 02/03/2017   DCH     Do not allow existing Sales Order parts to be deleted (i.e. SOLineKey > 0)
 *                      Set quantity required to quantity entered.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal dec;
            if (decimal.TryParse(value as string, out dec))
                return dec;
            return value;
        }
    }
    /// <summary>
    /// Interaction logic for PartsEditPage.xaml
    /// </summary>
    public partial class PartsEditPage : UserControl
    {
        public enum PageMode
        {
            Add,
            Edit
        };

        PartsEditPageViewModel _vm;
        App_ScheduledAppointment _scheduledAppointment;
        App_RepairPart _part;           // dch rkl 12/05/2016

        CI_Options _ciOptions;
        string quantityFormatString;
        string umFormatString;
        string costFormatString;
        string priceFormatString;

        PageMode _pageMode;

        List<LotQavl> _lotSerNo;            // dch rkl 12/05/2016

        public PartsEditPage(App_WorkTicket workTicket, CI_Item part, PageMode pageMode, App_ScheduledAppointment scheduledAppointment)
        {
            _pageMode = pageMode;
            _vm = new PartsEditPageViewModel(workTicket, part);
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();

            SetPageLayout();
        }

        public PartsEditPage(App_WorkTicket workTicket, App_RepairPart part, PageMode pageMode, App_ScheduledAppointment scheduledAppointment)
        {
            _pageMode = pageMode;
            _vm = new PartsEditPageViewModel(workTicket, part);
            _scheduledAppointment = scheduledAppointment;

            _part = part;       // dch rkl 12/05/2016

            InitializeComponent();

            SetPageLayout();
        }
        protected void SetPageLayout()
        {
            // Set the page title.
            switch (_pageMode)
            {
                case PageMode.Add:
                    _labelTitle.Content = "ADD PART";
                    break;
                case PageMode.Edit:
                    _labelTitle.Content = "EDIT PART";
                    break;
                default:
                    _labelTitle.Content = "ADD/EDIT PART";
                    break;
            }
            // get our ci_options first
            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            gridMain.DataContext = _vm.PartToEdit;

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));

            // Set Bindings
            _labelPartNumber.SetBinding(ContentProperty, "PartItemCode");
            _labelPartNumber.Text = _vm.PartToEdit.PartItemCode;

            var ciOptions = App.Database.GetCIOptions();

            _labelPartDescription.SetBinding(ContentProperty, "PartItemCodeDescription");
            _labelPartDescription.Text = _vm.PartToEdit.PartItemCodeDescription;

            _pickerWarehouse.ItemsSource = _vm.WarehouseList;
            try { _pickerWarehouse.SelectedValue = _vm.PartToEdit.Warehouse; } catch { }

            if (_pageMode == PageMode.Edit) { _entryQuantity.Text = _vm.PartToEdit.Quantity.ToString(); }
            else { _entryQuantity.Text = "1"; }

            // dch rkl 01/23/2017 Change U/M to picklist
            _pickerUnitOfMeasure.ItemsSource = _vm.UnitOfMeasureList;
            try { _pickerUnitOfMeasure.SelectedValue = _vm.PartToEdit.UnitOfMeasure; }
            catch (Exception ex) { }
            if (_pickerUnitOfMeasure.SelectedIndex == -1 && _vm.PartToEdit.PartItemCode.Trim().Substring(0, 1) == "*" || _vm.PartToEdit.ItemType == "4" || _vm.PartToEdit.ItemType == "5")
            {
                AddItemToUMList(_vm.PartToEdit.UnitOfMeasure);
            }
            //_entryUnitOfMeasure.SetBinding(ContentProperty, "UnitOfMeasure");
            //_entryUnitOfMeasure.Text = _vm.PartToEdit.UnitOfMeasure;
            //_entryUnitOfMeasure.Style = (Style)this.Resources["styleUOM"];

            // dch rkl 12/05/2016 if qty shipped > 0, cannot edit warehouse or u/m
            if (_vm.PartToEdit.QuantityShipped > 0)
            {
                _pickerWarehouse.IsEnabled = false;

                // dch rkl 01/23/2017 Change U/M to picklist
                //_entryUnitOfMeasure.IsEnabled = false;
                _pickerUnitOfMeasure.IsEnabled = false;
            }

            // dch rkl 11/23/2016 If Edit Mode, and a Quantity has been entered, disable U/M
            if (_pageMode == PageMode.Edit && _vm.PartToEdit.Quantity != 0)
            {
                // dch rkl 01/23/2017 Change U/M to picklist
                //_entryUnitOfMeasure.IsEnabled = false;
                _pickerUnitOfMeasure.IsEnabled = false;
            }

            _entryUnitCost.Text = _vm.PartToEdit.UnitCost.ToString("C2");

            // dch rkl 11/23/2016 if misc part, allow entry of unit cost
            if (_vm.PartToEdit.PartItemCode.Trim().Substring(0,1) == "*" || _vm.PartToEdit.ItemType == "4" || _vm.PartToEdit.ItemType == "5")
            {
                _entryUnitCost.IsEnabled = true;
                // dch rkl 01/23/2017 if miscellaneous, allow entry of U/M
                _pickerUnitOfMeasure.IsEditable = true;
            }
            else
            {
                _entryUnitCost.IsEnabled = false;
                // dch rkl 01/23/2017 if miscellaneous, allow entry of U/M
                _pickerUnitOfMeasure.IsEditable = false;
            }

            // dch rkl 01/18/2017 If not chargeable, set Unit Price and Ext Price to zero.
            if (_vm.PartToEdit.IsChargeable)
            {
                //_entryUnitPrice.Text = _vm.PartToEdit.UnitPrice.ToString("C2");
                _entryUnitPrice.Text = string.Format(priceFormatString, _vm.PartToEdit.UnitPrice);
                if (_pageMode == PageMode.Edit)
                {
                    _labelExtensionPrice.Content = string.Format(priceFormatString, (_vm.PartToEdit.UnitPrice * _vm.PartToEdit.Quantity));
                    //(_vm.PartToEdit.UnitPrice * _vm.PartToEdit.Quantity).ToString("C2"); }
                } else {
                    _labelExtensionPrice.Content = string.Format(priceFormatString, (_vm.PartToEdit.UnitPrice * 1));
                    //(_vm.PartToEdit.UnitPrice * 1).ToString("C2"); 
                }
            }
            else
            {
                _labelExtensionPrice.Content = string.Format(priceFormatString, 0);
                _entryUnitPrice.Text = string.Format(priceFormatString, 0);
            }

            _entryComments.SetBinding(ContentProperty, "Comment");
            _entryComments.Style = (Style)this.Resources["styleComments"];
            _entryComments.Text = _vm.PartToEdit.Comment;

            if (_pageMode == PageMode.Edit)
            {
                _switchIsChargeable.SetBinding(CheckBox.IsCheckedProperty, "IsChargeable");
                _switchIsPrintable.SetBinding(CheckBox.IsCheckedProperty, "IsPrintable");
                _switchIsPurchased.SetBinding(CheckBox.IsCheckedProperty, "IsPurchased");
                _switchIsOverhead.SetBinding(CheckBox.IsCheckedProperty, "IsOverhead");
            }

            // dch rkl 11/21/2016 if part is purchased, do not allow editing of quantity
            if (_vm.PartToEdit.IsPurchased)
            {
                _entryQuantity.IsEnabled = false;
            }
            else
            {
                _entryQuantity.IsEnabled = true;
            }

            // dch rkl 11/22/2016 if chargeable is checked, allow unit price to be updated.
            // If chargeable is not checked, unit price cannot be updated.
            if (_vm.PartToEdit.IsChargeable)
            {
                _entryUnitPrice.IsEnabled = true;
            }
            else
            {
                _entryUnitPrice.IsEnabled = false;
                _entryUnitPrice.Text = string.Format(priceFormatString, 0);
            }

            // dch rkl 11/23/2016 if misc part, hide warehouse dropdown
            bool bShowWhse = true;
            if (_vm.PartToEdit.PartItemCode.Trim().Substring(0, 1) == "*" || _vm.PartToEdit.ItemType == "4" || _vm.PartToEdit.ItemType == "5")
            {
                bShowWhse = false;
            }
            if (bShowWhse)
            {
                _pickerWarehouse.Visibility = Visibility.Visible;
                labelWarehouse.Visibility = Visibility.Visible;
            }
            else
            {
                _pickerWarehouse.Visibility = Visibility.Hidden;
                labelWarehouse.Visibility = Visibility.Hidden;
            }

            // dch rkl 01/13/2017 if extended description exists, display button to view/edit it BEGIN
            if (_vm.ExtendedDescriptionKey > 0 && _vm.ExtendedDescriptionText.Trim().Length > 0)
            {
                _buttonExtdDesc.Visibility = Visibility.Visible;
            }
            else
            {
                _buttonExtdDesc.Visibility = Visibility.Hidden;
            }
            // dch rkl 01/13/2017 if extended description exists, display button to view/edit it END

            switch (_pageMode)
            {
                case PageMode.Add:
                    buttonAddPartText.Text = "ADD";
                    break;
                case PageMode.Edit:
                    buttonAddPartText.Text = "UPDATE";
                    break;
            }

            if (_pageMode == PageMode.Add)
            {
                _buttonDeletePart.Visibility = Visibility.Hidden;
            } else
            {
                _buttonDeletePart.Visibility = Visibility.Visible;
            }

            // dch rkl 01/23/2017 If SO_SalesOrderDetail.JT158_WTBillFlag is set to "R" or "B", do not allow edit of the part
            int iSOLineKey;
            int.TryParse(_vm.PartToEdit.SoLineKey, out iSOLineKey);
            if (_vm.PartToEdit.SoLineKey != null)
            {
                if (iSOLineKey > 0)
                {
                    List<SO_SalesOrderDetail> lsSODtl = App.Database.GetSalesOrderDetails(_vm.WorkTicket.SalesOrderNo);
                    SO_SalesOrderDetail soDtl = lsSODtl.FirstOrDefault(s => s.LineKey == _vm.PartToEdit.SoLineKey);
                    if (soDtl != null && soDtl.JT158_WTBillFlag != null && (soDtl.JT158_WTBillFlag == "R" || soDtl.JT158_WTBillFlag == "B"))
                    {
                        _buttonAddPart.Visibility = Visibility.Hidden;
                        _buttonDeletePart.Visibility = Visibility.Hidden;
                    }
                }
            }

            // dch rkl 02/03/2017 Do not allow existing parts to be deleted
            if (iSOLineKey > 0)
            {
                _buttonDeletePart.Visibility = Visibility.Hidden;
            }
        }

        private void _buttonDeletePart_Click(object sender, RoutedEventArgs e)
        {
            // dch rkl 12/01/2016 Picker is now bound to type of IM_Warehouse
            //_vm.PartToEdit.Warehouse = (string)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
            IM_Warehouse whse = new IM_Warehouse();
            if (_pickerWarehouse != null && _pickerWarehouse.SelectedIndex > -1)
            {
                whse = (IM_Warehouse)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
            }
            //IM_Warehouse whse = (IM_Warehouse)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
            _vm.PartToEdit.Warehouse = whse.WarehouseCode;

            _vm.PartToEdit.Quantity = Convert.ToDouble(_entryQuantity.Text);
            //_vm.PartToEdit.SerialNumber = (string)_pickerSerialNumber.Items[_pickerSerialNumber.SelectedIndex];
            // dch rkl 10/13/2016 remove $ sign before saving
            //_vm.PartToEdit.UnitPrice = Convert.ToDouble(_entryUnitPrice.Text);
            _vm.PartToEdit.UnitPrice = Convert.ToDouble(_entryUnitPrice.Text.Replace("$", ""));

            // dch rkl 01/23/2017 Change U/M to picklist
            //_vm.PartToEdit.UnitOfMeasure = _entryUnitOfMeasure.Text;
            if (_pickerUnitOfMeasure != null && _pickerUnitOfMeasure.SelectedIndex > -1)
            {
                _vm.PartToEdit.UnitOfMeasure = _pickerUnitOfMeasure.SelectedValue.ToString();
            }

            _vm.PartToEdit.IsChargeable = (bool)_switchIsChargeable.IsChecked;
            _vm.PartToEdit.IsPrintable = (bool)_switchIsPrintable.IsChecked;
            _vm.PartToEdit.IsPurchased = (bool)_switchIsPurchased.IsChecked;
            _vm.PartToEdit.IsOverhead = (bool)_switchIsOverhead.IsChecked;
            _vm.PartToEdit.Comment = _entryComments.Text;

            _vm.DeletePart();
            
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
        }

        private void _pickerSerialNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_vm.UpdateQuantityOnHand(_vm.PartToEdit.Warehouse, (string)_pickerSerialNumber.Items[_pickerSerialNumber.SelectedIndex]);
        }

        private void _pickerWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_pickerWarehouse.SelectedIndex > -1)
            {
                // dch rkl 12/01/2016 Picker is bound to IM_Warehouse BEGIN
                //_vm.PartToEdit.Warehouse = (string)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
                IM_Warehouse whse = (IM_Warehouse)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
                _vm.PartToEdit.Warehouse = whse.WarehouseCode;
                // dch rkl 12/01/2016 Picker is bound to IM_Warehouse END

                //_vm.UpdateQuantityOnHand(_vm.PartToEdit.Warehouse, (string)_pickerSerialNumber.Items[_pickerSerialNumber.SelectedIndex]);

                // dch rkl 11/27/2016 Rebind Lot/Serial for this Warehouse BEGIN
                //List<Data.LotQavl> lsSrnNo = _vm.GetMfgSerialNumbersForPart();
                //_pickerSerialNumber.ItemsSource = lsSrnNo;
                //if ((_pickerSerialNumber.Items == null) || (_pickerSerialNumber.Items.Count == 0))
                //{
                //    _pickerSerialNumber.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    _pickerSerialNumber.Visibility = Visibility.Visible;
                //    _pickerSerialNumber.SelectedIndex = 0;
                //}
                // dch rkl 11/27/2016 Rebind Lot/Serial for this Warehouse END
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            //if (_pageMode == PageMode.Add)
            //{
            contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
            //}
           
        }

        // dch rkl 01/13/2017 Display Extended Description
        private void buttonExtdDesc_Click(object sender, RoutedEventArgs e)
        {
            // Show Extended Description Screen
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsEditExtdDescPage(_vm.WorkTicket, _vm.PartToEdit, PartsEditPage.PageMode.Edit,
                _scheduledAppointment);
        }

        private void ButtonAddPart_Click(object sender, RoutedEventArgs e)
        {
            // dch rkl 01/23/2017 Change U/M to picklist
            string UM = "";
            if (_pickerUnitOfMeasure != null && _pickerUnitOfMeasure.SelectedIndex > -1)
            {
                UM = _pickerUnitOfMeasure.SelectedValue.ToString();
            }

            if (UM.Length <= 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("You must select a unit of measure.", "Unit of Measure", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            // dch rkl 11/27/2016 Make Sure Value in Unit Cost is Numeric
            decimal dPrice;
            if (decimal.TryParse(_entryUnitPrice.Text.Replace("$", ""), out dPrice) == false)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Unit Price must be numeric.", "Invalid Unit Price", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            // dch rkl 11/27/2016 Make Sure Value in Unit Cost is Numeric
            decimal dCost;
            if (decimal.TryParse(_entryUnitCost.Text.Replace("$", ""), out dCost) == false)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Unit Cost must be numeric.", "Invalid Unit Cost", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            // dch rkl 12/06/2016 make sure sufficient inventory is available
            // dch rkl 12/05/2016 If Lot(5) or Serial (6) Controlled, show Lot/Serial Selection
            if (_vm.PartToEdit.Valuation == "5" || _vm.PartToEdit.Valuation == "6")
            {
                _lotSerNo = _vm.GetMfgSerialNumbersForPart();
                // dch rkl 01/13/2017 Make Sure Serial Number Count is >= Quantity Being Entered
                decimal dQty;
                decimal.TryParse(_entryQuantity.Text, out dQty);
                //if (_lotSerNo == null || _lotSerNo.Count == 0)
                if (_lotSerNo == null || _lotSerNo.Count < dQty)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Insufficient Inventory Available for this Item/Whse", "Insufficient Inventory", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                        return;
                }
            }

            // dch rkl 11/02/2016 make sure _pickerWarehouse has at least one value
            if (_pickerWarehouse != null && _pickerWarehouse.Items.Count > 0 && _pickerWarehouse.SelectedIndex > -1)
            {
                // dch rkl 12/01/2016 Picker is now bound to type of IM_Warehouse
                //_vm.PartToEdit.Warehouse = (string)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
                IM_Warehouse whse = (IM_Warehouse)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
                _vm.PartToEdit.Warehouse = whse.WarehouseCode;
            }
            else
            {
                _vm.PartToEdit.Warehouse = "";
            }

            _vm.PartToEdit.Quantity = Convert.ToDouble(_entryQuantity.Text);

            // dch rkl 10/13/2016 remove $ sign before saving
            //_vm.PartToEdit.UnitPrice = Convert.ToDouble(_entryUnitPrice.Text);
            _vm.PartToEdit.UnitPrice = Convert.ToDouble(_entryUnitPrice.Text.Replace("$", ""));

            // dch rkl 11/21/2016 Unit Cost is editable for misc items
            _vm.PartToEdit.UnitCost = Convert.ToDouble(_entryUnitCost.Text.Replace("$", ""));

            // dch rkl 01/23/2017 Change U/M to picklist
            //_vm.PartToEdit.UnitOfMeasure = _entryUnitOfMeasure.Text;
            _vm.PartToEdit.UnitOfMeasure = UM;

            var ciOptions = App.Database.GetCIOptions();
            if (ciOptions.AllowExpandedItemCodes == "Y")
            {
                _vm.PartToEdit.PartItemCode = _labelPartNumber.Text.Substring(0,_labelPartNumber.Text.Length > 30 ? 30 : _labelPartNumber.Text.Length);
            } else
            {
                _vm.PartToEdit.PartItemCode = _labelPartNumber.Text.Substring(0, _labelPartNumber.Text.Length > 15 ? 15 : _labelPartNumber.Text.Length);
            }

            _vm.PartToEdit.ItemCodeDesc = _labelPartDescription.Text;

            _vm.PartToEdit.IsChargeable = (bool)_switchIsChargeable.IsChecked;
            _vm.PartToEdit.IsPrintable = (bool)_switchIsPrintable.IsChecked;
            _vm.PartToEdit.IsPurchased = (bool)_switchIsPurchased.IsChecked;
            _vm.PartToEdit.IsOverhead = (bool)_switchIsOverhead.IsChecked;
            _vm.PartToEdit.Comment = _entryComments.Text;

            // dch rkl 02/03/2017 Per Jeanne, Set Quantity Required to Quantity entered
            _vm.PartToEdit.QuantityReqd = (decimal)_vm.PartToEdit.Quantity;
            
            switch (_pageMode)
            {
                case PageMode.Add:
                    // dch rkl 01/13/2017 Return the ID of the Inserted Part
                    int iId = _vm.AddPartToPartsList();
                    _vm.PartToEdit.ID = iId;
                    //_vm.AddPartToPartsList();

                    //await Navigation.PopAsync();
                    break;
                case PageMode.Edit:
                    _vm.UpdatePartOnPartsList();
                    break;
            }

            // dch rkl 12/05/2016 If Lot(5) or Serial (6) Controlled, show Lot/Serial Selection
            if (_vm.PartToEdit.Valuation == "5" || _vm.PartToEdit.Valuation == "6")
            {
                ShowLotSerNoSelection();
            }
            else
            {
                // If not lot controlled/
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
            }
        }

        // dch rkl 12/02/2016 Show Lot/Serial No Screen
        private void ShowLotSerNoSelection()
        {
            // Show Lot/Serial No Selection Screen
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsEditLotSerNoPage(_vm.WorkTicket, _vm.PartToEdit, PartsEditPage.PageMode.Edit,
                _scheduledAppointment);
        }

        private void EntryQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_labelExtensionPrice == null)
                return;
            double n;
            bool isNumeric = double.TryParse(_entryQuantity.Text, out n);
            if (!isNumeric)
            {
                n = 1;
                _entryQuantity.Text = string.Format(quantityFormatString, 1); //"1";
            }
            //_labelExtensionPrice.Content = "$" + (n * _vm.PartToEdit.UnitPrice).ToString();
            _labelExtensionPrice.Content = "$" + string.Format(priceFormatString, (n * _vm.PartToEdit.UnitPrice));
        }

        // dch rkl 11/21/2016 if part is purchased, do not allow editing of quantity
        private void _switchIsPurchased_Click(object sender, EventArgs e)
        {
            if (_switchIsPurchased.IsChecked == true)
            {
                _entryQuantity.IsEnabled = false;
            }
            else
            {
                _entryQuantity.IsEnabled = true;
            }
        }

        // dch rkl 11/22/2016 if chargeable is checked, allow unit price to be updated.
        // If chargeable is not checked, unit price cannot be updated.
        private void _switchIsChargeable_Click(object sender, EventArgs e)
        {
            if (_vm.PartToEdit.IsChargeable)
            {
                _entryUnitPrice.IsEnabled = true;

                // dch rkl 01/18/2017 If Is Chargeable = true, set printable to true and disable 
                _switchIsPrintable.IsChecked = true;      
                _switchIsPrintable.IsEnabled = false;

                // dch rkl 01/18/2017 If chargeable, set Unit Price and Ext Price values
                //_entryUnitPrice.Text = _vm.PartToEdit.UnitPrice.ToString("C2");
                //_labelExtensionPrice.Content = (_vm.PartToEdit.UnitPrice * _vm.PartToEdit.Quantity).ToString("C2");
                _entryUnitPrice.Text = string.Format(priceFormatString, _vm.PartToEdit.UnitPrice);
                _labelExtensionPrice.Content = string.Format(priceFormatString, (_vm.PartToEdit.UnitPrice * _vm.PartToEdit.Quantity));
            }
            else
            {
                _entryUnitPrice.IsEnabled = false;
                _entryUnitPrice.Text = string.Format(priceFormatString, 0);//"0.00";

                // dch rkl 01/18/2017 If Is Chargeable = false, enable printable
                _switchIsPrintable.IsEnabled = true;

                // dch rkl 01/18/2017 If not chargeable, set Unit Price and Ext Price to zero.
                _labelExtensionPrice.Content = string.Format(priceFormatString, 0);
                _entryUnitPrice.Text = string.Format(priceFormatString, 0);
            }
        }

        // dch rkl 01/23/2017 Allow entry of combo-box item for Misc Items BEGIN
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
                return;
            var newItem = comboBox.Text;
            AddItemToUMList(newItem);
            //CI_UnitOfMeasure um = _vm.UnitOfMeasureList.FirstOrDefault(s => s.UnitOfMeasure == newItem);
            //if (um == null)
            //{
            //    _vm.UnitOfMeasureList.Add(new CI_UnitOfMeasure() { UnitOfMeasure = newItem });
            //    _vm.UnitOfMeasureList.Sort((x, y) => x.UnitOfMeasure.CompareTo(y.UnitOfMeasure));
            //}
            //comboBox.SelectedValue = newItem;
        }

        private void AddItemToUMList(string newItem)
        {
            if (newItem != null)
            {
                CI_UnitOfMeasure um = _vm.UnitOfMeasureList.FirstOrDefault(s => s.UnitOfMeasure == newItem);
                if (um == null)
                {
                    _vm.UnitOfMeasureList.Add(new CI_UnitOfMeasure() { UnitOfMeasure = newItem });
                    _vm.UnitOfMeasureList.Sort((x, y) => x.UnitOfMeasure.CompareTo(y.UnitOfMeasure));
                }
                _pickerUnitOfMeasure.SelectedValue = newItem;
            }
        }
        // dch rkl 01/23/2017 Allow entry of combo-box item for Misc Items END

    }
}
