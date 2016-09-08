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
using TechDashboard.ViewModels;
using TechDashboard.Models;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ScheduleDetailPage.xaml
    /// </summary>
    public partial class ScheduleDetailPage : UserControl
    {
        ScheduleDetailPageViewModel _vm;
        App_ScheduledAppointment _scheduledAppointment;
        public ScheduleDetailPage(App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new ScheduleDetailPageViewModel(scheduledAppointment);
            _scheduledAppointment = scheduledAppointment;
            InitializePage();
        }

        protected void InitializePage()
        {
           StackPanel stackLayout = new StackPanel();
            //stackLayout.BackgroundColor = Color.FromHex ("#bcd5d1");
            
            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));

            Label labelModalTitle = new Label()
            {
                //FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Content = "SCHEDULE DETAILS",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3498DB")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(labelModalTitle);
            Grid.SetColumn(labelModalTitle, 0);
            Grid.SetRow(labelModalTitle, 0);
            stackLayout.Children.Add(titleLayout);

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label labelScheduledTitle = new Label()
            {
                //FontAttributes = FontAttributes.Bold,
                Foreground = asbestos,
                FontWeight = FontWeights.Bold,
                Content = "SCHEDULED"
            };
            grid.Children.Add(labelScheduledTitle);
            Grid.SetColumn(labelScheduledTitle, 1);
            Grid.SetRow(labelScheduledTitle, 0);

            Label labelActualTitle = new Label()
            {
                //FontAttributes = FontAttributes.Bold,
                Foreground = asbestos,
                FontWeight = FontWeights.Bold,
                Content = "ACTUAL"
            };
            grid.Children.Add(labelActualTitle);
            Grid.SetColumn(labelActualTitle, 2);
            Grid.SetRow(labelActualTitle, 0);

            Label labelDateTitle = new Label();
            labelDateTitle.Content = "DATE";
            //labelDateTitle.FontAttributes = FontAttributes.Bold;
            labelDateTitle.FontWeight = FontWeights.Bold;
            labelDateTitle.Foreground = asbestos;
            grid.Children.Add(labelDateTitle);
            Grid.SetColumn(labelDateTitle, 0);
            Grid.SetRow(labelDateTitle, 1);

            Label labelStartTimeTitle = new Label();
            labelStartTimeTitle.Content = "START TIME";
            //labelStartTimeTitle.FontAttributes = FontAttributes.Bold;
            labelStartTimeTitle.Foreground = asbestos;
            labelStartTimeTitle.FontWeight = FontWeights.Bold;
            grid.Children.Add(labelStartTimeTitle);
            Grid.SetColumn(labelStartTimeTitle, 0);
            Grid.SetRow(labelStartTimeTitle, 2);

            Label labelEndTimetitle = new Label();
            labelEndTimetitle.Content = "END TIME";
            //labelEndTimetitle.FontAttributes = FontAttributes.Bold;
            labelEndTimetitle.Foreground = asbestos;
            labelEndTimetitle.FontWeight = FontWeights.Bold;
            grid.Children.Add(labelEndTimetitle);
            Grid.SetColumn(labelEndTimetitle, 0);
            Grid.SetRow(labelEndTimetitle, 3);

            Label labelDurationTitle = new Label();
            labelDurationTitle.Content = "DURATION";
            labelDurationTitle.Foreground = asbestos;
            labelDurationTitle.FontWeight = FontWeights.Bold;
            grid.Children.Add(labelDurationTitle);
            Grid.SetColumn(labelDurationTitle, 0);
            Grid.SetRow(labelDurationTitle, 4);

            Label labelScheduledDate = new Label()
            {
                Content = _vm.ScheduleDetail.ScheduleDate.ToShortDateString(),
                Foreground = asbestos
            };
            grid.Children.Add(labelScheduledDate);
            Grid.SetColumn(labelScheduledDate, 1);
            Grid.SetRow(labelScheduledDate, 1);

            Label labelScheduledStartTime = new Label()
            {
                Content = _vm.ScheduleDetail.StartTime,
                Foreground = asbestos
            };
            grid.Children.Add(labelScheduledStartTime);
            Grid.SetColumn(labelScheduledStartTime, 1);
            Grid.SetRow(labelScheduledStartTime, 2);

            Label labelScheduledEndTime = new Label()
            {
                Content = _vm.ScheduleDetail.EndTime,
                Foreground = asbestos
            };
            grid.Children.Add(labelScheduledEndTime);
            Grid.SetColumn(labelScheduledEndTime, 1);
            Grid.SetRow(labelScheduledEndTime, 3);

            Label labelActualDate = new Label()
            {
                Content = _vm.TechnicianScheduleDetail.ScheduleDate.ToShortDateString(),
                Foreground = asbestos
            };
            grid.Children.Add(labelActualDate);
            Grid.SetColumn(labelActualDate, 2);
            Grid.SetRow(labelActualDate, 1);
            if (_vm.ImportDetail != null)
            {
                if (_vm.ImportDetail.StartTime != null)
                {
                    Label labelActualStartTime = new Label()
                    {
                        Content = _vm.ImportDetail.StartTime,
                        Foreground = asbestos
                    };
                    grid.Children.Add(labelActualStartTime);
                    Grid.SetColumn(labelActualStartTime, 2);
                    Grid.SetRow(labelActualStartTime, 2);
                }

                if (_vm.ImportDetail.EndTime != null)
                {
                    Label labelActualEndTime = new Label()
                    {
                        Content = _vm.ImportDetail.EndTime,
                        Foreground = asbestos
                    };
                    grid.Children.Add(labelActualEndTime);
                    Grid.SetColumn(labelActualEndTime, 2);
                    Grid.SetRow(labelActualEndTime, 3);
                }
            }
            //compute duration, if available

            stackLayout.Children.Add(grid);

            Button buttonCloseSchedule = new Button()
            {
                Content = "OK",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ecc71")),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 50
            };
            buttonCloseSchedule.Click += ButtonCloseSchedule_Clicked;
            stackLayout.Children.Add(buttonCloseSchedule);

            Content = stackLayout;
        }

        void ButtonCloseSchedule_Clicked(object sender, EventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

    }
}