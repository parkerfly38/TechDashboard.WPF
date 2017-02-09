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

/**************************************************************************************************
 * Page Name    ScheduleDetailPage
 * Description: Schedule Detail Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 10/31/2016   DCH     The Scheduled Start and End Time and Actual Start and End Time were not all
 *                      being displayed.  Make sure duration is calculated for both scheduled and actual.
 * 11/22/2016   DCH     Make sure start and end time are not null, to prevent error
 * 01/12/2017   DCH     The actual end time should default to the current date/time if it is blank.
 * 02/03/2017   DCH     Actual Duration is not being calculated correctly.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ScheduleDetailPage.xaml
    /// </summary>
    public partial class ScheduleDetailPage : UserControl
    {
        ScheduleDetailPageViewModel _vm;
        App_ScheduledAppointment _scheduledAppointment;

        // dch rkl 10/31/2016 scheduled and actual start times
        string schedStartTime = "";
        string schedEndTime = "";
        string actStartTime = "";
        string actEndTime = "";

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
                FontSize = 18,
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
                Content = "Scheduled"
            };
            grid.Children.Add(labelScheduledTitle);
            Grid.SetColumn(labelScheduledTitle, 1);
            Grid.SetRow(labelScheduledTitle, 0);

            Label labelActualTitle = new Label()
            {
                //FontAttributes = FontAttributes.Bold,
                Foreground = asbestos,
                FontWeight = FontWeights.Bold,
                Content = "Actual"
            };
            grid.Children.Add(labelActualTitle);
            Grid.SetColumn(labelActualTitle, 2);
            Grid.SetRow(labelActualTitle, 0);

            Label labelDateTitle = new Label();
            labelDateTitle.Content = "Date";
            //labelDateTitle.FontAttributes = FontAttributes.Bold;
            labelDateTitle.FontWeight = FontWeights.Bold;
            labelDateTitle.Foreground = asbestos;
            grid.Children.Add(labelDateTitle);
            Grid.SetColumn(labelDateTitle, 0);
            Grid.SetRow(labelDateTitle, 1);

            Label labelStartTimeTitle = new Label();
            labelStartTimeTitle.Content = "Start Time";
            //labelStartTimeTitle.FontAttributes = FontAttributes.Bold;
            labelStartTimeTitle.Foreground = asbestos;
            labelStartTimeTitle.FontWeight = FontWeights.Bold;
            grid.Children.Add(labelStartTimeTitle);
            Grid.SetColumn(labelStartTimeTitle, 0);
            Grid.SetRow(labelStartTimeTitle, 2);

            Label labelEndTimetitle = new Label();
            labelEndTimetitle.Content = "End Time";
            //labelEndTimetitle.FontAttributes = FontAttributes.Bold;
            labelEndTimetitle.Foreground = asbestos;
            labelEndTimetitle.FontWeight = FontWeights.Bold;
            grid.Children.Add(labelEndTimetitle);
            Grid.SetColumn(labelEndTimetitle, 0);
            Grid.SetRow(labelEndTimetitle, 3);

            Label labelDurationTitle = new Label();
            labelDurationTitle.Content = "Duration";
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
                // dch rkl 10/12/2016 show formatted time
                //Content = _vm.ScheduleDetail.StartTime,
                Content = _vm.ScheduleDetail.StartTimeFormatted,
                Foreground = asbestos
            };
            grid.Children.Add(labelScheduledStartTime);
            Grid.SetColumn(labelScheduledStartTime, 1);
            Grid.SetRow(labelScheduledStartTime, 2);

            schedStartTime = _vm.ScheduleDetail.StartTime;      // dch rkl 10/31/2016

            Label labelScheduledEndTime = new Label()
            {
                // dch rkl 10/12/2016 show formatted time
                //Content = _vm.ScheduleDetail.EndTime,
                Content = _vm.ScheduleDetail.EndTimeFormatted,
                Foreground = asbestos
            };
            grid.Children.Add(labelScheduledEndTime);
            Grid.SetColumn(labelScheduledEndTime, 1);
            Grid.SetRow(labelScheduledEndTime, 3);

            schedEndTime = _vm.ScheduleDetail.EndTime;      // dch rkl 10/31/2016

            Label labelActualDate = new Label()
            {
                Foreground = asbestos
            };

            // dch rkl 02/03/2017 define this here
            Label labelActualEndTime = new Label()
            {
                Foreground = asbestos,
                Content = ""
            };
            Label labelActualStartTime = new Label()
            {
                Content = "",
                Foreground = asbestos
            };

            if (_vm.TimeEntryDetail != null)
                labelActualDate.Content = _vm.TimeEntryDetail.TransactionDate.ToShortDateString();
            grid.Children.Add(labelActualDate);
            Grid.SetColumn(labelActualDate, 2);
            Grid.SetRow(labelActualDate, 1);
            bool bStartTimeSet = false;     // dch rkl 10/14/2016 Get Start Time from the Technician Record
            if (_vm.ImportDetail != null)
            {
                if (_vm.ImportDetail.StartTime != null)
                {
                    labelActualStartTime.Content = FormattedTime(_vm.TimeEntryDetail.StartTime);
                    //Label labelActualStartTime = new Label()
                    //{
                    //    Content = FormattedTime(_vm.TimeEntryDetail.StartTime),
                    //    Foreground = asbestos
                    //};
                    actStartTime = _vm.TimeEntryDetail.StartTime;      // dch rkl 10/31/2016
                    grid.Children.Add(labelActualStartTime);
                    Grid.SetColumn(labelActualStartTime, 2);
                    Grid.SetRow(labelActualStartTime, 2);
                    bStartTimeSet = true;    // dch rkl 10/14/2016 Get Start Time from the Technician Record
                }

                if (_vm.ImportDetail.EndTime != null)
                {
                    labelActualEndTime.Content = FormattedTime(_vm.TimeEntryDetail.EndTime);
                    //Label labelActualEndTime = new Label()
                    //{
                    //    Content = FormattedTime(_vm.TimeEntryDetail.EndTime),
                    //    Foreground = asbestos
                    //};
                    grid.Children.Add(labelActualEndTime);
                    Grid.SetColumn(labelActualEndTime, 2);
                    Grid.SetRow(labelActualEndTime, 3);

                    actEndTime = _vm.TimeEntryDetail.EndTime;      // dch rkl 10/31/2016
                }
            }
            // dch rkl 01/12/2017 If Actual End Time is blank, use current date BEGIN
            else
            {
                labelActualEndTime.Content = FormattedTime(DateTime.Now.ToString("hh:mm tt"));
                //Label labelActualEndTime = new Label()
                //{
                //    Content = FormattedTime(DateTime.Now.ToString("hh:mm tt")),
                //    Foreground = asbestos
                //};
                grid.Children.Add(labelActualEndTime);
                Grid.SetColumn(labelActualEndTime, 2);
                Grid.SetRow(labelActualEndTime, 3);

                actEndTime = DateTime.Now.ToString("hhmm");
            }
            // dch rkl 01/12/2017 If Actual End Time is blank, use current date END

            // dch rkl 10/14/2016 Get Start Time from the Technician Record BEGIN
            if (bStartTimeSet == false)
            {
                JT_Technician tech = App.Database.GetCurrentTechnicianFromDb();
                if (tech.CurrentStartTime != null)
                {
                    labelActualStartTime.Content = FormattedTime(tech.CurrentStartTime);
                    //Label labelActualStartTime = new Label()
                    //{
                    //    Content = FormattedTime(tech.CurrentStartTime),
                    //    Foreground = asbestos
                    //};
                    grid.Children.Add(labelActualStartTime);
                    Grid.SetColumn(labelActualStartTime, 2);
                    Grid.SetRow(labelActualStartTime, 2);

                    actStartTime = tech.CurrentStartTime;      // dch rkl 10/31/2016
                }
            }
            // dch rkl 10/14/2016 Get Start Time from the Technician Record END

            //compute duration, if available
            // dch rkl 10/31/2016 Calculate Duration for Scheduled and Actual
            // Scheduled
            DateTime dtSST;
            DateTime dtSET;
            if (schedStartTime.Length > 0)
            {
                schedStartTime = schedStartTime.Substring(0, 2) + ":" + schedStartTime.Substring(2, 2);
            }
            if (schedEndTime.Length > 0)
            {
                schedEndTime = schedEndTime.Substring(0, 2) + ":" + schedEndTime.Substring(2, 2);
            }
            if (DateTime.TryParse(schedStartTime, out dtSST) && DateTime.TryParse(schedEndTime, out dtSET))
            {
                TimeSpan tsSchDur = dtSET.Subtract(dtSST);
                Label labelSchedDur = new Label()
                {
                    Content = Math.Round(tsSchDur.TotalHours, 2, MidpointRounding.AwayFromZero).ToString(),
                    Foreground = asbestos
                };
                grid.Children.Add(labelSchedDur);
                Grid.SetColumn(labelSchedDur, 1);
                Grid.SetRow(labelSchedDur, 4);
            }

            // Actual
            DateTime dtAST;
            DateTime dtAET;
            if (actStartTime != null && actStartTime.Length > 0)
            {
                actStartTime = actStartTime.Substring(0, 2) + ":" + actStartTime.Substring(2, 2);
            }
            // dch rkl 11/22/2016 make sure actEndTime is not null
            if (actEndTime != null && actEndTime.Length > 0)
            {
                actEndTime = actEndTime.Substring(0, 2) + ":" + actEndTime.Substring(2, 2);
            }
            else
            {
                if (App.Database.GetApplicationSettings().TwentyFourHourTime)
                {
                    actEndTime = DateTime.Now.ToString("HH:mm");
                }
                else
                {
                    actEndTime = DateTime.Now.ToString("hh:mm tt");
                }
            }
            // dch rkl 02/03/2017 Actual duration is not being calculated correctly      
            //if (DateTime.TryParse(actStartTime, out dtAST) && DateTime.TryParse(actEndTime, out dtAET))
            if (DateTime.TryParse(labelActualStartTime.Content.ToString(), out dtAST) && DateTime.TryParse(labelActualEndTime.Content.ToString(), out dtAET))
            {
                TimeSpan tsActDur = dtAET.Subtract(dtAST);
                Label labelActDur = new Label()
                {
                    Content = Math.Round(tsActDur.TotalHours,2,MidpointRounding.AwayFromZero).ToString(),
                    Foreground = asbestos
                };
                grid.Children.Add(labelActDur);
                Grid.SetColumn(labelActDur, 2);
                Grid.SetRow(labelActDur, 4);
            }

            stackLayout.Children.Add(grid);

            Button buttonCloseSchedule = new Button()
            {
                Content = "OK",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ecc71")),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 40         // dch rkl 11/22/2016 change height
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


        // dch rkl 10/14/2016 format the time - Note: This logic should be added to the JT_Technician model
        private string FormattedTime(string sTimeIn)
        {
            if (sTimeIn == null) { sTimeIn = ""; }

            string sTimeOut = sTimeIn;

            string sHour = "";
            string sMin = "";
            string sAMorPM = "";
            int iHour = 0;

            if (sTimeIn.Length == 4)
            {
                sHour = sTimeIn.Substring(0, 2);
                sMin = sTimeIn.Substring(2, 2);
            }
            else if (sTimeIn.Length == 3)
            {
                sHour = "0" + sTimeIn.Substring(0, 1);
                sMin = sTimeIn.Substring(1, 2);
            }

            int.TryParse(sHour, out iHour);
            if (iHour > 0)
            {
                if (iHour < 12) { sAMorPM = "AM"; }
                else { sAMorPM = "PM"; }

                App_Settings appSettings = App.Database.GetApplicationSettings();
                if (appSettings.TwentyFourHourTime == false && iHour > 12)
                {
                    iHour = iHour - 12;
                    sHour = iHour.ToString();
                    if (sHour.Length == 1) { sHour = "0" + sHour; }
                }

                sTimeOut = string.Format("{0}:{1} {2}", sHour, sMin, sAMorPM);
            }

            return sTimeOut;
        }

    }
}