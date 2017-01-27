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
 * Page Name    SchedulePage
 * Description: Schedule Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels.
 * 10/27/2017   DCH     Validate the Start and End Dates in the filter to prevent error.
 * 11/01/2016   DCH     When user enters on a line in the list of tickets, treat like a double-click.
 * 11/22/2016   DCH     On Focus, select all text for time
 * 12/01/2016   DCH     If dates are outside of acceptable range in settings, set them to settings range.
 **************************************************************************************************/
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
            gridSchedule.PreviewKeyDown += GridSchedule_PreviewKeyDown;       // dch rkl 11/01/2016 Enter to select ticket

            // dch rkl 11/22/2016 select full text on focus
            filterStartDate.GotFocus += textbox_GotFocus;     
            filterEndDate.GotFocus += textbox_GotFocus;      

            filterStartDate.Focusable = true;
            Keyboard.Focus(filterStartDate);

            //bk 01/24/2017 if previous filter set - application only
            if (App.Current.Properties["filterStartDate"] != null)
            {
                _vm.DefaultStartDate = (DateTime)App.Current.Properties["filterStartDate"];
                filterStartDate.SelectedDate = (DateTime)App.Current.Properties["filterStartDate"];
            }
            if (App.Current.Properties["filterEndDate"] != null)
            {
                _vm.DefaultEndDate = (DateTime)App.Current.Properties["filterEndDate"];
                filterEndDate.SelectedDate = (DateTime)App.Current.Properties["filterEndDate"];
            }
            if (App.Current.Properties["filterStartDate"] != null && App.Current.Properties["filterEndDate"] != null)
            {

                _vm.filterScheduledAppointments((DateTime)filterStartDate.SelectedDate, (DateTime)filterEndDate.SelectedDate);
            }
                
        }

        private void GridSchedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // dch rkl 10/14/2016 prevent error when invalid row selected
            if (gridSchedule.SelectedIndex > -1)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(SchedulePage.SelectedTicketEvent, sender);
                RaiseEvent(newEventArgs);
            }
        }

        // dch rkl 11/01/2016 Grid View "Enter"
        private void GridSchedule_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // dch rkl 10/14/2016 prevent error when invalid row selected
            if (gridSchedule.SelectedIndex > -1 && e.Key == Key.Enter)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(SchedulePage.SelectedTicketEvent, sender);
                RaiseEvent(newEventArgs);
            }
        }

        private void buttonFilter_Click(object sender, RoutedEventArgs e)
        {
            // dch rkl 10/27/2016 Validate the dates that are input BEGIN
            DateTime dtStart;
            DateTime dtEnd;
            if (DateTime.TryParse(filterStartDate.SelectedDate.ToString(), out dtStart) == false)
            {
                MessageBox.Show("Invalid Start Date", "Invalid Date", MessageBoxButton.OK);
            }
            else if (DateTime.TryParse(filterEndDate.SelectedDate.ToString(), out dtEnd) == false)
            {
                MessageBox.Show("Invalid End Date", "Invalid Date", MessageBoxButton.OK);
            }
            else
            {
                // dch rkl 10/27/2016 Validate the dates that are input END

                App_Settings appSettings = App.Database.GetApplicationSettings();
                DateTime lowerLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysBefore) * (-1))).Date;
                DateTime upperLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysAfter))).Date;

                App.Current.Properties["filterStartDate"] = filterStartDate.SelectedDate;
                App.Current.Properties["filterEndDate"] = filterEndDate.SelectedDate;

                // dch rkl 12/01/2016 if dates are outside of acceptable range, change them. BEGIN
                //if (lowerLimit > filterStartDate.SelectedDate || upperLimit < filterEndDate.SelectedDate)
                //{
                //    MessageBox.Show("These dates exceed your application settings.  Please modify your range in there to import more data.", "Update Settings", MessageBoxButton.OK);
                //}
                if (lowerLimit > filterStartDate.SelectedDate) { filterStartDate.Text = lowerLimit.ToShortDateString(); }
                if (upperLimit < filterEndDate.SelectedDate) { filterEndDate.Text = upperLimit.ToShortDateString(); }
                // dch rkl 12/01/2016 if dates are outside of acceptable range, change them. END

                _vm.filterScheduledAppointments((DateTime)filterStartDate.SelectedDate, (DateTime)filterEndDate.SelectedDate);

            }    // dch rkl 10/27/2016 Validate the dates that are input
        }

        // dch rkl 11/22/2016 on focus on time, select all text
        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tbx = (TextBox)sender;
                tbx.SelectAll();
            }
            catch (Exception ex)
            { }
        }

        private void txtSTFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.filterByTicketNumber(txtSTFilter.Text);
        }
    }
}
