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

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ClockInAndOutPage.xaml
    /// </summary>
    public partial class ClockInPage : UserControl
    {
        ClockInPageViewModel _vm;
        DateTimePicker _pickerArriveTime;
        ComboBox _pickerTechnicianStatus;
        ComboBox _pickerTicketStatus;

        public ClockInPage()
        {
            _vm = new ClockInPageViewModel();
            InitializeComponent();
            InitializePage();
        }

        public ClockInPage(App_ScheduledAppointment scheduleDetail)
        {
            _vm = new ClockInPageViewModel(scheduleDetail);
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
                Content = "ARRIVAL TIME:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerArriveTime = new DateTimePicker {
                Format = DateTimeFormat.LongTime,
                DefaultValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                AutoCloseCalendar = false,
                ShowDropDownButton = false
            };
            if (App.Database.GetApplicatioinSettings().TwentyFourHourTime)
            {
                _pickerArriveTime.Format = DateTimeFormat.Custom;
                _pickerArriveTime.FormatString = "HH:mm";
            }

            Label labelTechStatus = new Label()
            {
                Content = "TECHNICIAN STATUS:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerTechnicianStatus = new ComboBox {
                ItemsSource = _vm.TechnicianStatusList,
                DisplayMemberPath = "StatusDescription"
            };
            
            for (int i = 0; i < _pickerTechnicianStatus.Items.Count; i++)
            {
                if (((JT_TechnicianStatus)_pickerTechnicianStatus.Items[i]).StatusDescription == _vm.DefaultArriveStatusCodeDescription)
                {
                    _pickerTechnicianStatus.SelectedIndex = i;
                    break;
                }
            }
            Label labelTicketStatus = new Label()
            {
                Content = "TICKET STATUS:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerTicketStatus = new ComboBox
            {
                ItemsSource = _vm.ServiceTicketStatusList,
                DisplayMemberPath = "Description"
            };
            
            for (int i = 0; i < _pickerTicketStatus.Items.Count; i++)
            {
                if (((JT_MiscellaneousCodes)_pickerTicketStatus.Items[i]).Description == _vm.DefaultServiceTicketArriveStatusCodeDescription)
                {
                    _pickerTicketStatus.SelectedIndex = i;
                    break;
                }
            }


            Button buttonClockIn = new Button();
            buttonClockIn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));
            buttonClockIn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));
            TextBlock clockInText = new TextBlock
            {
                Text = "CLOCK IN",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                Height = 50,
                Margin = new Thickness(0, 20, 0, 20)
            };
            buttonClockIn.Content = clockInText;
            buttonClockIn.Click += ButtonClockIn_Clicked;

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    titleLayout,
                    new StackPanel
                    {
                        Margin = new Thickness(30),
                        Children = {
                            labelTime,
                            _pickerArriveTime,
                            labelTechStatus,
                            _pickerTechnicianStatus,
                            labelTicketStatus,
                            _pickerTicketStatus,
                            buttonClockIn
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
            JT_TechnicianStatus selectedTechnicianStatus = _pickerTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            JT_MiscellaneousCodes selectedTicketStatus = _pickerTicketStatus.SelectedItem as JT_MiscellaneousCodes;

            TimeSpan timeofday = ((DateTime)_pickerArriveTime.Value).TimeOfDay;

            _vm.ClockIn(timeofday, selectedTechnicianStatus, selectedTicketStatus);
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_vm.ScheduleDetail);
            //await Navigation.PopToRootAsync();
        }
    }
}
