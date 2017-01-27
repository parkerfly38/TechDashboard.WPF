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

/**************************************************************************************************
 * Page Name    TicketDetailsPage
 * Description: Ticket Details Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels.
 * 10/31/2016   DCH     Include Technician and Department at the top of the page.
 * 10/31/2016   DCH     Change page title to "SERVICE TICKET DETAIL"
 * 10/31/2016   DCH     Align the Service Agreement/Preventive Maintenance/Warranty Repair section.
 * 10/31/2016   DCH     Align the Int Serial Number to the correct section
 * 11/01/2016   DCH     Include Address Name for Ticket
 ***************************************************************************************************/

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
                // dch rkl 10/26/2016 include schedule detail
                //contentArea.Content = new ClockOutPage(_vm.WorkTicket);
                contentArea.Content = new ClockOutPage(_vm.WorkTicket, _vm.ScheduledAppointment);
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

            // dch rkl 10/31/2016 Display Technician at the top of the screen
            // change tile to SERVICE TICKET DETAIL.
            //_labelTitle.Content = "TECHNICIAN";
            _labelTitle.Content = "SERVICE TICKET DETAIL";

            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 18;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);

            // dch rkl 10/31/2016 center text
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            // dch rkl 10/31/2016 Display Technician at the top of the screen
            // change tile to SERVICE TICKET DETAIL.
            //Label labelTechnicianName = new Label()
            //{
            //    // dch rkl 10/26/2016 include technician number
            //    //Content = _vm.AppTechnician.FirstName + ' ' + _vm.AppTechnician.LastName,
            //    Content = string.Format("{0}  {1} {2}", _vm.AppTechnician.TechnicianNo, 
            //        _vm.AppTechnician.FirstName, _vm.AppTechnician.LastName),
            //    Foreground = new SolidColorBrush(Colors.White),
            //    FontSize = 18
            //};

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                // dch rkl 10/31/2016     Height = 100
                Height = 80
            };
            
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // dch rkl 10/31/2016          titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            // dch rkl 10/31/2016          titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            // dch rkl 10/31/2016 Display Technician at the top of the screen
            // change tile to SERVICE TICKET DETAIL.
            //titleLayout.Children.Add(labelTechnicianName);
            //Grid.SetColumn(labelTechnicianName, 0);
            //Grid.SetRow(labelTechnicianName, 1);

            stackPanelMain.Children.Add(titleLayout);

            // dch rkl 10/31/2016 Display Dept and Technician at the top of the screen
            // change tile to SERVICE TICKET DETAIL.
            Label labelTechnicianTitle = new Label()
            {
                Content = "Technician",
                FontWeight = FontWeights.Bold
            };
            TextBlock labelTechnicianNo = new TextBlock()
            {
                //Text = _vm.AppTechnician.TechnicianNo,
                Text = String.Format("{0}-{1}",_vm.AppTechnician.TechnicianDeptNo, _vm.AppTechnician.TechnicianNo),
                VerticalAlignment = VerticalAlignment.Center      
            };
            TextBlock labelTechnicianNm = new TextBlock()
            {
                Text = _vm.AppTechnician.FirstName + ' ' + _vm.AppTechnician.LastName,
                VerticalAlignment = VerticalAlignment.Center
            };

            TextBlock labelServiceTicketNo = new TextBlock()
            {
                Text = _vm.WorkTicket.FormattedTicketNumber,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };

            TextBlock labelServiceTicketDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.Description,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };

            TextBlock labelCustomerName = new TextBlock()
            {
                Text = _vm.Customer.CustomerName,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };

            TextBlock labelCustomerNo = new TextBlock();
            labelCustomerNo.Text = _vm.Customer.CustomerNo;
            labelCustomerNo.VerticalAlignment = VerticalAlignment.Center;       // dch rkl 10/25/2016

            TextBlock labelContactCode = new TextBlock();
            labelContactCode.Text = _vm.CustomerContact.ContactCode;
            labelContactCode.VerticalAlignment = VerticalAlignment.Center;       // dch rkl 10/25/2016

            TextBlock labelContactName = new TextBlock();
            labelContactName.Text = _vm.CustomerContact.ContactName;
            labelContactName.VerticalAlignment = VerticalAlignment.Center;       // dch rkl 10/25/2016

            StackPanel stackPanelAddress = new StackPanel();

            // dch rkl 11/1/2016 add address name
            //if ((_vm.SalesOrder.ShipToName != null) &&
            //    (_vm.SalesOrder.ShipToName.Trim().Length > 0))
            //{
            //    TextBlock labelShipToName = new TextBlock();
            //    labelShipToName.Text = _vm.SalesOrder.ShipToName;

            //    stackPanelAddress.Children.Add(labelShipToName);
            //}

            // label for address line 1
            //if ((_vm.SalesOrder.ShipToAddress1 != null) &&
            //    (_vm.SalesOrder.ShipToAddress1.Trim().Length > 0))
            //{
            //    TextBlock labelAddressLine1 = new TextBlock();
            //    labelAddressLine1.Text = _vm.SalesOrder.ShipToAddress1;

            //    stackPanelAddress.Children.Add(labelAddressLine1);
            //}

            // label for address line 2
            //if ((_vm.SalesOrder.ShipToAddress2 != null) &&
            //    (_vm.SalesOrder.ShipToAddress2.Trim().Length > 0))
            //{
            //    TextBlock labelAddressLine2 = new TextBlock();
            //    labelAddressLine2.Text = _vm.SalesOrder.ShipToAddress2;
            //    stackPanelAddress.Children.Add(labelAddressLine2);
            //}

            // label for address line 3
            //if ((_vm.SalesOrder.ShipToAddress3 != null) &&
            //    (_vm.SalesOrder.ShipToAddress3.Trim().Length > 0))
            //{
            //    TextBlock labelAddressLine3 = new TextBlock();
            //    labelAddressLine3.Text = _vm.SalesOrder.ShipToAddress3;
            //    stackPanelAddress.Children.Add(labelAddressLine3);
            //}

            // label for city/state/zip
            //TextBlock labelCityStateZip = new TextBlock();
            //labelCityStateZip.Text = _vm.SalesOrder.ShipToCity + ", " + _vm.SalesOrder.ShipToState + " " + _vm.SalesOrder.ShipToZipCode;
            //stackPanelAddress.Children.Add(labelCityStateZip);

            Label labelTelephoneTitle = new Label()
            {
                Content = "Main Phone",
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
                Content = "Shipping Phone",
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
                Content = "Contact Phone",
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

            // dch rkl 10/25/2016 phone numbers BEGIN
            //Grid phoneGrid = new Grid();
            //phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //phoneGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //phoneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star),  });
            //phoneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //phoneGrid.Children.Add(labelTelephoneTitle);
            //Grid.SetColumn(labelTelephoneTitle, 0);
            //Grid.SetRow(labelTelephoneTitle, 0);
            //phoneGrid.Children.Add(labelTelephoneNo);
            //Grid.SetColumn(labelTelephoneNo, 1);
            //Grid.SetRow(labelTelephoneNo, 0);
            //phoneGrid.Children.Add(labelShipToPhoneTitle);
            //Grid.SetColumn(labelShipToPhoneTitle, 0);
            //Grid.SetRow(labelShipToPhoneTitle, 1);
            //phoneGrid.Children.Add(labelShipToPhoneNo);
            //Grid.SetColumn(labelShipToPhoneNo, 1);
            //Grid.SetRow(labelShipToPhoneNo, 1);
            //phoneGrid.Children.Add(labelContactPhoneTitle);
            //Grid.SetColumn(labelContactPhoneTitle, 0);
            //Grid.SetRow(labelContactPhoneTitle, 2);
            //Grid.SetRowSpan(labelContactPhoneTitle, 2);
            //phoneGrid.Children.Add(labelContactPhone1);
            //Grid.SetColumn(labelContactPhone1, 1);
            //Grid.SetRow(labelContactPhone1, 2);
            //phoneGrid.Children.Add(labelContactPhone2);
            //Grid.SetColumn(labelContactPhone2, 1);
            //Grid.SetRow(labelContactPhone2, 3);
            // dch rkl 10/25/2016 phone numbers END

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
            Label labelAddressTitle1 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold
            };
            Label labelAddressTitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold
            };
            Label labelAddressTitle3 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold
            };
            Label labelAddressCSZ = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold
            };
            Grid topGrid = new Grid();
            topGrid.Margin = new Thickness(10);
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // dch rkl 10/31/2016 add row at top for Technician No
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Service Ticket
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Customer No
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Contact
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Address Name
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Address 1
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Address 2
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Address 3
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // City, State, Zip
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Main Phone
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Shipping Phone
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // Contact Phone
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });      // dch rkl 10/25/2016
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });      // dch rkl 10/25/2016
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Grid.SetColumnSpan(stackPanelAddress, 2);

            // dch rkl 10/31/2016 Display Technician at the top of the screen
            int tgRow = 0;

            // change tile to SERVICE TICKET DETAIL.
            topGrid.Children.Add(labelTechnicianTitle);
            Grid.SetColumn(labelTechnicianTitle, 0);
            Grid.SetRow(labelTechnicianTitle, tgRow);
            topGrid.Children.Add(labelTechnicianNo);
            Grid.SetColumn(labelTechnicianNo, 1);
            Grid.SetRow(labelTechnicianNo, 0);
            topGrid.Children.Add(labelTechnicianNm);
            Grid.SetColumn(labelTechnicianNm, 2);
            Grid.SetRow(labelTechnicianNm, tgRow);
            tgRow++;

            topGrid.Children.Add(labelServiceTicketTitle);
            Grid.SetColumn(labelServiceTicketTitle, 0);
            Grid.SetRow(labelServiceTicketTitle, tgRow);
            topGrid.Children.Add(labelServiceTicketNo);
            Grid.SetColumn(labelServiceTicketNo, 1);
            Grid.SetRow(labelServiceTicketNo, tgRow);
            topGrid.Children.Add(labelServiceTicketDesc);
            Grid.SetColumn(labelServiceTicketDesc, 2);
            Grid.SetRow(labelServiceTicketDesc, tgRow);
            tgRow++;

            topGrid.Children.Add(labelCustomerTitle);
            Grid.SetColumn(labelCustomerTitle, 0);
            Grid.SetRow(labelCustomerTitle, tgRow);
            topGrid.Children.Add(labelCustomerNo);
            Grid.SetColumn(labelCustomerNo, 1);
            Grid.SetRow(labelCustomerNo, tgRow);
            topGrid.Children.Add(labelCustomerName);
            Grid.SetColumn(labelCustomerName, 2);
            Grid.SetRow(labelCustomerName, tgRow);
            tgRow++;

            topGrid.Children.Add(labelContactTitle);
            Grid.SetColumn(labelContactTitle, 0);
            Grid.SetRow(labelContactTitle, tgRow);
            topGrid.Children.Add(labelContactCode);
            Grid.SetColumn(labelContactCode, 1);
            Grid.SetRow(labelContactCode, tgRow);
            topGrid.Children.Add(labelContactName);
            Grid.SetColumn(labelContactName, 2);
            Grid.SetRow(labelContactName, tgRow);
            tgRow++;

            // dch rkl 11/07/2016 Address Layout BEGIN
            //topGrid.Children.Add(labelAddressTitle);
            //Grid.SetColumn(labelAddressTitle, 0);
            //Grid.SetRow(labelAddressTitle, 4);
            //topGrid.Children.Add(stackPanelAddress);
            //Grid.SetColumn(stackPanelAddress, 1);
            //Grid.SetRow(stackPanelAddress, 4);
            TextBlock labelShipToName = new TextBlock();
            if ((_vm.SalesOrder.ShipToName != null) && (_vm.SalesOrder.ShipToName.Trim().Length > 0))
            {
                labelShipToName.Text = _vm.SalesOrder.ShipToName;
            }

            topGrid.Children.Add(labelAddressTitle);
            Grid.SetColumn(labelAddressTitle, 0);
            Grid.SetRow(labelAddressTitle, tgRow);
            topGrid.Children.Add(labelShipToName);
            Grid.SetColumn(labelShipToName, 1);
            Grid.SetRow(labelShipToName, tgRow);
            tgRow++;

            if ((_vm.SalesOrder.ShipToAddress1 != null) && (_vm.SalesOrder.ShipToAddress1.Trim().Length > 0))
            {
                TextBlock labelAddressLine1 = new TextBlock();
                labelAddressLine1.Text = _vm.SalesOrder.ShipToAddress1;
                topGrid.Children.Add(labelAddressTitle1);
                Grid.SetColumn(labelAddressTitle1, 0);
                Grid.SetRow(labelAddressTitle1, tgRow);
                topGrid.Children.Add(labelAddressLine1);
                Grid.SetColumn(labelAddressLine1, 1);
                Grid.SetRow(labelAddressLine1, tgRow);
                tgRow++;
            }

            if ((_vm.SalesOrder.ShipToAddress2 != null) && (_vm.SalesOrder.ShipToAddress2.Trim().Length > 0))
            {
                TextBlock labelAddressLine2 = new TextBlock();
                labelAddressLine2.Text = _vm.SalesOrder.ShipToAddress2;
                topGrid.Children.Add(labelAddressTitle2);
                Grid.SetColumn(labelAddressTitle2, 0);
                Grid.SetRow(labelAddressTitle2, tgRow);
                topGrid.Children.Add(labelAddressLine2);
                Grid.SetColumn(labelAddressLine2, 1);
                Grid.SetRow(labelAddressLine2, tgRow);
                tgRow++;
            }

            TextBlock labelAddressLine3 = new TextBlock();
            if ((_vm.SalesOrder.ShipToAddress3 != null) && (_vm.SalesOrder.ShipToAddress3.Trim().Length > 0))
            {
                labelAddressLine3.Text = _vm.SalesOrder.ShipToAddress3;
                topGrid.Children.Add(labelAddressTitle3);
                Grid.SetColumn(labelAddressTitle3, 0);
                Grid.SetRow(labelAddressTitle3, tgRow);
                topGrid.Children.Add(labelAddressLine3);
                Grid.SetColumn(labelAddressLine3, 1);
                Grid.SetRow(labelAddressLine3, tgRow);
                tgRow++;
            }

            TextBlock labelCityStateZip = new TextBlock();
            labelCityStateZip.Text = _vm.SalesOrder.ShipToCity + ", " + _vm.SalesOrder.ShipToState + " " + _vm.SalesOrder.ShipToZipCode;
            topGrid.Children.Add(labelAddressCSZ);
            Grid.SetColumn(labelAddressCSZ, 0);
            Grid.SetRow(labelAddressCSZ, tgRow);
            topGrid.Children.Add(labelCityStateZip);
            Grid.SetColumn(labelCityStateZip, 1);
            Grid.SetRow(labelCityStateZip, tgRow);
            tgRow++;
            // dch rkl 11/07/2016 Address Layout END

            // dch rkl 10/25/2016 phone numbers BEGIN
            topGrid.Children.Add(labelTelephoneTitle);
            Grid.SetColumn(labelTelephoneTitle, 0);
            Grid.SetRow(labelTelephoneTitle, tgRow);
            topGrid.Children.Add(labelTelephoneNo);
            Grid.SetColumn(labelTelephoneNo, 1);
            Grid.SetRow(labelTelephoneNo, tgRow);
            tgRow++;

            topGrid.Children.Add(labelShipToPhoneTitle);
            Grid.SetColumn(labelShipToPhoneTitle, 0);
            Grid.SetRow(labelShipToPhoneTitle, tgRow);
            topGrid.Children.Add(labelShipToPhoneNo);
            Grid.SetColumn(labelShipToPhoneNo, 1);
            Grid.SetRow(labelShipToPhoneNo, tgRow);
            tgRow++;

            topGrid.Children.Add(labelContactPhoneTitle);
            Grid.SetColumn(labelContactPhoneTitle, 0);
            Grid.SetRow(labelContactPhoneTitle, tgRow);
            topGrid.Children.Add(labelContactPhone1);
            Grid.SetColumn(labelContactPhone1, 1);
            Grid.SetRow(labelContactPhone1, tgRow);
            tgRow++;

            //topGrid.Children.Add(phoneGrid);
            //Grid.SetColumn(phoneGrid, 0);
            //Grid.SetRow(phoneGrid, 4);
            //Grid.SetColumnSpan(phoneGrid, 2);
            //Grid.SetRowSpan(phoneGrid, 3);
            // dch rkl 10/25/2016 phone numbers END

            //ticket info
            TextBlock labelStepNumber = new TextBlock()
            {
                Text = _vm.WorkTicket.WTStep
            };
            TextBlock labelDescription = new TextBlock()
            {
                Text = _vm.WorkTicket.Description
            };
            //Label labelIntSerNoTitle = new Label()
            //{
            //    Content = "Int Serial No"
            //};
            TextBlock labelIntSerNo = new TextBlock()
            {
                Text = _vm.WorkTicket.InternalSerialNumber
            };
            //StackPanel stepHorizontalLayout = new StackPanel()
            //{
            //    Children =
            //    {
            //        labelStepNumber,
            //        labelDescription,
            //        //labelIntSerNoTitle,           // dch rkl 10/25/2016 
            //        //labelIntSerNo                   // dch rkl 10/25/2016 
            //    }
            //};

            TextBlock labelItemCode = new TextBlock();
            TextBlock labelItemDesc = new TextBlock();
            if (_vm.RepairItem != null)
            {
                labelItemCode.Text = _vm.RepairItem.ItemCode;
                labelItemDesc.Text = _vm.RepairItem.ItemCodeDesc;
            }
            //StackPanel repairItemLayout = new StackPanel()
            //{
            //   Children = {
            //        labelItemCode,
            //        labelItemDesc
            //    }
            //};

            // dch rkl 10/26/2016 make this a textblock
            //TextBox txtMfgSerialNo = new TextBox();
            //txtMfgSerialNo.Text = _vm.WorkTicket.DtlMfgSerialNo;
            //txtMfgSerialNo.IsEnabled = false; //until we get that functionality sorted out
            //txtMfgSerialNo.TextChanged += async delegate (object sender, TextChangedEventArgs e)
            //{
            //    _vm.WorkTicket.DtlMfgSerialNo = ((TextBox)sender).Text;
            //};
            TextBlock txtMfgSerialNo = new TextBlock();
            if (_vm.RepairItem != null)
            {
                txtMfgSerialNo.Text = _vm.WorkTicket.DtlMfgSerialNo;
            }

            TextBlock labelMfgSerialNoDesc = new TextBlock();
            if (_vm.RepairItem != null)
            {
                labelMfgSerialNoDesc.Text = _vm.RepairItem.ItemCodeDesc; // + _vm.RepairItem.StandardUnitOfMeasure;
            }
            //StackPanel mfgSerLayout = new StackPanel()
            //{
            //    Children = {
            //        txtMfgSerialNo,
            //        labelMfgSerialNoDesc
            //    }
            //};

            TextBlock labelServiceAgreementNo = new TextBlock()
            {
                Text = _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber
            };
            TextBlock labelServiceAgreementDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.ServiceAgreement.Description
            };
            //StackPanel serviceAgreementLayout = new StackPanel()
            //{
            //    Children = {
            //        labelServiceAgreementNo,
            //        labelServiceAgreementDesc
            //    }
            //};
            TextBlock labelProblemCode = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlProblemCode
            };
            TextBlock labelProblemCodeDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlProblemCodeDescription
            };
            //StackPanel problemCodeLayout = new StackPanel()
            //{
            //    Children = {
            //        labelProblemCode,
            //        labelProblemCodeDesc
            //    }
            //};

            TextBlock labelExceptionCode = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlCoverageExceptionCode
            };
            TextBlock labelExceptionCodeDesc = new TextBlock()
            {
                Text = _vm.WorkTicket.DtlaCoverageExceptionCodeDescription
            };
            //StackPanel exceptionCodeLayout = new StackPanel()
            //{
            //    Children = {
            //        labelExceptionCode,
            //        labelExceptionCodeDesc
            //    }
            //};

            //service agreement grids
            //Grid serviceAgreementGrid1 = new Grid();
            //serviceAgreementGrid1.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //serviceAgreementGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //serviceAgreementGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Grid serviceAgreementGrid2 = new Grid();
            //serviceAgreementGrid2.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //serviceAgreementGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //serviceAgreementGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Grid serviceAgreementGrid3 = new Grid();
            //serviceAgreementGrid3.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //serviceAgreementGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //serviceAgreementGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock labelSATitle = new TextBlock
            {
                Text = "Service Agreement",
                FontWeight = FontWeights.Bold,
                //TextWrapping = TextWrapping.WrapWithOverflow      // dch rkl 10/26/2016 no wrapping
                TextWrapping = TextWrapping.NoWrap
            };
            //serviceAgreementGrid1.Children.Add(labelSATitle);
            //Grid.SetRow(labelSATitle, 0);
            //Grid.SetColumn(labelSATitle, 0);
            TextBlock labelPMTitle = new TextBlock
            {
                //Text = "Preventative Maintenance",
                Text = "Preventive Maintenance",      // dch rkl 10/25/2016
                FontWeight = FontWeights.Bold,
                //TextWrapping = TextWrapping.WrapWithOverflow      // dch rkl 10/26/2016 no wrapping
                TextWrapping = TextWrapping.NoWrap
            };
            //serviceAgreementGrid2.Children.Add(labelPMTitle);
            //Grid.SetRow(labelPMTitle, 1);
            //Grid.SetColumn(labelPMTitle, 0);
            TextBlock labelWarrantyRepairTitle = new TextBlock
            {
                Text = "Warranty Repair",
                FontWeight = FontWeights.Bold,
                //TextWrapping = TextWrapping.WrapWithOverflow      // dch rkl 10/26/2016 no wrapping
                TextWrapping = TextWrapping.NoWrap
            };
            //serviceAgreementGrid3.Children.Add(labelWarrantyRepairTitle);
            //Grid.SetRow(labelWarrantyRepairTitle, 2);
            //Grid.SetColumn(labelWarrantyRepairTitle, 0);
            
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
            switchServiceAgreement.HorizontalAlignment = HorizontalAlignment.Center;
            //serviceAgreementGrid1.Children.Add(switchServiceAgreement);
            //Grid.SetRow(switchServiceAgreement, 0);
            //Grid.SetColumn(switchServiceAgreement, 1);

            switchPreventativeMaintenance.HorizontalAlignment = HorizontalAlignment.Center;
            //serviceAgreementGrid2.Children.Add(switchPreventativeMaintenance);
            //Grid.SetRow(switchPreventativeMaintenance, 1);
            //Grid.SetColumn(switchPreventativeMaintenance, 1);

            switchWarrantyRepair.HorizontalAlignment = HorizontalAlignment.Center;
            //serviceAgreementGrid3.Children.Add(switchWarrantyRepair);
            //Grid.SetRow(switchWarrantyRepair, 2);
            //Grid.SetColumn(switchWarrantyRepair, 1);
            //Grid.SetRowSpan(serviceAgreementGrid, 6);

            Grid stepGrid = new Grid();
            stepGrid.Margin = new Thickness(10);
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Step No
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Step No Descr
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // int Serial No
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Item Code
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Item Code Descr
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Mfg Serial No
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Mfg Serial No Descr
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Svc Agreement
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Svc Agreement Desc
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Problem Code
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Problem Code Desc
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Exception Code
            stepGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });        // Exception Code Desc

            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stepGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            int sgRow = 0;

            Label labelStepNoTitle = new Label
            {
                Content = "Step No",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelStepNoTitle);
            Grid.SetRow(labelStepNoTitle, sgRow);
            Grid.SetColumn(labelStepNoTitle, 0);
            //stepGrid.Children.Add(stepHorizontalLayout);
            //Grid.SetRow(stepHorizontalLayout, 0);
            //Grid.SetColumn(stepHorizontalLayout, 1);
            stepGrid.Children.Add(labelStepNumber);
            Grid.SetRow(labelStepNumber, sgRow);
            Grid.SetColumn(labelStepNumber, 1);
            stepGrid.Children.Add(labelSATitle);
            Grid.SetRow(labelSATitle, sgRow);
            Grid.SetColumn(labelSATitle, 2);
            stepGrid.Children.Add(switchServiceAgreement);
            Grid.SetRow(switchServiceAgreement, sgRow);
            Grid.SetColumn(switchServiceAgreement, 3);
            sgRow++;

            Label labelStepNoTitle2 = new Label
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelStepNoTitle2);
            Grid.SetRow(labelStepNoTitle2, sgRow);
            Grid.SetColumn(labelStepNoTitle2, 0);
            stepGrid.Children.Add(labelDescription);
            Grid.SetRow(labelDescription, sgRow);
            Grid.SetColumn(labelDescription, 1);
            stepGrid.Children.Add(labelPMTitle);
            Grid.SetRow(labelPMTitle, sgRow);
            Grid.SetColumn(labelPMTitle, 2);
            stepGrid.Children.Add(switchPreventativeMaintenance);
            Grid.SetRow(switchPreventativeMaintenance, sgRow);
            Grid.SetColumn(switchPreventativeMaintenance, 3);
            sgRow++;

            Label labelIntSerNoTitle = new Label
            {
                Content = "Int Serial No",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelIntSerNoTitle);
            Grid.SetRow(labelIntSerNoTitle, sgRow);
            Grid.SetColumn(labelIntSerNoTitle, 0);
            stepGrid.Children.Add(labelIntSerNo);
            Grid.SetRow(labelIntSerNo, sgRow);
            Grid.SetColumn(labelIntSerNo, 1);
            stepGrid.Children.Add(labelWarrantyRepairTitle);
            Grid.SetRow(labelWarrantyRepairTitle, sgRow);
            Grid.SetColumn(labelWarrantyRepairTitle, 2);
            stepGrid.Children.Add(switchWarrantyRepair);
            Grid.SetRow(switchWarrantyRepair, sgRow);
            Grid.SetColumn(switchWarrantyRepair, 3);
            sgRow++;

            Label labelItemCodeTitle = new Label()
            {
                Content = "Item Code",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelItemCodeTitle);
            Grid.SetRow(labelItemCodeTitle, sgRow);
            Grid.SetColumn(labelItemCodeTitle, 0);
            //stepGrid.Children.Add(repairItemLayout);
            //Grid.SetRow(repairItemLayout, sgRow);
            //Grid.SetColumn(repairItemLayout, 1);
            stepGrid.Children.Add(labelItemCode);
            Grid.SetRow(labelItemCode, sgRow);
            Grid.SetColumn(labelItemCode, 1);
            sgRow++;

            Label labelItemCodeTitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelItemCodeTitle2);
            Grid.SetRow(labelItemCodeTitle2, sgRow);
            Grid.SetColumn(labelItemCodeTitle2, 0);
            stepGrid.Children.Add(labelItemDesc);
            Grid.SetRow(labelItemDesc, sgRow);
            Grid.SetColumn(labelItemDesc, 1);
            sgRow++;

            Label labelMfgSerialNotitle = new Label()
            {
                Content = "Mfg Serial No",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelMfgSerialNotitle);
            Grid.SetColumn(labelMfgSerialNotitle, 0);
            Grid.SetRow(labelMfgSerialNotitle, sgRow);
            //stepGrid.Children.Add(mfgSerLayout);
            //Grid.SetColumn(mfgSerLayout, 1);
            //Grid.SetRow(mfgSerLayout, sgRow);
            stepGrid.Children.Add(txtMfgSerialNo);
            Grid.SetColumn(txtMfgSerialNo, 1);
            Grid.SetRow(txtMfgSerialNo, sgRow);
            sgRow++;

            Label labelMfgSerialNotitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelMfgSerialNotitle2);
            Grid.SetColumn(labelMfgSerialNotitle2, 0);
            Grid.SetRow(labelMfgSerialNotitle2, sgRow);
            stepGrid.Children.Add(labelMfgSerialNoDesc);
            Grid.SetColumn(labelMfgSerialNoDesc, 1);
            Grid.SetRow(labelMfgSerialNoDesc, sgRow);
            sgRow++;

            Label labelSvcAgreementTitle = new Label()
            {
                Content = "Svc Agreement",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelSvcAgreementTitle);
            Grid.SetColumn(labelSvcAgreementTitle, 0);
            Grid.SetRow(labelSvcAgreementTitle, sgRow);
            //stepGrid.Children.Add(serviceAgreementLayout);
            //Grid.SetColumn(serviceAgreementLayout, 1);
            //Grid.SetRow(serviceAgreementLayout, 4);
            stepGrid.Children.Add(labelServiceAgreementNo);
            Grid.SetColumn(labelServiceAgreementNo, 1);
            Grid.SetRow(labelServiceAgreementNo, sgRow);
            sgRow++;

            Label labelSvcAgreementTitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelSvcAgreementTitle2);
            Grid.SetColumn(labelSvcAgreementTitle2, 0);
            Grid.SetRow(labelSvcAgreementTitle2, sgRow);
            stepGrid.Children.Add(labelServiceAgreementDesc);
            Grid.SetColumn(labelServiceAgreementDesc, 1);
            Grid.SetRow(labelServiceAgreementDesc, sgRow);
            sgRow++;

            Label labelProblemCodeTitle = new Label()
            {
                Content = "Problem Code",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelProblemCodeTitle);
            Grid.SetColumn(labelProblemCodeTitle, 0);
            Grid.SetRow(labelProblemCodeTitle, sgRow);
            //stepGrid.Children.Add(problemCodeLayout);
            //Grid.SetColumn(problemCodeLayout, 1);
            //Grid.SetRow(problemCodeLayout , 5);
            stepGrid.Children.Add(labelProblemCode);
            Grid.SetColumn(labelProblemCode, 1);
            Grid.SetRow(labelProblemCode, sgRow);
            sgRow++;

            Label labelProblemCodeTitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelProblemCodeTitle2);
            Grid.SetColumn(labelProblemCodeTitle2, 0);
            Grid.SetRow(labelProblemCodeTitle2, sgRow);
            stepGrid.Children.Add(labelProblemCodeDesc);
            Grid.SetColumn(labelProblemCodeDesc, 1);
            Grid.SetRow(labelProblemCodeDesc, sgRow);
            sgRow++;

            Label labelExceptionCodeTitle = new Label()
            {
                Content = "Exception Code",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelExceptionCodeTitle);
            Grid.SetColumn(labelExceptionCodeTitle, 0);
            Grid.SetRow(labelExceptionCodeTitle, sgRow);
            //stepGrid.Children.Add(exceptionCodeLayout);
            //Grid.SetColumn(exceptionCodeLayout, 1);
            //Grid.SetRow(exceptionCodeLayout, 6);
            stepGrid.Children.Add(labelExceptionCode);
            Grid.SetColumn(labelExceptionCode, 1);
            Grid.SetRow(labelExceptionCode, sgRow);
            sgRow++;

            Label labelExceptionCodeTitle2 = new Label()
            {
                Content = " ",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center        // dch rkl 10/25/2016
            };
            stepGrid.Children.Add(labelExceptionCodeTitle2);
            Grid.SetColumn(labelExceptionCodeTitle2, 0);
            Grid.SetRow(labelExceptionCodeTitle2, sgRow);
            stepGrid.Children.Add(labelExceptionCodeDesc);
            Grid.SetColumn(labelExceptionCodeDesc, 1);
            Grid.SetRow(labelExceptionCodeDesc, sgRow);
            sgRow++;

            //stepGrid.Children.Add(serviceAgreementGrid1);
            //Grid.SetColumn(serviceAgreementGrid1, 3);
            //Grid.SetRow(serviceAgreementGrid1, 0);

            //stepGrid.Children.Add(serviceAgreementGrid2);
            //Grid.SetColumn(serviceAgreementGrid2, 3);
            //Grid.SetRow(serviceAgreementGrid2, 1);

            //stepGrid.Children.Add(serviceAgreementGrid3);
            //Grid.SetColumn(serviceAgreementGrid3, 3);
            //Grid.SetRow(serviceAgreementGrid3, 2);

            stackPanelMain.Children.Add(topGrid);
            stackPanelMain.Children.Add(stepGrid);

        }
    }
}
