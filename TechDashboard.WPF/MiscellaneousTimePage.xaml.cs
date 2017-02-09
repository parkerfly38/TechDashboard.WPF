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
using TechDashboard.Tools;
using TechDashboard.ViewModels;
using Xceed.Wpf.Toolkit;

/**************************************************************************************************
 * Page Name    MiscellaneousTimePage
 * Description: Miscellaneous Time Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 10/31/2016   DCH     Display the Technician Department Number next to the Technician Number
 * 11/01/2016   DCH     Change date pickers to textboxes, to allow free-form input
 * 11/01/2016   DCH     Hide the grid of existing time entries
 * 11/03/2016   DCH     Use Time Tracker Options to determine if the user should enter start/end
 *                      Time or Total Hours;  Make duration (hours) editable, depending on Time 
 *                      Tracker Settings.
 * 11/22/2016   DCH     Select full text of textboxes on focus
 *                      Validate times
 *                      Allow various formats of time to be entered, and convert to HH:MM PM Format
 * 01/12/2017   DCH     Time inputs still not all handled.  For example, if 830P entered, or 830 
 *                      entered, it was kicked out as invalid.
 * 01/12/2017   DCH     Do not set start/end time to default values of current date/time.
 * 01/12/2017   DCH     Error message for invalid start time text was not correct.
 * 01/20/2017   DCH     Move layout to .xaml page insetad of code.
 * 01/20/2017   DCH     App was crashing when invalid date was entered.
 * 01/20/2017   DCH     Add Cancel button.
 * 01/23/2017   DCH     Do not include WT Number and WT Step for miscellaneous time.
 * 02/03/2017   DCH     Validate date before saving, to prevent blank/null date.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for MiscellaneousTimePage.xaml
    /// </summary>
    public partial class MiscellaneousTimePage : UserControl
    {
        // dch rkl 11/03/2016 Get Time Tracker Options to Determine what can be entered
        string _captureTimeInTimeTracker = "N";

        MiscellaneousTimePageViewModel _vm;

        SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
        SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
        SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
        SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

        public MiscellaneousTimePage()
        {
            InitializeComponent();
            _vm = new MiscellaneousTimePageViewModel();

            earningCodePicker.ItemsSource = _vm.EarningsCode;

            if (App.Database.GetApplicationSettings().TwentyFourHourTime)
            {
                startTimeText.Text = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                startTimeText.Text = DateTime.Now.ToString("hh:mm tt");
            }

            textTransactionDate.Text = DateTime.Now.ToShortDateString();

            labelTechNo.Content = String.Format("{0}-{1}", _vm.AppTechnician.TechnicianDeptNo, _vm.AppTechnician.TechnicianNo);

            labelTechName.Content = _vm.AppTechnician.FirstName + " " + _vm.AppTechnician.LastName;

            // dch rkl 11/03/2016 Get Time Tracker Options to Determine what can be entered
            _captureTimeInTimeTracker = "N";
            JT_TimeTrackerOptions timeTrackerOptions = App.Database.GetTimeTrackerOptions();
            if (timeTrackerOptions != null && timeTrackerOptions.CaptureTimeInTimeTracker != null)
            {
                _captureTimeInTimeTracker = timeTrackerOptions.CaptureTimeInTimeTracker;
                if (_captureTimeInTimeTracker == "O") { _captureTimeInTimeTracker = "Y"; }  // treat "O" as "Y", per Chris M.
            }
            // if CaptureTimeInTimeTracker = "Y", they enter start/end time
            // if CaptureTimeInTimeTracker = "N", they enter hours
            if (_captureTimeInTimeTracker == "Y")
            {
                startTimeText.IsEnabled = true;
                startTimeText.Text = "";            // dch rkl 01/12/2017 set to blank by default
                endTimeText.IsEnabled = true;   
                durationTextCell.IsEnabled = false;
            }
            else
            {
                startTimeText.IsEnabled = false;
                startTimeText.Text = "";
                endTimeText.IsEnabled = false;
                endTimeText.Text = "";
                durationTextCell.IsEnabled = true;
            }
        }

        // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
        private void startTimeText_LostFocus(object sender, RoutedEventArgs e)
        {
            // Reformat the date
            bool bValidTime = false;
            if ((startTimeText.Text != null) && (startTimeText.Text.Length > 0))
            {
                startTimeText.Text = FormatTime(startTimeText.Text, ref bValidTime);
                // dch rkl 11/03/2016 Make the duration a textbox, because it can be edited in some cases
                //durationTextCell.Content = CalcHours();
                durationTextCell.Text = CalcHours();
            }

            if (bValidTime == false)
            {
                // dch rkl 01/12/2017 Invalid Message Text 
                var result = System.Windows.MessageBox.Show("Invalid Start Time.", "Invalid Start Time", MessageBoxButton.OK);
                //var result = System.Windows.MessageBox.Show("Invalid End Time.", "Invalid End Time", MessageBoxButton.OK);

                return;
            }
        }

        private void endTimeText_LostFocus(object sender, RoutedEventArgs e)
        {
            // Reformat the date
            bool bValidTime = false;
            if ((endTimeText.Text != null) && (endTimeText.Text.Length > 0))
            {
                endTimeText.Text = FormatTime(endTimeText.Text, ref bValidTime);
                durationTextCell.Text = CalcHours();
            }

            if (bValidTime == false)
            {
                var result = System.Windows.MessageBox.Show("Invalid End Time.", "Invalid End Time", MessageBoxButton.OK);
                return;
            }
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
                    sTimeIn= sTimeIn.Replace("AM", "");
                }
                else if (sTimeIn.IndexOf("PM") > -1)
                {
                    sAMPM = "PM";
                    sTimeIn=sTimeIn.Replace("PM", "");
                }
                else if (sTimeIn.IndexOf("A") > -1)
                {
                    sAMPM = "AM";
                    sTimeIn=sTimeIn.Replace("A", "");
                }
                else if (sTimeIn.IndexOf("P") > -1)
                {
                    sAMPM = "PM";
                    sTimeIn=sTimeIn.Replace("P", "");
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

        private string CalcHours()
        {
            // Calculate and display hours worked

            // dch rkl 01/12/2017 Do not set start/end time to default values of current date/time
            if (startTimeText.Text == null || endTimeText.Text == null)
            {
                return "0";
            }
            
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            if (startTimeText.Text == null)
            {
                if (App.Database.GetApplicationSettings().TwentyFourHourTime)
                {
                    startTimeText.Text = DateTime.Now.ToString("HH:mm");
                }
                else
                {
                    startTimeText.Text = DateTime.Now.ToString("hh:mm tt");
                }
            }
            if (endTimeText.Text == null)
            {
                if (App.Database.GetApplicationSettings().TwentyFourHourTime)
                {
                    endTimeText.Text = DateTime.Now.ToString("HH:mm");
                }
                else
                {
                    endTimeText.Text = DateTime.Now.ToString("hh:mm tt");
                }
            }
            string sHours = "0";
            DateTime dtStartTime;
            DateTime dtEndTime;
            if (DateTime.TryParse(startTimeText.Text, out dtStartTime) == false) { dtStartTime = DateTime.Now; }
            if (DateTime.TryParse(endTimeText.Text, out dtEndTime) == false) { dtEndTime = DateTime.Now; }
            TimeSpan durStart = new TimeSpan(dtStartTime.TimeOfDay.Hours, dtStartTime.TimeOfDay.Minutes, 0);
            TimeSpan durEnd = new TimeSpan(dtEndTime.TimeOfDay.Hours, dtEndTime.TimeOfDay.Minutes, 0);

            //if (startTimePicker.Value == null)
            //    startTimePicker.Value = DateTime.Now;
            //if (endTimePicker.Value == null)
            //    endTimePicker.Value = DateTime.Now;
            //string sHours = "0";
            //TimeSpan durStart = new TimeSpan(((DateTime)startTimePicker.Value).TimeOfDay.Hours, ((DateTime)startTimePicker.Value).TimeOfDay.Minutes, 0);
            //TimeSpan durEnd = new TimeSpan(((DateTime)endTimePicker.Value).TimeOfDay.Hours, ((DateTime)endTimePicker.Value).TimeOfDay.Minutes, 0);
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

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

        // dch rkl 11/22/2016 select all text on focus
        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            tbx.SelectAll();
        }

        // dch rkl 01/20/2017 Use Textbox For Date
        private void textTransactionDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime dtDate;
            if (DateTime.TryParse(textTransactionDate.Text, out dtDate))
            {
                textTransactionDate.Text = dtDate.ToShortDateString();
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Invalid transaction date entered.", "Invalid Date", MessageBoxButton.OK);
            }
        }

        private void ButtonAccept_Clicked(object sender, RoutedEventArgs e)
        {
            JT_DailyTimeEntry newTimeEntry = new JT_DailyTimeEntry();
            JT_Technician currentTechnician = App.Database.GetCurrentTechnicianFromDb();

            if (earningCodePicker.SelectedIndex < 0)
            {
                var result = System.Windows.MessageBox.Show("Please select an earnings code before saving.", "Missing Earnings Code", MessageBoxButton.OK);
                return;
            }

            // dch rkl 02/03/2017 Validate Date
            DateTime dtTranDate;
            if (textTransactionDate.Text.Trim().Length == 0 || DateTime.TryParse(textTransactionDate.Text, out dtTranDate) == false)
            {
                var result = System.Windows.MessageBox.Show("Please enter a valid transaction date.", "Invalid Transaction Date", MessageBoxButton.OK);
                return;
            }

            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            string startTime = "";
            string endTime = "";
            double hours = 0;
            if (_captureTimeInTimeTracker == "N")
            {
                // Enter Hours
                double iHours = 0;
                if (double.TryParse(durationTextCell.Text, out iHours) == false)
                {
                    var result = System.Windows.MessageBox.Show("Please enter valid hours before saving.", "Invalid Hours", MessageBoxButton.OK);
                    return;
                }
                else if(iHours <= 0 || iHours > 24)
                {
                    var result = System.Windows.MessageBox.Show("Please enter valid hours before saving.", "Invalid Hours", MessageBoxButton.OK);
                    return;
                }
                hours = iHours;
            }
            else
            {
                // Enter From and To Time
                DateTime dtStartTime;
                if (DateTime.TryParse(startTimeText.Text, out dtStartTime) == false)
                {
                    var result = System.Windows.MessageBox.Show("Please enter a valid start time before saving.", "Invalid Start Time", MessageBoxButton.OK);
                    return;
                }

                DateTime dtEndTime;
                if (DateTime.TryParse(endTimeText.Text, out dtEndTime) == false)
                {
                    var result = System.Windows.MessageBox.Show("Please enter a valid end time before saving.", "Invalid End Time", MessageBoxButton.OK);
                    return;
                }
                startTime = dtStartTime.TimeOfDay.ToSage100TimeString();
                endTime = dtEndTime.TimeOfDay.ToSage100TimeString();
                hours = Convert.ToDouble(CalcHours());
            }
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

            newTimeEntry.DepartmentNo = currentTechnician.TechnicianDeptNo;
            newTimeEntry.EmployeeNo = currentTechnician.TechnicianNo;
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            //newTimeEntry.EndTime = ((DateTime)endTimePicker.Value).TimeOfDay.ToSage100TimeString();
            newTimeEntry.EndTime = endTime;
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END

            newTimeEntry.IsModified = true;
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
            //newTimeEntry.StartTime = ((DateTime)startTimePicker.Value).TimeOfDay.ToSage100TimeString();
            newTimeEntry.StartTime = startTime;
            // dch rkl 11/1/2016 use textbox instead of datetime picker for times END
            newTimeEntry.TransactionDate = dtTranDate;
            // dch rkl 01/23/2017 Do not include WT Number and WT Step for miscellaneous time.
            //newTimeEntry.WTNumber = currentTechnician.CurrentWTNumber;
            //newTimeEntry.WTStep = currentTechnician.CurrentWTStep;
            newTimeEntry.EarningsCode = earningCodePicker.SelectedValue.ToString();

            _vm.DailyTimeEntry = newTimeEntry;

            // dch rkl 11/04/2016 Hours calculated above, since they can be entered manually
            //_vm.SaveDailyTimeEntry(Convert.ToDouble(CalcHours()));
            _vm.SaveDailyTimeEntry(Convert.ToDouble(hours));

            // dch rkl 11/01/2016 confirmation that transaction was saved
            var result2 = System.Windows.MessageBox.Show("Miscellaneous Time Entry Saved.", "Entry Saved", MessageBoxButton.OK);

            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new MiscellaneousTimePage();
            
        }

        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            // Cancel - clear fields
            textTransactionDate.Text = DateTime.Now.ToShortDateString();
            startTimeText.Text = "";
            endTimeText.Text = "";
            durationTextCell.Text = "";
            earningCodePicker.SelectedValue = "";
        }
    }
}
