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
using Xceed.Wpf.Toolkit;

/**************************************************************************************************
 * Page Name    ClockInPage
 * Description: Clock In Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 10/26/2016   DCH     Add a cancel button to return to the ticket details
 * 10/31/2016   DCH     Set Clock In Status to Service Ticket Status
 * 11/01/2016   DCH     Change date pickers to textboxes, to allow free-form input
 * 11/22/2016   DCH     Handle various versions of time, and convert to correct format
 * 01/12/2017   DCH     Time inputs still not all handled.  For example, if 830P entered, or 830 
 *                      entered, it was kicked out as invalid. 
 * 01/12/2017   DCH     After clocking in, make sure the clock out button is enabled on the ticket
 *                      details screen.
 * 01/16/2017   DCH     If there is no value for the Default Arrive Status Code, use the 
 *                      technician's current status
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ClockInAndOutPage.xaml
    /// </summary>
    public partial class ClockInPage : UserControl
    {
        ClockInPageViewModel _vm;

        // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
        //DateTimePicker _pickerArriveTime;
        TextBox _textArriveTime;
        // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

        ComboBox _pickerTechnicianStatus;
        ComboBox _pickerTicketStatus;

        // dch rkl 10/26/2016
        App_ScheduledAppointment _scheduleDetail;

        public ClockInPage()
        {
            _vm = new ClockInPageViewModel();
            InitializeComponent();
            InitializePage();
        }

        public ClockInPage(App_ScheduledAppointment scheduleDetail)
        {
            _vm = new ClockInPageViewModel(scheduleDetail);

            // dch rkl 10/26/2016
            _scheduleDetail = scheduleDetail;

            InitializeComponent();
            InitializePage();
        }
        protected void InitializePage()
        {
            //  Create a label for the technician list
            Label _labelTitle = new Label();
            _labelTitle.Content = "CLOCK IN";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;
            _labelTitle.FontSize = 18;      // dch rkl 10/26/2016

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            //Xamarin.Forms.Label mainLabel = new Xamarin.Forms.Label { Text = "CLOCK IN", FontFamily = Device.OnPlatform("OpenSans-Bold",null,null), TextColor = asbestos };
            Label labelTime = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,     // dch rkl 10/26/2016
                Content = "Arrival Time",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            _textArriveTime = new TextBox();
            if (App.Database.GetApplicationSettings().TwentyFourHourTime)
            {
                _textArriveTime.Text = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                _textArriveTime.Text = DateTime.Now.ToString("hh:mm tt");
            }
            _textArriveTime.Width = 60;
            _textArriveTime.HorizontalAlignment = HorizontalAlignment.Left;
            _textArriveTime.LostFocus += textArriveTime_LostFocus;
            _textArriveTime.GotFocus += textArriveTime_GotFocus;       // dch rkl 11/22/2016 on focus on time, select all text

            Label labelTechStatus = new Label()
            {
                Content = "Technician Status",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerTechnicianStatus = new ComboBox {
                ItemsSource = _vm.TechnicianStatusList,
                DisplayMemberPath = "StatusDescription",
                Width = 250     // dch rkl 10/26/2016
            };

            // dch rkl 01/16/2017 If there is no value for the Default Arrive Status Code, use the technician's current status BEGIN
            string arriveCode = _vm.DefaultArriveStatusCode;
            if (arriveCode.Trim().Length == 0)
            {
                JT_Technician tech = App.Database.GetCurrentTechnicianFromDb();
                arriveCode = tech.CurrentStatus;
            }
            // dch rkl 01/16/2017 If there is no value for the Default Arrive Status Code, use the technician's current status END

            for (int i = 0; i < _pickerTechnicianStatus.Items.Count; i++)
            {
                // dch rkl 01/16/2017 If there is no value for the Default Arrive Status Code, use the technician's current status BEGIN
                //if (((JT_TechnicianStatus)_pickerTechnicianStatus.Items[i]).StatusDescription == _vm.DefaultArriveStatusCodeDescription)
                if (((JT_TechnicianStatus)_pickerTechnicianStatus.Items[i]).StatusCode == arriveCode)
                // dch rkl 01/16/2017 If there is no value for the Default Arrive Status Code, use the technician's current status END
                {
                    _pickerTechnicianStatus.SelectedIndex = i;
                    break;
                }
            }
            Label labelTicketStatus = new Label()
            {
                Content = "Ticket Status",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerTicketStatus = new ComboBox
            {
                ItemsSource = _vm.ServiceTicketStatusList,
                DisplayMemberPath = "Description",
                Width = 250     // dch rkl 10/26/2016
            };

            // dch rkl 10/31/2016 If _vm.DefaultServiceTicketArriveStatusCodeDescription is null, set clock in
            // status to service ticket status.  Per Jeanne Jackson
            if (_vm.DefaultServiceTicketArriveStatusCodeDescription == null)
            {
                App_WorkTicket workTicket = App.Database.GetWorkTicket(_scheduleDetail);
                if (workTicket != null)
                {
                    for (int i = 0; i < _pickerTicketStatus.Items.Count; i++)
                    {
                        if (((JT_MiscellaneousCodes)_pickerTicketStatus.Items[i]).MiscellaneousCode == workTicket.StatusCode)
                        {
                            _pickerTicketStatus.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _pickerTicketStatus.Items.Count; i++)
                {
                    if (((JT_MiscellaneousCodes)_pickerTicketStatus.Items[i]).Description == _vm.DefaultServiceTicketArriveStatusCodeDescription)
                    {
                        _pickerTicketStatus.SelectedIndex = i;
                        break;
                    }
                }
            }

            // dch rkl 10/26/2016 make the button look like the rest of the buttons in the app BEGIN
            Button buttonClockIn = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 5, 30, 0),
                Height = 40
            };
            TextBlock clockInText = new TextBlock()
            {
                Text = "CLOCK IN",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold
            };
            // dch rkl 10/26/2016 make the button look like the rest of the buttons in the app END
            buttonClockIn.Content = clockInText;
            buttonClockIn.Click += ButtonClockIn_Clicked;

            // dch rkl 10/26/2016 create a "cancel" button to go back BEGIN
            Button buttonCancel = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 5, 30, 5),
                Height = 40
            };
            TextBlock cancelText = new TextBlock()
            {
                Text = "CANCEL",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonCancel.Content = cancelText;
            buttonCancel.Click += buttonCancel_Clicked;
            // dch rkl 10/26/2016 create a "cancel" button to go back END

            // dch rkl 10/26/2016 format the grid for arrival time, technician status and ticket status BEGIN
            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            topGrid.Children.Add(labelTime);
            Grid.SetColumn(labelTime, 0);
            Grid.SetRow(labelTime, 0);
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            //topGrid.Children.Add(_pickerArriveTime);
            //Grid.SetColumn(_pickerArriveTime, 1);
            //Grid.SetRow(_pickerArriveTime, 0);
            topGrid.Children.Add(_textArriveTime);
            Grid.SetColumn(_textArriveTime, 1);
            Grid.SetRow(_textArriveTime, 0);
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

            topGrid.Children.Add(labelTechStatus);
            Grid.SetColumn(labelTechStatus, 0);
            Grid.SetRow(labelTechStatus, 1);
            topGrid.Children.Add(_pickerTechnicianStatus);
            Grid.SetColumn(_pickerTechnicianStatus, 1);
            Grid.SetRow(_pickerTechnicianStatus, 1);

            topGrid.Children.Add(labelTicketStatus);
            Grid.SetColumn(labelTicketStatus, 0);
            Grid.SetRow(labelTicketStatus, 2);
            topGrid.Children.Add(_pickerTicketStatus);
            Grid.SetColumn(_pickerTicketStatus, 1);
            Grid.SetRow(_pickerTicketStatus, 2);
            // dch rkl 10/26/2016 format the grid for arrival time, technician status and ticket status END

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    titleLayout,
                    new StackPanel
                    {
                        Margin = new Thickness(30),
                        Children = {
                            //labelTime,
                            //_pickerArriveTime,
                            //labelTechStatus,
                            //_pickerTechnicianStatus,
                            //labelTicketStatus,
                            //_pickerTicketStatus,
                            topGrid,        // dch rkl 10/26/2016
                            buttonClockIn
                            , buttonCancel              // dch rkl 10/26/2016 add cancel button
                        }
                    }

                }
            });
        }

        protected async void ButtonClockIn_Clicked(object sender, EventArgs e)
        {
            
            if (_pickerTechnicianStatus.SelectedIndex < 0)
            {
                //await new MessageBox ("Status", "Select a technician status.", "OK");
                //return;
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a technician status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (_pickerTicketStatus.SelectedIndex < 0)
            {
                // await DisplayAlert("Status", "Select a ticket status.", "OK");
                // return;
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a ticket status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            // Validate Arrive Time
            DateTime dtArriveTime;
            if (DateTime.TryParse(_textArriveTime.Text, out dtArriveTime) == false)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Enter a valid Arrive Time.", "Arrive Time", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN

            JT_TechnicianStatus selectedTechnicianStatus = _pickerTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            JT_MiscellaneousCodes selectedTicketStatus = _pickerTicketStatus.SelectedItem as JT_MiscellaneousCodes;

            // dch rkl 11/1/2016 use textbox instead of datetime picker for times 
            //TimeSpan timeofday = ((DateTime)_pickerArriveTime.Value).TimeOfDay;
            TimeSpan timeofday = dtArriveTime.TimeOfDay;

            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

            _vm.ClockIn(timeofday, selectedTechnicianStatus, selectedTicketStatus);
            ContentControl contentArea = (ContentControl)this.Parent;
            _vm.ScheduleDetail.IsCurrent = true;            // dch rkl 01/12/2017 This is what makes the Clock Out Button Appear
            contentArea.Content = new TicketDetailsPage(_vm.ScheduleDetail);
            //await Navigation.PopToRootAsync();
        }

        // dch rkl 10/26/2016 add cancel button - return to ticket details
        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduleDetail);
        }

        // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
        private void textArriveTime_LostFocus(object sender, RoutedEventArgs e)
        {
            // Reformat and Validate the date
            bool bValidTime = false;
            if ((_textArriveTime.Text != null) && (_textArriveTime.Text.Length > 0))
            {
                _textArriveTime.Text = FormatTime(_textArriveTime.Text, ref bValidTime);
            }

            if (bValidTime == false)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Invalid Clock In Time", "Clock In Time", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
        }

        // dch rkl 11/22/2016 on focus on time, select all text
        private void textArriveTime_GotFocus(object sender, RoutedEventArgs e)
        {
            _textArriveTime.SelectAll();
        }

        private string FormatTime(string sTimeIn, ref bool bValidTime)
        {
            // Validate and Format the Time
            string sTimeOut = sTimeIn;
            bValidTime = false;

            try
            {
                App_Settings appSettings = App.Database.GetApplicationSettings();

                // dch rkl 11/22/2016 handle all versions of time entered
                sTimeIn = sTimeIn.ToUpper();

                // dch rkl 01/12/2017 handle additional time formats BEGIN
                string sAMPM = "";
                int iHorM;
                if (sTimeIn.IndexOf("AM") > -1)
                {
                    sAMPM = "AM";
                    sTimeIn = sTimeIn.Replace("AM", "");
                }
                else if (sTimeIn.IndexOf("PM") > -1)
                {
                    sAMPM = "PM";
                    sTimeIn = sTimeIn.Replace("PM", "");
                }
                else if (sTimeIn.IndexOf("A") > -1)
                {
                    sAMPM = "AM";
                    sTimeIn = sTimeIn.Replace("A", "");
                }
                else if (sTimeIn.IndexOf("P") > -1)
                {
                    sAMPM = "PM";
                    sTimeIn = sTimeIn.Replace("P", "");
                }
                if (sTimeIn.Length == 1)
                {
                    int.TryParse(sTimeIn, out iHorM);
                    if (appSettings.TwentyFourHourTime && sAMPM == "PM")
                    {
                        iHorM += 12;
                        sTimeIn = string.Format("{0}:00", iHorM);
                    }
                    else
                    {
                        sTimeIn = string.Format("0{0}:00", sTimeIn);
                    }
                }
                else if (sTimeIn.Length == 2)
                {
                    int.TryParse(sTimeIn, out iHorM);
                    if (iHorM > 12)
                    {
                        if (appSettings.TwentyFourHourTime)
                        {
                            sTimeIn = string.Format("{0}:00", sTimeIn);
                            sAMPM = "";
                        }
                        else
                        {
                            sTimeIn = string.Format("{0}:00", iHorM - 12);
                            sAMPM = "PM";
                        }
                    }
                    else { sTimeIn = string.Format("{0}:00", sTimeIn); }
                }
                else if (sTimeIn.Length == 3) { sTimeIn = string.Format("0{0}:{1}", sTimeIn.Substring(0, 1), sTimeIn.Substring(1, 2)); }
                else if (sTimeIn.Length == 4) { sTimeIn = string.Format("{0}:{1}", sTimeIn.Substring(0, 2), sTimeIn.Substring(2, 2)); }
                if (sAMPM.Length > 0) { sTimeIn = sTimeIn + " " + sAMPM; }
                //if (sTimeIn.IndexOf("A") > -1 && sTimeIn.IndexOf("AM") == -1) { sTimeIn = sTimeIn.Replace("A", "AM"); }
                //if (sTimeIn.IndexOf("P") > -1 && sTimeIn.IndexOf("PM") == -1) { sTimeIn = sTimeIn.Replace("P", "PM"); }
                //if (sTimeIn.Length == 1) { sTimeIn = string.Format("0{0}:00", sTimeIn); }
                //else if (sTimeIn.Length == 2) { sTimeIn = string.Format("{0}:00", sTimeIn); }
                // dch rkl 01/12/2017 handle additional time formats END

                string myDateString = DateTime.Now.Date.ToString("MM/dd/yyyy") + " " + sTimeIn;
                DateTime dtDate = DateTime.Parse(myDateString);

                // dch rkl 01/12/2017 handle additional time formats BEGIN
                if (appSettings.TwentyFourHourTime) { sTimeOut = dtDate.ToString("HH:mm tt"); }
                else { sTimeOut = dtDate.ToString("hh:mm tt"); }
                //sTimeOut = dtDate.ToString("hh:mm tt");
                // dch rkl 01/12/2017 handle additional time formats END

                bValidTime = true;
            }
            catch (Exception ex)
            {
                bValidTime = false;
            }

            return sTimeOut;
        }
        // dch rkl 11/1/2016 use textbox instead of datetime picker for times END
    }
}
