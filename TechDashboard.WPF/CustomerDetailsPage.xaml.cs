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
    /// Interaction logic for CustomerDetailsPage.xaml
    /// </summary>
    public partial class CustomerDetailsPage : UserControl
    {
        CustomerDetailsPageViewModel _vm;
        Label _labelTitle;
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

            //  Create a label for the technician list
            _labelTitle = new Label();
            _labelTitle.Content = "CUSTOMER DETAILS";
            _labelTitle.FontSize = 22;
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Button btnBack = new Button();

            StackPanel spBackButton = new StackPanel();
            Image imBackButton = new Image();
            imBackButton.Source = new BitmapImage(new Uri("Resources/arrow-111-64.png", UriKind.Relative));
            imBackButton.Height = 32;
            imBackButton.Width = 32;
            spBackButton.Children.Add(imBackButton);
            btnBack.Content = spBackButton;
            btnBack.HorizontalAlignment = HorizontalAlignment.Left;
            btnBack.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3498DB"));
            btnBack.BorderBrush = null;
            btnBack.Width = 32;

            btnBack.Click += BtnBack_Click;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);
            titleLayout.Children.Add(btnBack);
            Grid.SetColumn(btnBack, 0);
            Grid.SetRow(btnBack, 1);

            // create a stack layout to hold all the customer data
            StackPanel stackLayoutCustomerData = new StackPanel();
            //stackLayoutCustomerData.Padding = 30;

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // add customer number
            Label labelCustomerNumber = new Label();
            labelCustomerNumber.Content = _vm.Customer.FormattedCustomerNumber;
            labelCustomerNumber.Foreground = asbestos;
            labelCustomerNumber.FontWeight = FontWeights.Bold;
            //stackLayoutCustomerData.Children.Add(labelCustomerNumber);

            Label labelCustomerNumberTitle = new Label()
            {
                Content = "Customer No:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelCustomerNumberTitle);
            Grid.SetColumn(labelCustomerNumberTitle, 0);
            Grid.SetRow(labelCustomerNumberTitle, 0);
            topGrid.Children.Add(labelCustomerNumber);
            Grid.SetColumn(labelCustomerNumber, 1);
            Grid.SetRow(labelCustomerNumber, 0);

            // add customer name
            Label labelCustomerName = new Label();
            labelCustomerName.Content = _vm.Customer.CustomerName;
            labelCustomerName.Foreground = asbestos;
            //stackLayoutCustomerData.Children.Add(labelCustomerName);

            Label labelCustomerNameTitle = new Label()
            {
                Content = "Customer Name:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelCustomerNameTitle);
            Grid.SetColumn(labelCustomerNameTitle, 0);
            Grid.SetRow(labelCustomerNameTitle, 1);
            topGrid.Children.Add(labelCustomerName);
            Grid.SetColumn(labelCustomerName, 1);
            Grid.SetRow(labelCustomerName, 1);

            // add address line 1
            Label labelAddressLine1 = new Label();
            labelAddressLine1.Content = _vm.Customer.AddressLine1;
            labelAddressLine1.Foreground = asbestos;
            //stackLayoutCustomerData.Children.Add(labelAddressLine1);

            Label labelAddressTitle = new Label();
            labelAddressTitle.Content = "Address:";
            labelAddressTitle.Foreground = asbestos;
            labelAddressTitle.FontWeight = FontWeights.Bold;

            topGrid.Children.Add(labelAddressTitle);
            Grid.SetColumn(labelAddressTitle, 0);
            Grid.SetRow(labelAddressTitle, 2);
            topGrid.Children.Add(labelAddressLine1);
            Grid.SetColumn(labelAddressLine1, 1);
            Grid.SetRow(labelAddressLine1, 2);

            // add address line 2 if it exists
            if (_vm.Customer.AddressLine2 != null)
            {
                Label labelAddressLine2 = new Label();
                labelAddressLine2.Content = _vm.Customer.AddressLine2;
                labelAddressLine2.Foreground = asbestos;
                //stackLayoutCustomerData.Children.Add(labelAddressLine2);
                topGrid.Children.Add(labelAddressLine2);
                Grid.SetColumn(labelAddressLine2, 1);
                Grid.SetRow(labelAddressLine2, 3);
            }

            // add address line 3 if it exists
            if (_vm.Customer.AddressLine3 != null)
            {
                Label labelAddressLine3 = new Label();
                labelAddressLine3.Content = _vm.Customer.AddressLine3;
                labelAddressLine3.Foreground = asbestos;
                //stackLayoutCustomerData.Children.Add(labelAddressLine3);
                topGrid.Children.Add(labelAddressLine3);
                Grid.SetColumn(labelAddressLine3, 1);
                Grid.SetRow(labelAddressLine3, 4);
            }

            // add city/state/zip
            Label labelCityStateZip = new Label();
            labelCityStateZip.Content = _vm.Customer.City + ", " + _vm.Customer.State + "  " + _vm.Customer.ZipCode;
            labelCityStateZip.Foreground = asbestos;

            Label labelCityStateZipTitle = new Label();
            labelCityStateZipTitle.Content = "City/State/Zip:";
            labelCityStateZipTitle.FontWeight = FontWeights.Bold;
            labelCityStateZipTitle.Foreground = asbestos;

            topGrid.Children.Add(labelCityStateZipTitle);
            Grid.SetColumn(labelCityStateZipTitle, 0);
            Grid.SetRow(labelCityStateZipTitle, 5);
            topGrid.Children.Add(labelCityStateZip);
            Grid.SetColumn(labelCityStateZip, 1);
            Grid.SetRow(labelCityStateZip, 5);

            // add phone number
            Label labelPhoneNumber = new Label();
            labelPhoneNumber.Content = _vm.Customer.TelephoneNo;
            if (_vm.Customer.TelephoneExt != null)
            {
                labelPhoneNumber.Content = labelPhoneNumber.Content + " ext. " + _vm.Customer.TelephoneExt;
            }
            labelPhoneNumber.Foreground = asbestos;

            Label labelPhoneTitle = new Label();
            labelPhoneTitle.Content = "Phone:";
            labelPhoneTitle.Foreground = asbestos;
            labelPhoneTitle.FontWeight = FontWeights.Bold;

            topGrid.Children.Add(labelPhoneTitle);
            Grid.SetColumn(labelPhoneTitle, 0);
            Grid.SetRow(labelPhoneTitle, 6);
            topGrid.Children.Add(labelPhoneNumber);
            Grid.SetColumn(labelPhoneNumber, 1);
            Grid.SetRow(labelPhoneNumber, 6);

            // add contact
            string contactCodeName = "";
            foreach (App_CustomerContact cont in _vm.CustomerContacts)
            {
                contactCodeName = string.Format("{0} - {1}", cont.ContactCode, cont.ContactName);
            }

            Label labelContact = new Label();
            labelContact.Content = contactCodeName;
            labelContact.Foreground = asbestos;

            Label labelContactTitle = new Label();
            labelContactTitle.Content = "Contact:";
            labelContactTitle.FontWeight = FontWeights.Bold;
            labelContactTitle.Foreground = asbestos;

            topGrid.Children.Add(labelContactTitle);
            Grid.SetColumn(labelContactTitle, 0);
            Grid.SetRow(labelContactTitle, 7);
            topGrid.Children.Add(labelContact);
            Grid.SetColumn(labelContact, 1);
            Grid.SetRow(labelContact, 7);

            stackLayoutCustomerData.Children.Add(topGrid);

            // Add a button to show the map
            Button buttonShowMap = new Button();
            TextBlock showMapTextBlock = new TextBlock();
            showMapTextBlock.Text = "MAP";
            showMapTextBlock.Foreground = new SolidColorBrush(Colors.White);
            buttonShowMap.Background = asbestos;
            showMapTextBlock.FontWeight = FontWeights.Bold;
            Image showmapImage = new Image();
            showmapImage.Source = new BitmapImage(new Uri("Resources/map.png", UriKind.Relative));
            buttonShowMap.Height = 80;
            buttonShowMap.Click += ButtonShowMap_Clicked;
            StackPanel buttonShowMapStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { showmapImage, showMapTextBlock }
            };
            buttonShowMap.Content = buttonShowMapStackPanel;

            stackLayoutCustomerData.Children.Add(buttonShowMap);

            // Create the actual list for contacts

            // Create a template to display each contact in the list
            //stackLayoutCustomerData.Children.Add(
            //    new Xamarin.Forms.Label()
            //    {
            //        Text = "Contacts",
            //        FontFamily = Device.OnPlatform("OpenSans-Bold", null, null),
            //        HorizontalTextAlignment = TextAlignment.Center,
            //        BackgroundColor = asbestos,
            //        TextColor = Color.White
            //    });

            //var dataTemplateCustomerContact = new DataTemplate(typeof(CustomerContactViewCell));

            //_listViewContacts = new ListView()
            //{
            //    HasUnevenRows = true,
            //    HorizontalOptions = LayoutOptions.Fill,
            //    SeparatorVisibility = SeparatorVisibility.None,
            //    BackgroundColor = Color.White,

            //    ItemsSource = _vm.CustomerContacts,
            //    ItemTemplate = dataTemplateCustomerContact
            //};
            //_listViewContacts.BindingContext = _vm.CustomerContacts;
            //stackLayoutCustomerData.Children.Add(_listViewContacts);

            // On to customer contacts!
            // Create a template to display each customer contact in a list
            //var dataTemplateCustomerContact = new DataTemplate(typeof(CustomerContactViewCell));

            // Create the actual list
            //        Xamarin.Forms.ListView listViewCustomerContacts = new ListView()
            //        {
            //            HasUnevenRows = true,
            //            ItemsSource = _vm.CustomerContacts,
            //            ItemTemplate = dataTemplateCustomerContact,
            //SeparatorColor = asbestos
            //        };
            //        listViewCustomerContacts.BindingContext = _vm.CustomerContacts;

            // put it all together
            gridMain.Children.Add(new StackPanel()
            {
                
                Children =
                {
                    titleLayout,
                    stackLayoutCustomerData,
                    //new Xamarin.Forms.Label() { Text = "Contacts:", FontFamily = Device.OnPlatform("OpenSans-Bold",null,null), TextColor = asbestos },
                    //listViewCustomerContacts
                }
            });
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

        private void ButtonShowMap_Clicked(object sender, RoutedEventArgs e)
        {
            // puke
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
