using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
 * Page Name    ClockOutPage
 * Description: Clock Out Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 10/26/2016   DCH     Return to ticket details instead of schedule
 * 10/31/2016   DCH     Set Clock In Status to Service Ticket Status
 * 11/01/2016   DCH     Include department on page
 * 11/01/2016   DCH     Earnings Code should include code + description
 * 11/01/2016   DCH     Make sure default earnings code is not null
 * 11/01/2016   DCH     Only display meter reading if JT_ServiceAgreementPMDetail.Basis = "M"
 * 11/01/2016   DCH     Change date pickers to textboxes, to allow free-form input
 * 11/15/2016   DCH     Add clock in date and clock out date, so hours are calculated when clocking
 *                      out the next day.
 * 11/30/2016   DCH     Add Coverage Checkboxes
 *                      Add technician number and name
 *                      Move layout to XAML instead of generating in code
 * 12/02/2016   DCH     Add fields that are missing compared to original app:
 *                      - Billable Hurs
 *                      - Billing Rate
 *                      - Billable Flag
 *                      - Billable Amount (Extended)
 *                      - Billing Ref Rate
 * 01/12/2017   DCH     Time inputs still not all handled.  For example, if 830P entered, or 830 
 *                      entered, it was kicked out as invalid.
 * 01/12/2017   DCH     Set Tab Index To correct order.
 * 01/16/2017   DCH     If there is no value for the Default Depart Status Code, use the 
 *                      technician's current status
 * 01/23/2017   DCH     If Time Tracker Options is "Y", they enter start/end time.  If "N", they
 *                      enter hours.
 * 01/23/2017   DCH     When clocking out, make sure the service agreement gets captured.
 * 01/27/2017   BK      Adding MinHourlyCostIncrement
 * 02/01/2017   BK      Adding CI Options formatting
 * 02/03/2017   DCH     Move where the hours worked is captured, so it always captures > 24 hours.
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ClockOutPage.xaml
    /// </summary>
    public partial class ClockOutPage : UserControl
    {

        ClockOutPageViewModel _vm;
        App_WorkTicket _workTicket;

        // dch rkl 10/26/2016 return to ticket details instead of scheduled ticket list on cancel
        App_ScheduledAppointment _scheduleDetail;

        // dch rkl 01/23/2017 If Time Tracker Options is "Y", they enter start / end time.If "N", they enter hours.
        string _captureTimeInTimeTracker;
        decimal _MinHourlyCostIncrement;
        CI_Options _ciOptions;

        public ClockOutPage()
        {
            InitializeComponent();
        }

        // dch rkl 10/26/2016 include schedule detail
        //public ClockOutPage(App_WorkTicket workTicket)
        public ClockOutPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduleDetail)
        {
            InitializeComponent();

            _vm = new ClockOutPageViewModel(workTicket);

            _ciOptions = App.Database.GetCIOptions();

            // dch rkl 10/26/2016 return to ticket details instead of scheduled ticket list on cancel
            _scheduleDetail = scheduleDetail;

            // dch rkl 11/30/2016 display coverage checkboxes
            _workTicket = workTicket;

            // dch rkl 12/02/2016 Ticket Details

            // Get Current Technician
            JT_Technician technician = App.Database.GetCurrentTechnicianFromDb();

            // Service Ticket
            textServiceTicket.Content = _scheduleDetail.ServiceTicketNumber;

            // Employee Number and Name
            textEmployeeNumber.Content = technician.FormattedTechnicianNo;
            textEmployeeName.Content = string.Format("{0} {1}",technician.FirstName, technician.LastName);

            // Set Coverage Checkboxes
            // Warranty Repair
            if (_workTicket.DtlWarrantyRepair == "Y") { switchWarrRepair.IsChecked = true; }

            // Labor Coverred on Warranty
            bool bIsChkd = false;
            if (_workTicket.StatusDate != null && _workTicket.RepairItem.MfgLaborWarrantyPeriod != null)
            {
                TimeSpan tsDateDiff = _workTicket.RepairItem.MfgLaborWarrantyPeriod.Subtract(_workTicket.StatusDate);
                if (tsDateDiff.TotalDays > 0 && _workTicket.DtlWarrantyRepair == "Y")
                {
                    switchLaborCovWarr.IsChecked = true;
                    bIsChkd = true;
                }
            }
            if (_workTicket.StatusDate != null && _workTicket.RepairItem.IntLaborWarrantyPeriod != null)
            {
                TimeSpan tsDateDiff = _workTicket.RepairItem.IntLaborWarrantyPeriod.Subtract(_workTicket.StatusDate);
                if (tsDateDiff.TotalDays > 0 && _workTicket.DtlWarrantyRepair == "Y")
                {
                    switchLaborCovWarr.IsChecked = true;
                    bIsChkd = true;
                }
            }

            // Service Agreement Repair
            if (_workTicket.DtlCoveredOnContract == "Y") { switchSvcAgrRepair.IsChecked = true; }

            // LLabor Covered on Service Agreement
            if (_workTicket.IsPreventativeMaintenance && _workTicket.ServiceAgreement.IsPMLaborCovered)
            {
                switchLaborCovSvcAgr.IsChecked = true;
            }
            else if(_workTicket.IsPreventativeMaintenance == false && _workTicket.IsServiceAgreementRepair && _workTicket.ServiceAgreement.IsLaborCovered)
            {
                switchLaborCovSvcAgr.IsChecked = true;
            }

            // Billable Picker
            pickerBillable.ItemsSource = _vm.BillableList;

            // Start Time
            DateTime dtStartDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + _vm.StartTime;
            if (App.Database.GetApplicationSettings().TwentyFourHourTime)
            {
                textStartTime.Text = dtStartDateTime.ToString("HH:mm");
            }
            else
            {
                textStartTime.Text = dtStartDateTime.ToString("hh:mm tt");
            }

            // Start Date
            textStartDate.Text = technician.CurrentStartDate.ToShortDateString();

            // Depart Time
            textEndDate.Text = DateTime.Now.ToShortDateString();
            if (App.Database.GetApplicationSettings().TwentyFourHourTime)
            {
                textDepartTime.Text = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                textDepartTime.Text = DateTime.Now.ToString("hh:mm tt");
            }

            // dch rkl 01/23/2017 If Time Tracker Options is "Y", they enter start / end time.If "N", they enter hours. BEGIN
            // bk rkl 02/01/2017 moving north of sethoursbilled call
            _captureTimeInTimeTracker = "N";
            _MinHourlyCostIncrement = 0;
            JT_TimeTrackerOptions tto = App.Database.GetTimeTrackerOptions();
            if (tto != null && tto.CaptureTimeInTimeTracker != null)
            {
                _captureTimeInTimeTracker = tto.CaptureTimeInTimeTracker;
                _MinHourlyCostIncrement = tto.MinHourlyCostIncrement;
                if (_captureTimeInTimeTracker == "O") { _captureTimeInTimeTracker = "Y"; }
            }
            // Set Hours Billed
            if (textStartTime.Text != null && textDepartTime.Text != null)
            {
                SetHoursBilled();
            }

            // Technician Status
            // dch rkl 01/16/2017 If there is no value for the Default Depart Status Code, use the technician's current status BEGIN
            string statusCode = _vm.DefaultDepartStatusCode;
            if (statusCode == null || statusCode.Trim().Length == 0)
            {
                statusCode = technician.CurrentStatus;
            }
            // dch rkl 01/16/2017 If there is no value for the Default Depart Status Code, use the technician's current status END
            pickerTechnicianStatus.ItemsSource = _vm.TechnicianStatusList;
            for (int i = 0; i < pickerTechnicianStatus.Items.Count; i++)
            {
                // dch rkl 01/16/2017 If there is no value for the Default Depart Status Code, use the technician's current status BEGIN
                //if (((JT_TechnicianStatus)pickerTechnicianStatus.Items[i]).StatusDescription == _vm.DefaultDepartStatusCodeDescription)
                if (((JT_TechnicianStatus)pickerTechnicianStatus.Items[i]).StatusCode == statusCode)
                // dch rkl 01/16/2017 If there is no value for the Default Depart Status Code, use the technician's current status END
                {
                    pickerTechnicianStatus.SelectedIndex = i;
                    break;
                }
            }

            // Ticket Status
            pickerTicketStatus.ItemsSource = _vm.ServiceTicketStatusList;
            pickerTicketStatus.SelectedValue = _vm.DefaultServiceTicketStatusCode;

            // Activity Code
            pickerActivityCode.ItemsSource = _vm.ActivityCodeList;
            pickerActivityCode.SelectedValue = _vm.DefaultActivityCode;

            // Department
            pickerDepartment.ItemsSource = _vm.DepartmentCodesList;
            JT_ActivityCode dfltActCode = new JT_ActivityCode();
            if (_vm.DefaultActivityCode != null) { dfltActCode = App.Database.GetActivityCodeFromDB(_vm.DefaultActivityCode); }
            if (dfltActCode != null) { pickerDepartment.SelectedValue = dfltActCode.DeptWorkedIn; }

            // Earnings Code
            List<JT_EarningsCode> lsEarnCd = new List<Models.JT_EarningsCode>();
            foreach(JT_EarningsCode earncd in _vm.EarningsCodeList)
            {
                // Only include types of Regular or Overtime
                if (earncd.TypeOfEarnings == "O" || earncd.TypeOfEarnings == "R")
                {
                    earncd.EarningsDeductionDesc = string.Format("{0} - {1}", earncd.EarningsCode, earncd.EarningsDeductionDesc);
                    lsEarnCd.Add(earncd);
                }
            }
            pickerEarningsCode.ItemsSource = lsEarnCd;

            // dch rkl 11/01/2016 make sure default earning code is not null
            if (_vm.DefaultEarningCode != null) { pickerEarningsCode.SelectedValue = _vm.DefaultEarningCode; }
            else { pickerEarningsCode.SelectedIndex = 0; }

            // Meter Reading 
            // dch rkl 11/1/2016 per Jeanne, hide the meter reading if JT_ServiceAgreementPMDetail.Basis = "M" BEGIN
            if (_vm.WorkTicket.ServiceAgreement != null && _vm.WorkTicket.ServiceAgreement.PmDetail != null &&
                _vm.WorkTicket.ServiceAgreement.PmDetail.Basis != null && _vm.WorkTicket.ServiceAgreement.PmDetail.Basis == "M")
            {
                labelMeterReading.Visibility = Visibility.Visible;
                editorMeterReading.Visibility = Visibility.Visible;
            }
            else
            {
                labelMeterReading.Visibility = Visibility.Hidden;
                editorMeterReading.Visibility = Visibility.Hidden;
            }
            // dch rkl 11/1/2016 per Jeanne, hide the meter reading if JT_ServiceAgreementPMDetail.Basis = "M" END

            // Set Ref Rate
            SetRefRate();

            // Work Performed
            editorWorkPerformed.MaxHeight = editorWorkPerformed.MinHeight;

            
            if (_captureTimeInTimeTracker == "Y")
            {
                // Enter start/end time
                textStartDate.IsEnabled = true;
                textStartTime.IsEnabled = true;
                textEndDate.IsEnabled = true;
                textDepartTime.IsEnabled = true;
                editorHoursWorked.IsEnabled = false;
            }
            else
            {
                // Enter hours
                textStartDate.IsEnabled = false;
                textStartDate.Text = "";
                textStartTime.IsEnabled = false;
                textStartTime.Text = "";
                textEndDate.IsEnabled = false;
                textEndDate.Text = "";
                textDepartTime.IsEnabled = false;
                textDepartTime.Text = "";
                editorHoursWorked.IsEnabled = true;
            }
            // dch rkl 01/23/2017 If Time Tracker Options is "Y", they enter start / end time.If "N", they enter hours. END

            // dch rkl 01/12/2017 Set Tab Indexes
            textStartDate.TabIndex = 0;
            textStartTime.TabIndex = 1;
            textEndDate.TabIndex = 2;
            textDepartTime.TabIndex = 3;
            editorHoursWorked.TabIndex = 4;
            pickerBillable.TabIndex = 5;
            editorHoursBilled.TabIndex = 6;
            editorBillableRate.TabIndex = 7;
            pickerTechnicianStatus.TabIndex = 8;
            pickerTicketStatus.TabIndex = 9;
            pickerActivityCode.TabIndex = 10;
            pickerDepartment.TabIndex = 11;
            pickerEarningsCode.TabIndex = 12;
            editorMeterReading.TabIndex = 13;
            editorWorkPerformed.TabIndex = 14;
            buttonClockout.TabIndex = 15;
            buttonCancel.TabIndex = 16;
        }

        private void _pickerDepartTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetHoursBilled();
        }

        private void SetHoursBilled()
        {
            DateTime dtStart;
            DateTime dtEnd;

            if (DateTime.TryParse(textStartTime.Text, out dtStart) && DateTime.TryParse(textDepartTime.Text, out dtEnd))
            {
                // dch rkl 11/15/2016 calculate time with dates
                DateTime dtStartDt;
                DateTime.TryParse(textStartDate.Text, out dtStartDt);
                TimeSpan ts2 = new TimeSpan(dtStart.Hour, dtStart.Minute, 0);
                dtStart = dtStartDt + ts2;

                DateTime dtEndDt;
                DateTime.TryParse(textEndDate.Text, out dtEndDt);
                ts2 = new TimeSpan(dtEnd.Hour, dtEnd.Minute, 0);
                dtEnd = dtEndDt + ts2;

                // bk need to round based off _MinHourlyCostIncrement
                editorHoursWorked.Text = Math.Round(dtEnd.Subtract(dtStart).TotalHours, 2).ToString();
                editorHoursWorked.Text = (Math.Ceiling(dtEnd.Subtract(dtStart).TotalHours / (double)_MinHourlyCostIncrement) * (double)_MinHourlyCostIncrement).ToString();
                if (pickerBillable.SelectedValue == "N")
                {
                    editorHoursBilled.Text = "0"; 
                }
                else {
                    editorHoursBilled.Text = editorHoursWorked.Text;
                }
            }
        }

        private void ButtonClockOut_Clicked(object sender, RoutedEventArgs e)
        {
            if (pickerTechnicianStatus.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a technician status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (pickerTicketStatus.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select a ticket status.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (pickerEarningsCode.SelectedIndex < 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Select an earnings code.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            if (pickerActivityCode.SelectedIndex < 0)
            {
                //await DisplayAlert("Activity Code", "Select an activity code.", "OK");
                //return;
                MessageBoxResult result = System.Windows.MessageBox.Show("Select an activity code.", "Status", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            // Validate Dates
            DateTime dtDepart = DateTime.Now;
            double hoursWorked = 0;

            // dch rkl 01/23/2017 captureTimeInTimeTracker == "Y", date and time must be entered
            if (_captureTimeInTimeTracker == "Y")
            {
                DateTime dtArrive;
                if (textStartTime.Text != null)
                {
                    if (DateTime.TryParse(textStartTime.Text, out dtArrive) == false)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("Enter a valid Arrive Date.", "Arrive Date", MessageBoxButton.OK);
                        if (result == MessageBoxResult.OK)
                            return;
                    }
                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Enter a valid Arrive Date.", "Arrive Date", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                        return;
                }

                if (textDepartTime.Text != null)
                {
                    if (DateTime.TryParse(textDepartTime.Text, out dtDepart) == false)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("Enter a valid Depart Date.", "Depart Date", MessageBoxButton.OK);
                        if (result == MessageBoxResult.OK)
                            return;
                    }
                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Enter a valid Depart Date.", "Depart Date", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                        return;
                }

                // dch rkl 02/03/2017 Capture hours worked, for validation
                if (editorHoursWorked != null) { double.TryParse(editorHoursWorked.Text, out hoursWorked); }

            }
            else
            {
                // dch rkl 01/23/2017 captureTimeInTimeTracker == "N", hours must be entered
                if (editorHoursWorked != null) { double.TryParse(editorHoursWorked.Text, out hoursWorked); }
                if (hoursWorked == 0)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Enter valid Hours Worked.", "Hours Worked", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                        return;
                }
            }
            if (hoursWorked > 24)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Time entry cannot exceed 24 hours.  Please adjust your Depart Date/Time and create separate Clock In/Clock Out for additional hours.", "Hours Worked", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            double meterReading = 0;
            double hoursBilled = 0;

            JT_TechnicianStatus selectedTechnicianStatus = pickerTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            JT_MiscellaneousCodes selectedTicketStatus = pickerTicketStatus.SelectedItem as JT_MiscellaneousCodes;
            JT_EarningsCode selectedEarningsCode = pickerEarningsCode.SelectedItem as JT_EarningsCode;
            JT_ActivityCode selectedActivityCode = pickerActivityCode.SelectedItem as JT_ActivityCode;

            try
            {
                if (_vm.IsRepairItemAnEquipmentAsset)
                {
                    meterReading = double.Parse(editorMeterReading.Text);
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
                hoursBilled = double.Parse(editorHoursBilled.Text);
            }
            catch
            {
                // empty
            }

            // dch rkl 01/23/2017 captureTimeInTimeTracker == "Y", date and time must be entered            
            if (_captureTimeInTimeTracker == "Y")
            {
                // dch rkl 02/03/2017 use arrive date
                //_vm.ClockOut(dtDepart.TimeOfDay, selectedTechnicianStatus, selectedTicketStatus, selectedActivityCode,
                //    pickerDepartment.SelectedValue.ToString(), selectedEarningsCode, hoursBilled, meterReading, editorWorkPerformed.Text,
                //    textEndDate.Text, _captureTimeInTimeTracker, hoursWorked, _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber);
                _vm.ClockOut(dtDepart.TimeOfDay, selectedTechnicianStatus, selectedTicketStatus, selectedActivityCode,
                    pickerDepartment.SelectedValue.ToString(), selectedEarningsCode, hoursBilled, meterReading, editorWorkPerformed.Text,
                    textStartDate.Text, _captureTimeInTimeTracker, hoursWorked, _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber);
            }
            else
            {
                TimeSpan ts = new TimeSpan();
                _vm.ClockOut(ts, selectedTechnicianStatus, selectedTicketStatus, selectedActivityCode,
                    pickerDepartment.SelectedValue.ToString(), selectedEarningsCode, hoursBilled, meterReading, editorWorkPerformed.Text, 
                    "", _captureTimeInTimeTracker, hoursWorked, _vm.WorkTicket.ServiceAgreement.ServiceAgreementNumber);
            }

            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new SchedulePage();
        }

        private void EditorWorkPerformed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((editorWorkPerformed.Text != null) && (editorWorkPerformed.Text.Length > 0))
            {
                editorHoursWorked.IsEnabled = true;
            }
            else
            {
                // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
                DateTime dtDepart;
                DateTime dtArrive;
                if (textStartTime.Text != null && textDepartTime.Text != null)
                {
                    if (DateTime.TryParse(textStartTime.Text, out dtArrive) && DateTime.TryParse(textDepartTime.Text, out dtDepart))
                    {
                        editorHoursWorked.Text = Math.Round((dtDepart.TimeOfDay - dtArrive.TimeOfDay).TotalHours, 2).ToString();
                        if (pickerBillable.SelectedValue == "N")
                        {
                            editorHoursBilled.Text = "0";
                        }
                        else {
                            editorHoursBilled.Text = editorHoursWorked.Text;
                        }
                    }
                }
                //_editorHoursBilled.Text = Math.Round((((DateTime)_pickerDepartTime.Value).TimeOfDay - ((DateTime)_pickerStartTime.Value).TimeOfDay).TotalHours, 2).ToString();
                // dch rkl 11/1/2016 use textbox instead of datetime picker for times END
                editorHoursWorked.IsEnabled = false;
            }
        }

        // dch rkl 01/23/2017 If hours worked manually changed... BEGIN
        private void editorHoursWorked_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_captureTimeInTimeTracker == "N" && pickerBillable.SelectedValue != "N")
            {
                editorHoursBilled.Text = editorHoursWorked.Text;
            }
        }
        // dch rkl 01/23/2017 If hours worked manually changed... END

        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;

            // dch rkl 10/26/2016 return to ticket details instead of scheduled ticket list on cancel
            //contentArea.Content = new SchedulePage();
            contentArea.Content = new TicketDetailsPage(_scheduleDetail);

        }

        // dch rkl 11/1/2016 use textbox instead of datetime picker for times BEGIN
        private void textStartTime_LostFocus(object sender, RoutedEventArgs e)
        {
            // Reformat the date
            if ((textStartTime.Text != null) && (textStartTime.Text.Length > 0))
            {
                bool bValidTime = false;
                textStartTime.Text = FormatTime(textStartTime.Text, ref bValidTime);
                if (bValidTime)
                {
                    SetHoursBilled();
                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Invalid start time entered.", "Invalid Time", MessageBoxButton.OK);
                }
            }
        }

        private void textDepartTime_LostFocus(object sender, RoutedEventArgs e)
        {
            // Reformat the date
            if ((textDepartTime.Text != null) && (textDepartTime.Text.Length > 0))
            {
                bool bValidTime = false;
                textDepartTime.Text = FormatTime(textDepartTime.Text, ref bValidTime);
                if (bValidTime)
                {
                    SetHoursBilled();
                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Invalid depart time entered.", "Invalid Time", MessageBoxButton.OK);
                }
            }
        }

        // dch rkl 11/15/2016 Add Lost Focus for date changes
        private void textStartDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime dtDate;
            if (DateTime.TryParse(textStartDate.Text, out dtDate))
            {
                textStartDate.Text = dtDate.ToShortDateString();
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Invalid start date entered.", "Invalid Date", MessageBoxButton.OK);
            }
        }

        private void textEndDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime dtDate;
            if (DateTime.TryParse(textEndDate.Text, out dtDate))
            {
                textEndDate.Text = dtDate.ToShortDateString();
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Invalid end date entered.", "Invalid Date", MessageBoxButton.OK);
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

        // dch rkl 11/22/2016 if activity code changed, set department to default for this activity code
        // If the department for the activity code is null/blank, use the employee department
        private void pickerActivityCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Set the Department
            string sActivityCode = "";
            if (pickerActivityCode.SelectedIndex > -1 && pickerActivityCode.SelectedValue != null) { sActivityCode = pickerActivityCode.SelectedValue.ToString(); }
            JT_ActivityCode activityCode = App.Database.GetActivityCodeFromDB(sActivityCode);
            if (activityCode != null && activityCode.DeptWorkedIn != null)
            {
                pickerDepartment.SelectedValue = activityCode.DeptWorkedIn;
            }
            else
            {
                JT_Technician tech = App.Database.GetCurrentTechnicianFromDb();
                if (tech != null && tech.TechnicianDeptNo != null)
                {
                    pickerDepartment.SelectedValue = tech.TechnicianDeptNo;
                }
            }

            // Determine if the Billable Dropdown is enabled
            // An activity code is considered billable when a misc item code is assigned to the activity code
            if (activityCode != null && activityCode.BillingMiscCode != null && activityCode.BillingMiscCode.Trim().Length > 0)
            {
                // When a billable activity code is assigned to the activity code, the billing status dropdown is enabled.
                pickerBillable.IsEnabled = true;
                SetBillable();
            }
            else
            {
                // When a non-billable activity code is assigned to the activity code, the billing
                // status is disabled and set to no charge.
                pickerBillable.IsEnabled = false;
                pickerBillable.SelectedValue = "N";
            }
        }

        // dch rkl 12/02/2016 Billable Flag Changed
        private void pickerBillable_SelectionChanged(object sender, EventArgs e)
        {
            // Re-calculate billing rates and Billing Amount
            SetRefRate();
            var billable = (ComboBox)sender;

            // if not do not bill, disable and set BillableHours to 0
            if (billable.SelectedValue == "N")
            {
                editorHoursBilled.Text = "0";
                editorHoursBilled.IsEnabled = false;
            } else
            {
                editorHoursBilled.Text = editorHoursWorked.Text;
                editorHoursBilled.IsEnabled = true;
            }
        }

        // dch rkl 12/02/2016 Set Billable/Non-Billable Flag
        private void SetBillable()
        {
            pickerBillable.SelectedValue = "B";     // default to billable
            string sActivityCode = "";
            if (pickerActivityCode.SelectedIndex > -1 && pickerActivityCode.SelectedValue != null) { sActivityCode = pickerActivityCode.SelectedValue.ToString(); }
            if (sActivityCode.Length > 0)
            {
                JT_ActivityCode activityCode = App.Database.GetActivityCodeFromDB(sActivityCode);
                if (activityCode.BillingMiscCode == null)
                {
                    pickerBillable.SelectedValue = "N";
                    pickerBillable.IsEnabled = false;
                }
                else if (_workTicket.ServiceAgreement.BillingType != null && _workTicket.ServiceAgreement.BillingType == "P" 
                    && (bool)switchSvcAgrRepair.IsChecked)
                    { pickerBillable.SelectedValue = "N"; }
                else if (_workTicket.ServiceAgreement.BillingType== "P" && (bool)switchSvcAgrRepair.IsChecked)
                    { pickerBillable.SelectedValue = "X"; }
                else if (_workTicket.ServiceAgreement.IsLaborCovered == false && _workTicket.ServiceAgreement.BillingType != "T"
                    && _workTicket.ServiceAgreement.BillingType != "P") { pickerBillable.SelectedValue = "X"; }
                else if (_workTicket.ServiceAgreement.BillingType == "F" && _workTicket.ServiceAgreement.IsLaborCovered == false)
                    { pickerBillable.SelectedValue = "N"; }
            }
        }

        // dch rkl 12/02/2016 Set Ref Rate
        private void SetRefRate()
        {
            decimal billingRate = 0;

            string sActivityCode = "";
            decimal billingRateMultiplier = 1;
            if (pickerActivityCode.SelectedIndex > -1 && pickerActivityCode.SelectedValue != null) { sActivityCode = pickerActivityCode.SelectedValue.ToString(); }
            if (sActivityCode.Length > 0)
            {
                JT_ActivityCode activityCode = App.Database.GetActivityCodeFromDB(sActivityCode);
                billingRateMultiplier = activityCode.BillingRateMultiplier;
                if (billingRateMultiplier == 0) { billingRateMultiplier = 1; }
            }

            if (_workTicket.DtlCoverageExceptionCode != null && _workTicket.DtlCoverageExceptionCode.Trim().Length > 0)
            {
                billingRate = _workTicket.DtlCoverageExceptionFixedRate;
            }
            if (_workTicket.IsPreventativeMaintenance)
            {
                billingRate = Math.Round(_workTicket.ServiceAgreement.PmDetail.Rate * billingRateMultiplier, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (_workTicket.ServiceAgreement.DetailRate != 0)
                {
                    billingRate = Math.Round(_workTicket.ServiceAgreement.DetailRate * billingRateMultiplier, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    billingRate = Math.Round(_workTicket.ServiceAgreement.StandardLaborRate * billingRateMultiplier, 2, MidpointRounding.AwayFromZero);
                }
            }

            // Look for customer billing rates
            if (sActivityCode.Trim().Length > 0)
            {
                App_Customer customer = App.Database.GetAppCustomer(_workTicket);
                JT_CustomerBillingRates custBillRate = App.Database.GetJT_CustomerBillingRate(customer.ARDivisionNo, customer.CustomerNo, 
                    sActivityCode);
                if (custBillRate != null)
                {
                    billingRate = Math.Round(custBillRate.BillRatePerHour * billingRateMultiplier, 2, MidpointRounding.AwayFromZero);
                }
            }

            // Set Rate based on Technician
            if (billingRate == 0)
            {
                JT_Technician technician = App.Database.GetCurrentTechnicianFromDb();

                billingRate = Math.Round(technician.StandardBillingRate * billingRateMultiplier, 2, MidpointRounding.AwayFromZero);
            }

            // Set the value in the rate box
            editorRefRate.Text = billingRate.ToString("C2");
            
            // Billable Rate
            SetBillableRate();
        }

        // dch rkl 12/02/2016 Set Billable Rate
        private void SetBillableRate()
        {
            decimal billingRate = 0;

            string sBillable = "";
            if (pickerBillable.SelectedIndex > -1 && pickerBillable.SelectedValue != null) { sBillable = pickerBillable.SelectedValue.ToString(); }

            string sActivityCode = "";
            string sBillingMiscCode = "";
            if (pickerActivityCode.SelectedIndex > -1 && pickerActivityCode.SelectedValue != null) { sActivityCode = pickerActivityCode.SelectedValue.ToString(); }
            if (sActivityCode.Length > 0)
            {
                JT_ActivityCode activityCode = App.Database.GetActivityCodeFromDB(sActivityCode);
                if (activityCode != null && activityCode.BillingMiscCode != null)
                {
                    sBillingMiscCode = activityCode.BillingMiscCode;
                }
            }

            if (sBillable != "B")
                { billingRate = 0; }
            else if ((bool)switchLaborCovSvcAgr.IsChecked && _workTicket.ServiceAgreement.BillingType == "F" 
                && _workTicket.DtlCoverageExceptionCode == null && sBillingMiscCode == "")
                { billingRate = 0; }
            else if (_workTicket.ServiceAgreement.IsLaborCovered == true && _workTicket.IsCoveredOnContract == true)
                { billingRate = 0; }
            else if ((bool)switchLaborCovWarr.IsChecked && _workTicket.DtlCoverageExceptionCode == null 
                && _workTicket.SE_OverridePricing != "Y")
                { billingRate = 0; }
            else { billingRate = Decimal.Parse(editorRefRate.Text.Replace("$", "")); }

            // Set the value in the rate box
            editorBillableRate.Text = billingRate.ToString("C2");

            // Set the value in the Billable Amount Box
            decimal hours;
            decimal.TryParse(editorHoursBilled.Text.Replace("$", ""), out hours);
            editorBillableAmount.Text = Math.Round(billingRate * hours, 2, MidpointRounding.AwayFromZero).ToString("C2");
        }

        // dch rkl 11/22/2016 on focus on time, select all text
        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            tbx.SelectAll();
        }
    }
}
