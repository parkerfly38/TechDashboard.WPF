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
 * Page Name    CustomerDetailsPage
 * Description: Customer Details Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/31/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 11/23/2016   DCH     Remove Back Arrow and use Cancel Button to be consistent across pages.
 * 01/12/2017   DCH     Make sure this displays the customer contact, not the ticket contact.
 * 01/23/2017   DCH     Do no inlcude division if AR_Options.Divisions parameter is set to "N"
 * 01/23/2017   DCH     Move layout from code-behind to Xaml 
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for CustomerDetailsPage.xaml
    /// </summary>
    public partial class CustomerDetailsPage : UserControl
    {
        CustomerDetailsPageViewModel _vm;
        //Label _labelTitle;
        App_ScheduledAppointment _scheduledAppointment;
        
        public CustomerDetailsPage(App_Customer customer, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new CustomerDetailsPageViewModel(customer);
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();
            SetPageDisplay();
        }
        public CustomerDetailsPage()
        {
            _vm = new CustomerDetailsPageViewModel();
            InitializeComponent();
            SetPageDisplay();
        }

        protected void SetPageDisplay()
        {

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));

            // dch rkl 10/31/2016
            SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));

            // dch rkl 11/23/2016
            SolidColorBrush red = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));

            //btnBack.Click += BtnBack_Click;

            // create a stack layout to hold all the customer data
            //StackPanel stackLayoutCustomerData = new StackPanel();

            //Grid topGrid = new Grid();
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            //topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // add customer number
            //Label labelCustomerNumber = new Label();
            labelCustomerNumber.Content = _vm.Customer.FormattedCustomerNumber;
            //labelCustomerNumber.Foreground = asbestos;
            //labelCustomerNumber.FontWeight = FontWeights.Bold;

            //Label labelCustomerNumberTitle = new Label()
            //{
            //    Content = "Customer No",
            //    FontWeight = FontWeights.Bold,
            //    Foreground = asbestos
            //};

            //topGrid.Children.Add(labelCustomerNumberTitle);
            //Grid.SetColumn(labelCustomerNumberTitle, 0);
            //Grid.SetRow(labelCustomerNumberTitle, 0);
            //topGrid.Children.Add(labelCustomerNumber);
            //Grid.SetColumn(labelCustomerNumber, 1);
            //Grid.SetRow(labelCustomerNumber, 0);

            // add customer name
            //Label labelCustomerName = new Label();
            labelCustomerName.Content = _vm.Customer.CustomerName;
            //labelCustomerName.Foreground = asbestos;
            //stackLayoutCustomerData.Children.Add(labelCustomerName);

            //Label labelCustomerNameTitle = new Label()
            //{
            //    Content = "Customer Name",
            //    FontWeight = FontWeights.Bold,
            //    Foreground = asbestos
            //};

            //topGrid.Children.Add(labelCustomerNameTitle);
            //Grid.SetColumn(labelCustomerNameTitle, 0);
            //Grid.SetRow(labelCustomerNameTitle, 1);
            //topGrid.Children.Add(labelCustomerName);
            //Grid.SetColumn(labelCustomerName, 1);
            //Grid.SetRow(labelCustomerName, 1);

            // add address line 1
            //Label labelAddressLine1 = new Label();
            labelAddressLine1.Content = _vm.Customer.AddressLine1;
            //labelAddressLine1.Foreground = asbestos;
            //stackLayoutCustomerData.Children.Add(labelAddressLine1);

            //Label labelAddressTitle = new Label();
            //labelAddressTitle.Content = "Address";
            //labelAddressTitle.Foreground = asbestos;
            //labelAddressTitle.FontWeight = FontWeights.Bold;

            //topGrid.Children.Add(labelAddressTitle);
            //Grid.SetColumn(labelAddressTitle, 0);
            //Grid.SetRow(labelAddressTitle, 2);
            //topGrid.Children.Add(labelAddressLine1);
            //Grid.SetColumn(labelAddressLine1, 1);
            //Grid.SetRow(labelAddressLine1, 2);

            // add address line 2 if it exists
            if (_vm.Customer.AddressLine2 != null)
            {
                //Label labelAddressLine2 = new Label();
                labelAddressLine2.Content = _vm.Customer.AddressLine2;
                //labelAddressLine2.Foreground = asbestos;
                //topGrid.Children.Add(labelAddressLine2);
                //Grid.SetColumn(labelAddressLine2, 1);
                //Grid.SetRow(labelAddressLine2, 3);
            }

            // add address line 3 if it exists
            if (_vm.Customer.AddressLine3 != null)
            {
                //Label labelAddressLine3 = new Label();
                labelAddressLine3.Content = _vm.Customer.AddressLine3;
                //labelAddressLine3.Foreground = asbestos;
                //topGrid.Children.Add(labelAddressLine3);
                //Grid.SetColumn(labelAddressLine3, 1);
                //Grid.SetRow(labelAddressLine3, 4);
            }

            // add city/state/zip
            //Label labelCityStateZip = new Label();
            labelCityStateZip.Content = _vm.Customer.City + ", " + _vm.Customer.State + "  " + _vm.Customer.ZipCode;
            //labelCityStateZip.Foreground = asbestos;

            //Label labelCityStateZipTitle = new Label();
            //labelCityStateZipTitle.Content = "City/State/Zip";
            //labelCityStateZipTitle.FontWeight = FontWeights.Bold;
            //labelCityStateZipTitle.Foreground = asbestos;

            //topGrid.Children.Add(labelCityStateZipTitle);
            //Grid.SetColumn(labelCityStateZipTitle, 0);
            //Grid.SetRow(labelCityStateZipTitle, 5);
            //topGrid.Children.Add(labelCityStateZip);
            //Grid.SetColumn(labelCityStateZip, 1);
            //Grid.SetRow(labelCityStateZip, 5);

            // add phone number
            //Label labelPhoneNumber = new Label();
            labelPhoneNumber.Content = _vm.Customer.TelephoneNo;
            if (_vm.Customer.TelephoneExt != null)
            {
                labelPhoneNumber.Content = labelPhoneNumber.Content + " EXT. " + _vm.Customer.TelephoneExt;
            }
            //labelPhoneNumber.Foreground = asbestos;

            //Label labelPhoneTitle = new Label();
            //labelPhoneTitle.Content = "Phone";
            //labelPhoneTitle.Foreground = asbestos;
            //labelPhoneTitle.FontWeight = FontWeights.Bold;

            //topGrid.Children.Add(labelPhoneTitle);
            //Grid.SetColumn(labelPhoneTitle, 0);
            //Grid.SetRow(labelPhoneTitle, 6);
            //topGrid.Children.Add(labelPhoneNumber);
            //Grid.SetColumn(labelPhoneNumber, 1);
            //Grid.SetRow(labelPhoneNumber, 6);

            // add contact
            string contactCodeName = "";          
            foreach (App_CustomerContact cont in _vm.CustomerContacts)
            {
                // dch rkl 01/12/2017 Make sure this displays the customer contact, not the ticket contact BEGIN
                if (cont.ContactCode == _vm.Customer.ContactCode)
                {
                    contactCodeName = string.Format("{0} - {1}", cont.ContactCode, cont.ContactName);
                    break;
                }
                //contactCodeName = string.Format("{0} - {1}", cont.ContactCode, cont.ContactName);
                // dch rkl 01/12/2017 Make sure this displays the customer contact, not the ticket contact END
            }

            //Label labelContact = new Label();
            labelContact.Content = contactCodeName;
            //labelContact.Foreground = asbestos;

            //Label labelContactTitle = new Label();
            //labelContactTitle.Content = "Contact";
            //labelContactTitle.FontWeight = FontWeights.Bold;
            //labelContactTitle.Foreground = asbestos;

            //topGrid.Children.Add(labelContactTitle);
            //Grid.SetColumn(labelContactTitle, 0);
            //Grid.SetRow(labelContactTitle, 7);
            //topGrid.Children.Add(labelContact);
            //Grid.SetColumn(labelContact, 1);
            //Grid.SetRow(labelContact, 7);

            //stackLayoutCustomerData.Children.Add(topGrid);

            // dch rkl 11/23/2016 Add a button to go back
            //Button buttonBack = new Button();
            //TextBlock backTextBlock = new TextBlock();
            //backTextBlock.Text = "BACK";
            //backTextBlock.Foreground = new SolidColorBrush(Colors.White);
            //backTextBlock.FontSize = 16;
            //buttonBack.Background = red;
            //backTextBlock.FontWeight = FontWeights.Bold;
            //buttonBack.Height = 40;
            //buttonBack.Width = 345;
            //buttonBack.Click += BtnBack_Click;
            //buttonBack.Content = backTextBlock;

            // Add a button to show the map
            //Button buttonShowMap = new Button();
            //TextBlock showMapTextBlock = new TextBlock();
            //showMapTextBlock.Text = "MAP";
            //showMapTextBlock.Foreground = new SolidColorBrush(Colors.White);
            //showMapTextBlock.FontSize = 16;     // dch rkl 10/31/2016
            //buttonShowMap.Background = emerald;    // dch rkl 10/31/2016
            //showMapTextBlock.FontWeight = FontWeights.Bold;
            //Image showmapImage = new Image();
            //showmapImage.Source = new BitmapImage(new Uri("Resources/map.png", UriKind.Relative));
            //buttonShowMap.Height = 40;      // dch rkl 11/22/2016 change height
            //buttonShowMap.Width = 345;
            //buttonShowMap.Click += ButtonShowMap_Clicked;
            //StackPanel buttonShowMapStackPanel = new StackPanel()
            //{
            //    Orientation = Orientation.Horizontal,
            //    Children = { showmapImage, showMapTextBlock }
            //};
            //buttonShowMap.Content = buttonShowMapStackPanel;

            //Grid btnGrid = new Grid();
            //btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(42, GridUnitType.Pixel) });
            //btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(350, GridUnitType.Pixel) });
            //btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(350, GridUnitType.Pixel) });

            //btnGrid.Children.Add(buttonShowMap);
            //Grid.SetColumn(buttonShowMap, 0);
            //Grid.SetRow(buttonShowMap, 0);

            //btnGrid.Children.Add(buttonBack);
            //Grid.SetColumn(buttonBack, 1);
            //Grid.SetRow(buttonBack, 0);

            //stackLayoutCustomerData.Children.Add(btnGrid);


            // put it all together
            //gridMain.Children.Add(new StackPanel()
            //{                
            //    Children =
            //    {
            //        stackLayoutCustomerData
            //    }
            //});
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

        private void ButtonShowMap_Clicked(object sender, RoutedEventArgs e)
        {
            // TODO
            //await Navigation.PushAsync(new CustomerMapPage());
            //Geocoder geoCoder = new Geocoder();
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
            contentArea.Content = new CustomerMapPage(location, _scheduledAppointment, _vm.Customer, "CustomerDetails");
        }
    }
}
