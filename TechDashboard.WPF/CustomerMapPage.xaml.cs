using Microsoft.Maps.MapControl.WPF;
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
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for CustomerMapPage.xaml
    /// </summary>
    public partial class CustomerMapPage : UserControl
    {
        protected CustomerMapPageViewModel _vm;
        protected Map _map;
        string _parentPage;
        App_ScheduledAppointment _scheduledAppointment;
        App_Customer _customer;

        public CustomerMapPage(XmlDocument customerLocation, App_ScheduledAppointment scheduledAppointment, string parentPage = "TicketDetails")
        {
            _vm = new CustomerMapPageViewModel(customerLocation);
            _scheduledAppointment = scheduledAppointment;
            _parentPage = parentPage;
            InitializeComponent();
            Initialize();
        }

        public CustomerMapPage(XmlDocument customerLocation, App_ScheduledAppointment scheduledAppointment, App_Customer customer, string parentPage = "CustomerDetails")
        {
            _vm = new CustomerMapPageViewModel(customerLocation);
            _scheduledAppointment = scheduledAppointment;
            _parentPage = parentPage;
            _customer = customer;
            InitializeComponent();
            Initialize();
        }

        public CustomerMapPage()
        {
            _vm = new CustomerMapPageViewModel();
            InitializeComponent();
            Initialize();
        }

        protected void Initialize()
        {
            btnBack.Click += BtnBack_Click;
            XmlDocument locations = _vm.CustomerLocation;
            //XmlElement root = locations.DocumentElement;
            var nsmgr = new XmlNamespaceManager(locations.NameTable);
            nsmgr.AddNamespace("a", "http://schemas.microsoft.com/search/local/ws/rest/v1");
            XmlNodeList pointNode = locations.SelectNodes("//a:Point",nsmgr);
            double latitude = Convert.ToDouble(pointNode[0]["Latitude"].InnerText);
            double longitude = Convert.ToDouble(pointNode[0]["Longitude"].InnerText);
            Location location = new Location(latitude, longitude);
            mapCustomer.Children.Add(
                new Pushpin()
                {
                    Location = location
                });
            mapCustomer.Center = location;
            // puke
            // Create the map
            /*_map = new Map()
            {
                Center = Location
            };

            // Add the customer location pin 
            _map.Pins.Add(_vm.CustomerLocationPin);
            _vm.CustomerLocationPin.Clicked += CustomerLocationPin_Clicked;
            // center on the new pin
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(_vm.CustomerLocationPin.Position, Distance.FromMiles(0.5)));

            // add to screen contents for drawing
            Content = new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    _map
                }
            };*/
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            if (_parentPage == "TicketDetails")
                contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
            else
                contentArea.Content = new CustomerDetailsPage(_customer, _scheduledAppointment);
        }
    }
}
