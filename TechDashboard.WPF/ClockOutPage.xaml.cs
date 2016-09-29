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
    /// Interaction logic for ClockOutPage.xaml
    /// </summary>
    public partial class ClockOutPage : UserControl
    {

        ClockOutPageViewModel _vm;
        DateTimePicker _pickerStartTime;
        DateTimePicker _pickerDepartTime;
        TextBox _editorHoursBilled;
        ComboBox _pickerTechnicianStatus;
        ComboBox _pickerTicketStatus;
        ComboBox _pickerEarningsCode;
        ComboBox _pickerActivityCode;
        TextBox _editorMeterReading;
        TextBox _editorWorkPerformed;
        Label _labelTitle;

        public ClockOutPage()
        {
            InitializeComponent();
        }

        public ClockOutPage(App_WorkTicket workTicket)
        {
            // Set the page title.
            //Title = "Clock Out";

            _vm = new ClockOutPageViewModel(workTicket);

            _labelTitle = new Label();
            _labelTitle.Content = "CLOCK OUT";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
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

            Label labelStartTime = new Label()
            {
                Content = "START TIME:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _pickerStartTime = new DateTimePicker();
            //add a date, as much as I hate it, to make this work
            _pickerStartTime.Value = DateTime.Now +_vm.StartTime;
            _pickerStartTime.IsEnabled = false;
            _pickerStartTime.Format = DateTimeFormat.FullDateTime;
            _pickerStartTime.ShowDropDownButton = false;
            _pickerStartTime.AutoCloseCalendar = true;

            Label labelEndTime = new Label()
            {
                Content = "DEPART TIME:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _pickerDepartTime = new DateTimePicker {
                Value = DateTime.Now + new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                ShowDropDownButton = false,
                AutoCloseCalendar = true,
                Format = DateTimeFormat.FullDateTime
            };
            //_pickerDepartTime.Unfocused += PickerDepartTime_Unfocused;

            if (App.Database.GetApplicatioinSettings().TwentyFourHourTime)
            {
                _pickerStartTime.Format = DateTimeFormat.Custom;
                _pickerStartTime.FormatString = "d/mm/yyyy HH:mm";
                _pickerDepartTime.Format = DateTimeFormat.Custom;
                _pickerDepartTime.FormatString = "d/mm/yyyy HH:mm";
            }
            _pickerDepartTime.ValueChanged += _pickerDepartTime_ValueChanged;

            Label labelHoursBilled = new Label()
            {
                Content = "HOURS BILLED:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _editorHoursBilled = new TextBox();
            _editorHoursBilled.Text = Math.Round((((DateTime)_pickerDepartTime.Value).TimeOfDay - ((DateTime)_pickerStartTime.Value).TimeOfDay).TotalHours, 2).ToString();
            _editorHoursBilled.IsEnabled = false;

            Label labelTechnicianStatus = new Label()
            {
                Content = "Technician Status:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _pickerTechnicianStatus = new ComboBox {
                ItemsSource = _vm.TechnicianStatusList,
                Margin = new Thickness(30,0, 30, 0),
            };
            _pickerTechnicianStatus.DisplayMemberPath = "StatusDescription";

            for (int i = 0; i < _pickerTechnicianStatus.Items.Count; i++)
            {
                if (((JT_TechnicianStatus)_pickerTechnicianStatus.Items[i]).StatusDescription == _vm.DefaultDepartStatusCodeDescription)
                {
                    _pickerTechnicianStatus.SelectedIndex = i;
                    break;
                }
            }

            Label labelTicketStatus = new Label()
            {
                Content = "Service Ticket Status:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _pickerTicketStatus = new ComboBox {
                ItemsSource = _vm.ServiceTicketStatusList,
                Margin = new Thickness(30, 0, 30, 0),
            };
            _pickerTicketStatus.SelectedValuePath = "MiscellaneousCode";
            _pickerTicketStatus.DisplayMemberPath = "Description";
            _pickerTicketStatus.SelectedValue = _vm.DefaultServiceTicketStatusCode;

            Label labelActivityCode = new Label()
            {
                Content = "Activity Code",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };

            _pickerActivityCode = new ComboBox {
                ItemsSource = _vm.ActivityCodeList,
                Margin = new Thickness(30, 0, 30, 0),
            };
            _pickerActivityCode.SelectedValuePath = "ActivityCode";
            _pickerActivityCode.DisplayMemberPath = "ActivityCodeAndDescription";
            _pickerActivityCode.SelectedValue = _vm.DefaultActivityCode;

            Label labelEarningsCode = new Label()
            {
                Content = "Earnings Code",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"))
            };
            _pickerEarningsCode = new ComboBox {
                ItemsSource = _vm.EarningsCodeList,
                Margin = new Thickness(30, 0, 30, 0),
            };
            _pickerEarningsCode.SelectedValuePath = "EarningsCode";
            _pickerEarningsCode.DisplayMemberPath = "EarningsDeductionDesc";
            _pickerEarningsCode.SelectedValue = _vm.DefaultEarningCode;

            Label labelMeterReading = new Label()
            {
                Content = "METER READING:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                Margin = new Thickness(30,0,30,0)
            };

            _editorMeterReading = new TextBox()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30,0, 30, 0)
            };
            //_editorMeterReading.IsEnabled = _vm.IsRepairItemAnEquipmentAsset;

            Label labelWorkPerformed = new Label()
            {
                Content = "WORK PERFORMED:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                Margin = new Thickness(30, 0, 30, 0)
            };

            _editorWorkPerformed = new TextBox();
            _editorWorkPerformed.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            _editorWorkPerformed.TextWrapping = TextWrapping.Wrap;
            _editorWorkPerformed.VerticalAlignment = VerticalAlignment.Bottom;
            _editorWorkPerformed.Margin = new Thickness(30, 0, 30, 0);
            _editorWorkPerformed.VerticalAlignment = VerticalAlignment.Stretch;
            _editorWorkPerformed.MinHeight = 80;
            _editorWorkPerformed.MaxHeight = _editorWorkPerformed.MinHeight;
            _editorWorkPerformed.AcceptsReturn = true;
            _editorWorkPerformed.AcceptsTab = true;
            _editorWorkPerformed.TextChanged += EditorWorkPerformed_TextChanged;

            // create a "clock out" button to go back
            Button buttonClockOut = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30,5, 30, 0),
                Height = 40
            };
            TextBlock clockOutText = new TextBlock()
            {
                Text = "CLOCK OUT",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold
            };
            buttonClockOut.Content = clockOutText;
            buttonClockOut.Click += ButtonClockOut_Clicked;

            // create a "cancel" button to go back
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

            Content = new StackPanel
            {
                Children =
                {
                    titleLayout,
                    new StackPanel
                    {
                        Margin = new Thickness(30,20,30,0),
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            labelStartTime,
                            _pickerStartTime
                        }
                    },
                    new StackPanel
                    {
                        Margin = new Thickness(30,0,30,0),
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            labelEndTime,
                            _pickerDepartTime
                        }
                    },
                    new StackPanel
                    {
                        Margin = new Thickness(30,0,30,0),
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            labelHoursBilled,
                            _editorHoursBilled
                        }
                    },
                    new Label()
                    {
                        Content = "TECHNICIAN STATUS:",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                        Margin = new Thickness(30, 0, 30,0)
                    },
                    _pickerTechnicianStatus,
                    new Label()
                    {
                        Content = "TICKET STATUS:",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                        Margin = new Thickness(30, 0, 30, 0)
                    },

                    _pickerTicketStatus,
                    new Label()
                    {
                        Content = "ACTIVITY CODE:",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                        Margin = new Thickness(30,0, 30, 0)
                    },
                    _pickerActivityCode,
                    new Label()
                    {
                        Content = "EARNINGS CODE:",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D")),
                        Margin = new Thickness(30, 0, 30, 0)
                    },
                    _pickerEarningsCode,
                    new StackPanel {
                        Orientation = Orientation.Vertical,
                        Children = {
                            new StackPanel {
                                Children = {
                                    labelMeterReading,
                                    _editorMeterReading
                                }
                            },
                            new StackPanel
                            {
                                Orientation = Orientation.Vertical,
                                Children = {
                                    labelWorkPerformed,
                                    _editorWorkPerformed
                                }
                            }
                        }
                    },
                    buttonClockOut,
                    buttonCancel
                }
            };
        }

        private void _pickerDepartTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetHoursBilled();
        }

        private void SetHoursBilled()
        {
            _editorHoursBilled.Text = Math.Round((((DateTime)_pickerDepartTime.Value).TimeOfDay - ((DateTime)_pickerStartTime.Value).TimeOfDay).TotalHours, 2).ToString();
        }

        private void ButtonClockOut_Clicked(object sender, RoutedEventArgs e)
        {
            if (_pickerTechnicianStatus.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a technician status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (_pickerTicketStatus.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a ticket status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (_pickerEarningsCode.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select an earnings code.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (_pickerActivityCode.SelectedIndex < 0)
            {
                //await DisplayAlert("Activity Code", "Select an activity code.", "OK");
                //return;
                MessageBoxResult result = System.Windows.MessageBox.Show("Select an activity code.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            double meterReading = 0;
            double hoursBilled = 0;

            JT_TechnicianStatus selectedTechnicianStatus = _pickerTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            JT_MiscellaneousCodes selectedTicketStatus = _pickerTicketStatus.SelectedItem as JT_MiscellaneousCodes;
            JT_EarningsCode selectedEarningsCode = _pickerEarningsCode.SelectedItem as JT_EarningsCode;
            JT_ActivityCode selectedActivityCode = _pickerActivityCode.SelectedItem as JT_ActivityCode;

            try
            {
                if (_vm.IsRepairItemAnEquipmentAsset)
                {
                    meterReading = double.Parse(_editorMeterReading.Text);
                }
                else
                {
                    meterReading = 0;
                }
            }
            catch
            {
                // empty
            }
            try
            {
                hoursBilled = double.Parse(_editorHoursBilled.Text);
            }
            catch
            {
                // empty
            }

            _vm.ClockOut(((DateTime)_pickerDepartTime.Value).TimeOfDay, selectedTechnicianStatus, selectedTicketStatus, selectedActivityCode, "",
                         selectedEarningsCode, hoursBilled, meterReading, _editorWorkPerformed.Text);

            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new SchedulePage();
        }

        private void EditorWorkPerformed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((_editorWorkPerformed.Text != null) && (_editorWorkPerformed.Text.Length > 0))
            {
                _editorHoursBilled.IsEnabled = true;
            }
            else
            {
                _editorHoursBilled.Text = Math.Round((((DateTime)_pickerDepartTime.Value).TimeOfDay - ((DateTime)_pickerStartTime.Value).TimeOfDay).TotalHours, 2).ToString();
                _editorHoursBilled.IsEnabled = false;
            }
        }

        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;

            contentArea.Content = new SchedulePage();
        }
    }
}
