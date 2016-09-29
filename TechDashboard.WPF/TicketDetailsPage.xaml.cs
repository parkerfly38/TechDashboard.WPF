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
using System.Xml;
using TechDashboard.Models;
using TechDashboard.Tools;
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for TicketDetailsPage.xaml
    /// </summary>
    public partial class TicketDetailsPage : UserControl
    {
        protected TicketDetailsPageViewModel _vm;

        public TicketDetailsPage(App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new TicketDetailsPageViewModel(scheduledAppointment);
            InitializeComponent();
            InitializePage();
            btnClockInOut.Click += BtnClockInOut_Click;
            btnCustomer.Click += BtnCustomer_Click;
            btnNotes.Click += BtnNotes_Click;
            btnParts.Click += BtnParts_Click;
            btnMap.Click += BtnMap_Click;
            btnSchedule.Click += BtnSchedule_Click;
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new ScheduleDetailPage(_vm.ScheduledAppointment);
        }

        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsListPage(_vm.WorkTicket, _vm.ScheduledAppointment);
        }

        private void BtnMap_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder customerAddress = new StringBuilder();
            //Pin pin = new Pin();

            customerAddress.Append(_vm.Customer.AddressLine1);
            customerAddress.Append(", ");
            customerAddress.Append(_vm.Customer.City);
            customerAddress.Append(", ");
            customerAddress.Append(_vm.Customer.State);
            customerAddress.Append(" ");
            customerAddress.Append(_vm.Customer.ZipCode);

            XmlDocument location;

            try
            {
                //var location = geoCoder.GetPositionsForAddress(customerAddress.ToString());
                //var address = geoCoder.GetAddressesForPosition(new Position(39.909606, -76.299061));
                //List<Position> approximateLocation = location.ToList();
                //pin.Position = approximateLocation[0];
                //pin.Label = _vm.Customer.CustomerName;
                //pin.Address = customerAddress.ToString();

                //await Navigation.PushAsync(new CustomerMapPage(pin));
                GeoCode geocode = new GeoCode();
                location = geocode.Geocode(customerAddress.ToString());
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Invalid Location", "Address cannot be mapped.", "OK");
                return;
            }
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new CustomerMapPage(location, _vm.ScheduledAppointment);
        }

        private void BtnNotes_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new NotesPage(_vm.WorkTicket, _vm.ScheduledAppointment);
        }

        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentarea = (ContentControl)this.Parent;
            contentarea.Content = new CustomerDetailsPage(_vm.Customer, _vm.ScheduledAppointment);
        }

        private void BtnClockInOut_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            if (App.Database.IsClockedIn())
            {
                contentArea.Content = new ClockOutPage(_vm.WorkTicket);
            }
            else {
                contentArea.Content = new ClockInPage(_vm.ScheduledAppointment);
            }
        }

        public TicketDetailsPage()
        {
            InitializeComponent();
        }

        public void InitializePage()
        {

            if (App.Database.IsClockedIn())
            {
                if (_vm.ScheduledAppointment.IsCurrent)
                {
                    StackPanel sp = (StackPanel)btnClockInOut.Content;
                    TextBlock tb = (TextBlock)sp.Children[1];
                    tb.Text = "CLOCK OUT";//btnClockInOut.Content
                }
                else
                {
                    btnClockInOut.Visibility = Visibility.Hidden;
                }
            } else
            {
                StackPanel sp = (StackPanel)btnClockInOut.Content;
                TextBlock tb = (TextBlock)sp.Children[1];
                tb.Text = "CLOCK IN";//btnClockInOut.Content
            }

            Label _labelTitle = new Label();
            _labelTitle.Content = "TECHNICIAN";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 18;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);

            //Label labelTechnicianNo = new Label();
            //labelTechnicianNo.Content = _vm.AppTechnician.FormattedTechnicianNumber;
            //labelTechnicianNo.Foreground = new SolidColorBrush(Colors.White);

            Label labelTechnicianName = new Label()
            {
                Content = _vm.AppTechnician.FirstName + ' ' + _vm.AppTechnician.LastName,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18
            };

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                Height = 100
            };
            
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);
            //titleLayout.Children.Add(labelTechnicianNo);
            //Grid.SetColumn(labelTechnicianNo, 0);
            //Grid.SetRow(labelTechnicianNo, 1);
            titleLayout.Children.Add(labelTechnicianName);
            Grid.SetColumn(labelTechnicianName, 0);
            Grid.SetRow(labelTechnicianName, 1);
            //stackPanelMain.Children.Add(labelTechnicianName);
            stackPanelMain.Children.Add(titleLayout);

            TextBlock labelServiceTicketNo = new TextBlock()
            {
                Text = _vm.WorkTicket.FormattedTicketNumber
            };

            TextBlock labelServiceTicketDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.Description
            };

            TextBlock labelCustomerName = new TextBlock()
            {
                Text = _vm.Customer.CustomerName
            };

            TextBlock labelCustomerNo = new TextBlock();
            labelCustomerNo.Text = _vm.Customer.CustomerNo;

            TextBlock labelContactCode = new TextBlock();
            labelContactCode.Text = _vm.CustomerContact.ContactCode;

            TextBlock labelContactName = new TextBlock();
            labelContactName.Text = _vm.CustomerContact.ContactName;

            StackPanel stackPanelAddress = new StackPanel();

            // label for address line 1
            if ((_vm.SalesOrder.ShipToAddress1 != null) &&
                (_vm.SalesOrder.ShipToAddress1.Trim().Length > 0))
            {
                TextBlock labelAddressLine1 = new TextBlock();
                labelAddressLine1.Text = _vm.SalesOrder.ShipToAddress1;

                stackPanelAddress.Children.Add(labelAddressLine1);
            }

            // label for address line 2
            if ((_vm.SalesOrder.ShipToAddress2 != null) &&
                (_vm.SalesOrder.ShipToAddress2.Trim().Length > 0))
            {
                TextBlock labelAddressLine2 = new TextBlock();
                labelAddressLine2.Text = _vm.SalesOrder.ShipToAddress2;
                stackPanelAddress.Children.Add(labelAddressLine2);
            }

            // label for address line 3
            if ((_vm.SalesOrder.ShipToAddress3 != null) &&
                (_vm.SalesOrder.ShipToAddress3.Trim().Length > 0))
            {
                TextBlock labelAddressLine3 = new TextBlock();
                labelAddressLine3.Text = _vm.SalesOrder.ShipToAddress3;
                stackPanelAddress.Children.Add(labelAddressLine3);
            }

            // label for city/state/zip
            TextBlock labelCityStateZip = new TextBlock();
            labelCityStateZip.Text = _vm.SalesOrder.ShipToCity + ", " + _vm.SalesOrder.ShipToState + " " + _vm.SalesOrder.ShipToZipCode;
            stackPanelAddress.Children.Add(labelCityStateZip);

            Label labelTelephoneTitle = new Label()
            {
                Content = "Main Phone:",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            // label for the phone number
            Label labelTelephoneNo = new Label();
            labelTelephoneNo.Content = _vm.Customer.TelephoneNo;
            if ((_vm.Customer.TelephoneExt != null) &&
                (_vm.Customer.TelephoneExt.Trim().Length > 0))
            {
                labelTelephoneNo.Content += " Ext. " + _vm.Customer.TelephoneExt;
            
            }

            labelTelephoneNo.HorizontalAlignment = HorizontalAlignment.Left;
            labelTelephoneNo.VerticalAlignment = VerticalAlignment.Top;
            Label labelShipToPhoneTitle = new Label()
            {
                Content = "Shipping Phone:",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Label labelShipToPhoneNo = new Label();
            labelShipToPhoneNo.Content = _vm.SalesOrder.TelephoneNo;
            if (_vm.SalesOrder.TelephoneExt != null && _vm.SalesOrder.TelephoneExt.Trim().Length > 0)
            {
                labelShipToPhoneNo.Content += "Ext. " + _vm.SalesOrder.TelephoneExt;
            }

            labelShipToPhoneNo.HorizontalAlignment = HorizontalAlignment.Left;
            labelShipToPhoneNo.VerticalAlignment = VerticalAlignment.Top;

            Label labelContactPhoneTitle = new Label()
            {
                Content = "Contact Phone:",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Label labelContactPhone1 = new Label();
            labelContactPhone1.HorizontalAlignment = HorizontalAlignment.Left;
            labelContactPhone1.VerticalAlignment = VerticalAlignment.Top;
            labelContactPhone1.Content = _vm.CustomerContact.TelephoneNo1;
            if (_vm.CustomerContact.TelephoneExt1 != null && _vm.CustomerContact.TelephoneExt1.Trim().Length > 0)
            {
                labelContactPhone1.Content += "Ext. " + _vm.CustomerContact.TelephoneExt1;
            }

            Label labelContactPhone2 = new Label();
            if (_vm.CustomerContact.TelephoneNo2 != null && _vm.CustomerContact.TelephoneNo2.Trim().Length > 0)
            {
                labelContactPhone2.Content = _vm.CustomerContact.TelephoneNo2;
            }
            if (_vm.CustomerContact.TelephoneExt2 != null && _vm.CustomerContact.TelephoneExt2.Trim().Length > 0)
            {
                labelContactPhone2.Content += "Ext. " + _vm.CustomerContact.TelephoneExt2;
            }
            labelContactPhone2.HorizontalAlignment = HorizontalAlignment.Left;
            labelContactPhone2.VerticalAlignment = VerticalAlignment.Top;
            labelContactPhone2.Content = _vm.CustomerContact.TelephoneNo1;

            Grid phoneGrid = new Grid();
            phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            phoneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star),  });
            phoneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            phoneGrid.Children.Add(labelTelephoneTitle);
            Grid.SetColumn(labelTelephoneTitle, 0);
            Grid.SetRow(labelTelephoneTitle, 0);
            phoneGrid.Children.Add(labelTelephoneNo);
            Grid.SetColumn(labelTelephoneNo, 1);
            Grid.SetRow(labelTelephoneNo, 0);
            phoneGrid.Children.Add(labelShipToPhoneTitle);
            Grid.SetColumn(labelShipToPhoneTitle, 0);
            Grid.SetRow(labelShipToPhoneTitle, 1);
            phoneGrid.Children.Add(labelShipToPhoneNo);
            Grid.SetColumn(labelShipToPhoneNo, 1);
            Grid.SetRow(labelShipToPhoneNo, 1);
            phoneGrid.Children.Add(labelContactPhoneTitle);
            Grid.SetColumn(labelContactPhoneTitle, 0);
            Grid.SetRow(labelContactPhoneTitle, 2);
            Grid.SetRowSpan(labelContactPhoneTitle, 2);
            phoneGrid.Children.Add(labelContactPhone1);
            Grid.SetColumn(labelContactPhone1, 1);
            Grid.SetRow(labelContactPhone1, 2);
            phoneGrid.Children.Add(labelContactPhone2);
            Grid.SetColumn(labelContactPhone2, 1);
            Grid.SetRow(labelContactPhone2, 3);
            

            Label labelServiceTicketTitle = new Label()
            {
                Content = "Service Ticket",
                FontWeight = FontWeights.Bold
            };
            Label labelCustomerTitle = new Label()
            {
                Content = "Customer No",
                FontWeight = FontWeights.Bold
            };
            Label labelContactTitle = new Label()
            {
                Content = "Contact",
                FontWeight = FontWeights.Bold
            };
            Label labelAddressTitle = new Label()
            {
                Content = "Address",
                FontWeight = FontWeights.Bold
            };
            Grid topGrid = new Grid();
            topGrid.Margin = new Thickness(10);
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumnSpan(stackPanelAddress, 2);
            topGrid.Children.Add(labelServiceTicketTitle);
            Grid.SetColumn(labelServiceTicketTitle, 0);
            Grid.SetRow(labelServiceTicketTitle, 0);
            topGrid.Children.Add(labelServiceTicketNo);
            Grid.SetColumn(labelServiceTicketNo, 1);
            Grid.SetRow(labelServiceTicketNo, 0);
            topGrid.Children.Add(labelServiceTicketDesc);
            Grid.SetColumn(labelServiceTicketDesc, 2);
            Grid.SetRow(labelServiceTicketDesc, 0);
            topGrid.Children.Add(labelCustomerTitle);
            Grid.SetColumn(labelCustomerTitle, 0);
            Grid.SetRow(labelCustomerTitle, 1);
            topGrid.Children.Add(labelCustomerNo);
            Grid.SetColumn(labelCustomerNo, 1);
            Grid.SetRow(labelCustomerNo, 1);
            topGrid.Children.Add(labelCustomerName);
            Grid.SetColumn(labelCustomerName, 2);
            Grid.SetRow(labelCustomerName, 1);

            topGrid.Children.Add(labelContactTitle);
            Grid.SetColumn(labelContactTitle, 0);
            Grid.SetRow(labelContactTitle, 2);
            topGrid.Children.Add(labelContactCode);
            Grid.SetColumn(labelContactCode, 1);
            Grid.SetRow(labelContactCode, 2);
            topGrid.Children.Add(labelContactName);
            Grid.SetColumn(labelContactName, 2);
            Grid.SetRow(labelContactName, 2);

            topGrid.Children.Add(labelAddressTitle);
            Grid.SetColumn(labelAddressTitle, 0);
            Grid.SetRow(labelAddressTitle, 3);
            topGrid.Children.Add(stackPanelAddress);
            Grid.SetColumn(stackPanelAddress, 1);
            Grid.SetRow(stackPanelAddress, 3);
            topGrid.Children.Add(phoneGrid);
            Grid.SetColumn(phoneGrid, 0);
            Grid.SetRow(phoneGrid, 4);
            Grid.SetColumnSpan(phoneGrid, 2);
            Grid.SetRowSpan(phoneGrid, 3);
            /*topGrid.Children.Add(labelTelephoneTitle);
            Grid.SetColumn(labelTelephoneTitle, 0);
            Grid.SetRow(labelTelephoneTitle, 4);
            topGrid.Children.Add(labelTelephoneNo);
            Grid.SetColumn(labelTelephoneNo, 1);
            Grid.SetRow(labelTelephoneNo, 4);

            topGrid.Children.Add(labelShipToPhoneTitle);
            Grid.SetColumn(labelShipToPhoneTitle, 0);
            Grid.SetRow(labelShipToPhoneTitle, 5);
            topGrid.Children.Add(labelShipToPhoneNo);
            Grid.SetColumn(labelShipToPhoneNo, 1);
            Grid.SetRow(labelShipToPhoneNo, 5);
            topGrid.Children.Add(labelContactPhoneTitle);
            Grid.SetColumn(labelContactPhoneTitle, 0);
            Grid.SetRow(labelContactPhoneTitle, 6);

            StackPanel spContactPhone = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    labelContactPhone1,
                    labelContactPhone2
                }
            };

            topGrid.Children.Add(spContactPhone);
            Grid.SetColumn(spContactPhone, 1);
            Grid.SetRow(spContactPhone, 6);
            */

            //ticket info
            TextBlock labelStepNumber = new TextBlock()
            {
                Text = _vm.WorkTicket.WTStep
            };
            TextBlock labelDescription = new TextBlock()
            {
                Text = _vm.WorkTicket.Description
            };
            Label labelIntSerNoTitle = new Label()
            {
                Content = "Int Serial No"
            };
            TextBlock labelIntSerNo = new TextBlock()
            {
                Text = _vm.WorkTicket.InternalSerialNumber
            };
            StackPanel stepHorizontalLayout = new StackPanel()
            {
                Children =
                {
                    labelStepNumber,
                    labelDescription,
                    labelIntSerNoTitle,
                    labelIntSerNo
                }
            };

            TextBlock labelItemCode = new TextBlock();
            TextBlock labelItemDesc = new TextBlock();
            if (_vm.RepairItem != null)
            {
                labelItemCode.Text = _vm.RepairItem.ItemCode;
                labelItemDesc.Text = _vm.RepairItem.ItemCodeDesc;
            }
            StackPanel repairItemLayout = new StackPanel()
            {
               Children = {
                    labelItemCode,
                    labelItemDesc
                }
            };

            TextBox txtMfgSerialNo = new TextBox();
            txtMfgSerialNo.Text = _vm.WorkTicket.DtlMfgSerialNo;
            txtMfgSerialNo.IsEnabled = false; //until we get that functionality sorted out
            txtMfgSerialNo.TextChanged += async delegate (object sender, TextChangedEventArgs e)
            {
                _vm.WorkTicket.DtlMfgSerialNo = ((TextBox)sender).Text;
            };

            TextBlock labelMfgSerialNoDesc = new TextBlock();
            if (_vm.RepairItem != null)
            {
                labelMfgSerialNoDesc.Text = _vm.RepairItem.ItemCodeDesc; // + _vm.RepairItem.StandardUnitOfMeasure;
            }
            StackPanel mfgSerLayout = new StackPanel()
            {
                Children = {
                    txtMfgSerialNo,
                    labelMfgSerialNoDesc
                }
            };

            TextBlock labelServiceAgreementNo = new TextBlock()
            {
                Text = _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber
            };
            TextBlock labelServiceAgreementDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.ServiceAgreement.Description
            };
            StackPanel serviceAgreementLayout = new StackPanel()
            {
                Children = {
                    labelServiceAgreementNo,
                    labelServiceAgreementDesc
                }
            };
            TextBlock labelProblemCode = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlProblemCode
            };
            TextBlock labelProblemCodeDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlProblemCodeDescription
            };
            StackPanel problemCodeLayout = new StackPanel()
            {
                Children = {
                    labelProblemCode,
                    labelProblemCodeDesc
                }
            };

            TextBlock labelExceptionCode = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlCoverageExceptionCode
            };
            TextBlock labelExceptionCodeDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlaCoverageExceptionCodeDescription
            };
            StackPanel exceptionCodeLayout = new StackPanel()
            {
                Children = {
                    labelExceptionCode,
                    labelExceptionCodeDesc
                }
            };

            //service agreement grid
            Grid serviceAgreementGrid = new Grid();
            serviceAgreementGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            serviceAgreementGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            serviceAgreementGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            serviceAgreementGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            serviceAgreementGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            TextBlock labelSATitle = new TextBlock
            {
                Text = "Service Agreement",
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.WrapWithOverflow
            };
            serviceAgreementGrid.Children.Add(labelSATitle);
            Grid.SetRow(labelSATitle, 0);
            Grid.SetColumn(labelSATitle, 0);
            TextBlock labelPMTitle = new TextBlock
            {
                Text = "Preventative Maintenance",
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.WrapWithOverflow
            };
            serviceAgreementGrid.Children.Add(labelPMTitle);
            Grid.SetRow(labelPMTitle, 1);
            Grid.SetColumn(labelPMTitle, 0);
            TextBlock labelWarrantyRepairTitle = new TextBlock
            {
                Text = "Warranty Repair",
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.WrapWithOverflow
            };
            serviceAgreementGrid.Children.Add(labelWarrantyRepairTitle);
            Grid.SetRow(labelWarrantyRepairTitle, 2);
            Grid.SetColumn(labelWarrantyRepairTitle, 0);
            
            CheckBox switchWarrantyRepair = new CheckBox();
            switchWarrantyRepair.IsEnabled = false;
            if ((_vm.WorkTicket.DtlWarrantyRepair != null) &&
                (_vm.WorkTicket.DtlWarrantyRepair.ToUpper() == "Y"))
            {
                switchWarrantyRepair.IsChecked = true;
            }
            else {
                switchWarrantyRepair.IsChecked = false;
            }
            CheckBox switchServiceAgreement = new CheckBox();
            switchServiceAgreement.IsEnabled = false;
            if (_vm.WorkTicket.ServiceAgreement != null && _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber != null)
            {
                switchServiceAgreement.IsChecked = true;
            }
            else {
                switchServiceAgreement.IsChecked = false;
            }
            CheckBox switchPreventativeMaintenance = new CheckBox();
            switchPreventativeMaintenance.IsEnabled = false;
            if (_vm.WorkTicket.IsPreventativeMaintenance != null && _vm.WorkTicket.IsPreventativeMaintenance)
            {
                switchPreventativeMaintenance.IsChecked = true;
            }
            else {
                switchPreventativeMaintenance.IsChecked = false;
            }
            serviceAgreementGrid.Children.Add(switchServiceAgreement);
            Grid.SetRow(switchServiceAgreement, 0);
            Grid.SetColumn(switchServiceAgreement, 1);
            serviceAgreementGrid.Children.Add(switchWarrantyRepair);
            Grid.SetRow(switchWarrantyRepair, 2);
            Grid.SetColumn(switchWarrantyRepair, 1);
            serviceAgreementGrid.Children.Add(switchPreventativeMaintenance);
            Grid.SetRow(switchPreventativeMaintenance, 1);
            Grid.SetColumn(switchPreventativeMaintenance, 1);
            Grid.SetRowSpan(serviceAgreementGrid, 6);

            Grid stepGrid = new Grid();
            stepGrid.Margin = new Thickness(10);
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Label labelStepNoTitle = new Label
            {
                Content = "Step No",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelStepNoTitle);
            Grid.SetRow(labelStepNoTitle, 0);
            Grid.SetColumn(labelStepNoTitle, 0);
            stepGrid.Children.Add(stepHorizontalLayout);
            Grid.SetRow(stepHorizontalLayout, 0);
            Grid.SetColumn(stepHorizontalLayout, 1);
            Label labelItemCodeTitle = new Label()
            {
                Content = "Item Code",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelItemCodeTitle);
            Grid.SetRow(labelItemCodeTitle, 1);
            Grid.SetColumn(labelItemCodeTitle, 0);
            stepGrid.Children.Add(repairItemLayout);
            Grid.SetRow(repairItemLayout, 1);
            Grid.SetColumn(repairItemLayout, 1);
            Label labelMfgSerialNotitle = new Label()
            {
                Content = "Mfg Serial No",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelMfgSerialNotitle);
            Grid.SetColumn(labelMfgSerialNotitle, 0);
            Grid.SetRow(labelMfgSerialNotitle, 2);
            stepGrid.Children.Add(mfgSerLayout);
            Grid.SetColumn(mfgSerLayout, 1);
            Grid.SetRow(mfgSerLayout, 2);
            Label labelSvcAgreementTitle = new Label()
            {
                Content = "Svc Agreement",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelSvcAgreementTitle);
            Grid.SetColumn(labelSvcAgreementTitle, 0);
            Grid.SetRow(labelSvcAgreementTitle, 3);
            stepGrid.Children.Add(serviceAgreementLayout);
            Grid.SetColumn(serviceAgreementLayout, 1);
            Grid.SetRow(serviceAgreementLayout, 3);
            Label labelProblemCodeTitle = new Label()
            {
                Content = "Problem Code",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelProblemCodeTitle);
            Grid.SetColumn(labelProblemCodeTitle, 0);
            Grid.SetRow(labelProblemCodeTitle, 4);
            stepGrid.Children.Add(problemCodeLayout);
            Grid.SetColumn(problemCodeLayout, 1);
            Grid.SetRow(problemCodeLayout , 4);
            Label labelExceptionCodeTitle = new Label()
            {
                Content = "Exception Code",
                FontWeight = FontWeights.Bold
            };
            stepGrid.Children.Add(labelExceptionCodeTitle);
            Grid.SetColumn(labelExceptionCodeTitle, 0);
            Grid.SetRow(labelExceptionCodeTitle, 5);
            stepGrid.Children.Add(exceptionCodeLayout);
            Grid.SetColumn(exceptionCodeLayout, 1);
            Grid.SetRow(exceptionCodeLayout, 5);
            stepGrid.Children.Add(serviceAgreementGrid);
            Grid.SetColumn(serviceAgreementGrid, 3);
            Grid.SetRow(serviceAgreementGrid, 0);

            stackPanelMain.Children.Add(topGrid);
            stackPanelMain.Children.Add(stepGrid);

        }

    }
}
