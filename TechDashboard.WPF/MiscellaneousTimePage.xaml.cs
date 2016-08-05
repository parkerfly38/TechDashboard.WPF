using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using TechDashboard.Models;
using TechDashboard.ViewModels;
using Xceed.Wpf.Toolkit;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for MiscellaneousTimePage.xaml
    /// </summary>
    public partial class MiscellaneousTimePage : UserControl
    {

        DateTimePicker startTimePicker = new DateTimePicker();
        DateTimePicker endTimePicker = new DateTimePicker();
        DatePicker transactionDatePicker = new DatePicker();
        ComboBox earningCodePicker = new ComboBox();
        MiscellaneousTimePageViewModel _vm;
        Label _labelTitle;
        Dictionary<string, string> earningCodeToDesc = new Dictionary<string, string>();

        SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
        SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
        SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
        SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

        public MiscellaneousTimePage()
        {
            InitializeComponent();
            _vm = new MiscellaneousTimePageViewModel();

            //  Create a label for the technician list
            _labelTitle = new Label();
            _labelTitle.Content = "MISCELLANEOUS TIME";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = peterriver,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            Label durationTextCell = new Label
            {
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            foreach (var item in _vm.EarningsCode)
            {
                earningCodeToDesc.Add(item.EarningsCode + " - " + item.EarningsDeductionDesc, item.EarningsCode);
                earningCodePicker.Items.Add(item.EarningsCode + " - " + item.EarningsDeductionDesc);
            }

            Button buttonAccept = new Button()
            {
                Margin = new Thickness(30, 10, 30, 10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 50,
                Background = emerald,
                BorderBrush = emerald
            };
            TextBlock textButtonAccept = new TextBlock()
            {
                Text = "ACCEPT",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold
            };
            buttonAccept.Content = textButtonAccept;
            buttonAccept.Click += ButtonAccept_Clicked;
            Button buttonCancel = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 10, 30, 10),
                Background = alizarin,
                BorderBrush = alizarin
            };
            TextBlock buttnCancelText = new TextBlock()
            {
                Text = "CANCEL",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonCancel.Content = buttnCancelText;
            buttonCancel.Click += ButtonCancel_Clicked;

            startTimePicker.ShowDropDownButton = false;
            endTimePicker.ShowDropDownButton = false;
            startTimePicker.DefaultValue = DateTime.Now;
            endTimePicker.DefaultValue = DateTime.Now;
            startTimePicker.Format = DateTimeFormat.LongTime;
            endTimePicker.Format = DateTimeFormat.LongTime;
            transactionDatePicker.SelectedDate = DateTime.Now;

            if (App.Database.GetApplicatioinSettings().TwentyFourHourTime)
            {
                startTimePicker.Format = DateTimeFormat.Custom;
                startTimePicker.FormatString = "HH:mm";
                endTimePicker.Format = DateTimeFormat.Custom;
                endTimePicker.FormatString = "HH:mm";
            }

            startTimePicker.ValueChanged += (sender, e) =>
            {
                durationTextCell.Content = CalcHours();               
            };

            endTimePicker.ValueChanged += (sender, e) =>
            {
                durationTextCell.Content = CalcHours();
            };

            Label labelTitle = new Label()
            {
                Content = "Date - Start Time - End Time",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Background = peterriver
            };

            ListView timeEntries = new ListView()
            {
                ItemsSource = _vm.TimeEntries,
                ItemTemplate = (DataTemplate)this.Resources["MiscDataTemplate"]          
            };

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Label labelEmployeeNoTitle = new Label()
            {
                Content = "EMPLOYEE NO",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelEmployeeNoTitle);
            Grid.SetColumn(labelEmployeeNoTitle, 0);
            Grid.SetRow(labelEmployeeNoTitle, 0);

            Label labelTechNo = new Label()
            {
                Content = _vm.AppTechnician.TechnicianNo,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelTechNo);
            Grid.SetColumn(labelTechNo, 1);
            Grid.SetRow(labelTechNo, 0);

            Label labelTechName = new Label()
            {
                Content = _vm.AppTechnician.FirstName + " " + _vm.AppTechnician.LastName,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelTechName);
            Grid.SetColumn(labelTechName, 2);
            Grid.SetRow(labelTechName, 0);

            Label labelTransactionDateTitle = new Label()
            {
                Content = "TRANSACTION DATE",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelTransactionDateTitle);
            Grid.SetColumn(labelTransactionDateTitle, 0);
            Grid.SetRow(labelTransactionDateTitle, 1);
            topGrid.Children.Add(transactionDatePicker);
            Grid.SetColumn(transactionDatePicker, 1);
            Grid.SetRow(transactionDatePicker, 1);

            Label labelStartTimeTitle = new Label()
            {
                Content = "START TIME",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelStartTimeTitle);
            Grid.SetColumn(labelStartTimeTitle, 0);
            Grid.SetRow(labelStartTimeTitle, 2);
            topGrid.Children.Add(startTimePicker);
            Grid.SetColumn(startTimePicker, 1);
            Grid.SetRow(startTimePicker, 2);

            Label labelEndTimeTitle = new Label()
            {
                Content = "END TIME",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelEndTimeTitle);
            Grid.SetColumn(labelEndTimeTitle, 0);
            Grid.SetRow(labelEndTimeTitle, 3);
            topGrid.Children.Add(endTimePicker);
            Grid.SetColumn(endTimePicker, 1);
            Grid.SetRow(endTimePicker, 3);

            Label labelHoursWorkedTitle = new Label()
            {
                Content = "HOURS WORKED",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelHoursWorkedTitle);
            Grid.SetColumn(labelHoursWorkedTitle, 0);
            Grid.SetRow(labelHoursWorkedTitle, 4);
            topGrid.Children.Add(durationTextCell);
            Grid.SetColumn(durationTextCell, 1);
            Grid.SetRow(durationTextCell, 4);

            Label labelEarningsCodeTitle = new Label()
            {
                Content = "EARNINGS CODE",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topGrid.Children.Add(labelEarningsCodeTitle);
            Grid.SetColumn(labelEarningsCodeTitle, 0);
            Grid.SetRow(labelEarningsCodeTitle, 5);
            topGrid.Children.Add(earningCodePicker);
            Grid.SetColumn(earningCodePicker, 1);
            Grid.SetRow(earningCodePicker, 5);
            Grid.SetColumnSpan(earningCodePicker, 2);

            gridMain.Children.Add(new StackPanel()
            {
                Margin = new Thickness(30),
                Children = {
                    titleLayout,
                    topGrid,
                    buttonAccept,
                    timeEntries
                }
            });
        }

        private string CalcHours()
        {
            // Calculate and display hours worked
            if (startTimePicker.Value == null)
                startTimePicker.Value = DateTime.Now;
            if (endTimePicker.Value == null)
                endTimePicker.Value = DateTime.Now;
            string sHours = "0";
            TimeSpan durStart = new TimeSpan(((DateTime)startTimePicker.Value).TimeOfDay.Hours, ((DateTime)startTimePicker.Value).TimeOfDay.Minutes, 0);
            TimeSpan durEnd = new TimeSpan(((DateTime)endTimePicker.Value).TimeOfDay.Hours, ((DateTime)endTimePicker.Value).TimeOfDay.Minutes, 0);
            TimeSpan diff = durEnd.Subtract(durStart);
            double dHours = diff.Hours;
            double dMinutes = Math.Round((double)diff.Minutes / 60, 2, MidpointRounding.AwayFromZero);
            sHours = (dHours + dMinutes).ToString();
            return sHours;
        }

        private void ButtonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = null;
        }

        private void ButtonAccept_Clicked(object sender, RoutedEventArgs e)
        {
            JT_DailyTimeEntry newTimeEntry = new JT_DailyTimeEntry();
            JT_Technician currentTechnician = App.Database.GetCurrentTechnicianFromDb();

            if (earningCodePicker.SelectedIndex == 0)
            {
                var result = System.Windows.MessageBox.Show("Please select an earnings code before saving.", "Missing Earnings Code", MessageBoxButton.OK);
                return;
            }

            newTimeEntry.DepartmentNo = currentTechnician.TechnicianDeptNo;
            newTimeEntry.EmployeeNo = currentTechnician.TechnicianNo;
            newTimeEntry.EndTime = ((DateTime)endTimePicker.Value).TimeOfDay.ToString();
            newTimeEntry.IsModified = true;
            newTimeEntry.StartTime = ((DateTime)startTimePicker.Value).TimeOfDay.ToString();
            newTimeEntry.TransactionDate = (DateTime)transactionDatePicker.SelectedDate;
            newTimeEntry.WTNumber = currentTechnician.CurrentWTNumber;
            newTimeEntry.WTStep = currentTechnician.CurrentWTStep;
            newTimeEntry.EarningsCode = earningCodeToDesc[(string)earningCodePicker.Items[earningCodePicker.SelectedIndex]];

            _vm.DailyTimeEntry = newTimeEntry;
            _vm.SaveDailyTimeEntry(Convert.ToDouble(CalcHours()));
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new MiscellaneousTimePage();
            
        }
    }
}
