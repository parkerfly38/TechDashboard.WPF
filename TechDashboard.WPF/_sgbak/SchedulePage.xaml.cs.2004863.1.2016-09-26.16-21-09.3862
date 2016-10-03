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

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : UserControl
    {

        SchedulePageViewModel _vm;
        public static readonly RoutedEvent SelectedTicketEvent = EventManager.RegisterRoutedEvent("SelectedTicketEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SchedulePage));

        public event RoutedEventHandler SelectedTicket
        {
            add { AddHandler(SelectedTicketEvent, value); }
            remove { RemoveHandler(SelectedTicketEvent, value); }
        }

        public SchedulePage()
        {
            InitializeComponent();
            _vm = new SchedulePageViewModel();
            this.DataContext = _vm;
            gridSchedule.ItemsSource = _vm.ScheduleDetails;
            gridSchedule.MouseDoubleClick += GridSchedule_MouseDoubleClick;
        }

        private void GridSchedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SchedulePage.SelectedTicketEvent, sender);
            RaiseEvent(newEventArgs);
        }

        private void buttonFilter_Click(object sender, RoutedEventArgs e)
        {
            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            DateTime lowerLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysBefore) * (-1))).Date;
            DateTime upperLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysAfter))).Date;
            
            if (lowerLimit > filterStartDate.SelectedDate || upperLimit < filterEndDate.SelectedDate)
            {
                MessageBox.Show("These dates exceed your application settings.  Please modify your range in there to import more data.","Update Settings", MessageBoxButton.OK);
            }

            _vm.filterScheduledAppointments((DateTime)filterStartDate.SelectedDate, (DateTime)filterEndDate.SelectedDate);
        }
    }
}
