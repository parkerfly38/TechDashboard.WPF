using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TechDashboard.Data;
using TechDashboard.Models;
using TechDashboard.Tools;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * ClockOutPageViewModel.cs
     * 11/30/2016 DCH Allow blank department
     * 12/02/2016 DCH Add BillableFlag and BillableDesc
     * 01/23/2017 DCH If Time Tracker Options is "Y", they enter start/end time.  If "N", they
     *                enter hours.
     * 01/23/2017 DCH When clocking out, make sure the service agreement gets captured.
     * 02/03/2017 DCH When clocking out over two days, use the start (clock-in) date as the tran date.
     *********************************************************************************************************/
    public class ClockOutPageViewModel : INotifyPropertyChanged
    {
        // dch rkl 12/02/2016 Billable Flags
        public partial class App_Billable
        {
            #region Properties

            private string _billableFlag;
            private string _billableDesc;

            public string BillableFlag
            {
                get {  return _billableFlag; }
                set { _billableFlag = value; }
            }

            public string BillableDesc
            {
                get { return _billableDesc; }
                set { _billableDesc = value; }
            }

            #endregion

            public App_Billable(string billableFlag, string billableDesc)
            {
                _billableFlag = billableFlag;
                _billableDesc = billableDesc;
            }
        }

        #region Properties

        private JT_Technician _currentTechnician;

        protected App_WorkTicket _workTicket;

        // dch rkl 12/02/2016 Billable Flags
        private List<App_Billable> _billableList;

        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        protected App_ErpOptions _erpOptions;

        public string DefaultDepartStatusCode
        {
            get { return _erpOptions.DefaultTechnicianDepartStatusCode; }
        }

        public string DefaultServiceTicketStatusCode
        {
            get { return _workTicket.StatusCode; }
        }

        public string DefaultActivityCode
        {
            get { return _workTicket.ActivityCode; }
        }

        public string DefaultEarningCode
        {
            get { return _activityCodeList.Where(x => x.ActivityCode == _workTicket.ActivityCode).Select(x => x.DefaultEarningCode).FirstOrDefault<string>(); }
        }

        public string DefaultDepartStatusCodeDescription
        {
            get { return _erpOptions.DefaultTechnicianDepartStatusCodeDescription; }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        protected List<JT_TechnicianStatus> _technicianStatusList;
        public List<JT_TechnicianStatus> TechnicianStatusList
        {
            get { return _technicianStatusList; }
        }

        // dch rkl 11/01/2016 Department Codes BEGIN
        protected List<JT_MiscellaneousCodes> _departmentCodesList;

        public List<JT_MiscellaneousCodes> DepartmentCodesList
        {
            get { return _departmentCodesList; }
        }
        // dch rkl 11/01/2016 Department Codes END

        protected List<JT_MiscellaneousCodes> _serviceTicketStatusList;
        public List<JT_MiscellaneousCodes> ServiceTicketStatusList
        {
            get { return _serviceTicketStatusList; }
        }

        protected List<JT_EarningsCode> _earningsCodeList;
        public List<JT_EarningsCode> EarningsCodeList
        {
            get { return _earningsCodeList; }
        }

        protected List<JT_ActivityCode> _activityCodeList;
        public List<JT_ActivityCode> ActivityCodeList
        {
            get { return _activityCodeList; }
        }

        protected JT_DailyTimeEntry _timeEntry;

        protected JT_TechnicianScheduleDetail _scheduleDetail;
        public JT_TechnicianScheduleDetail ScheduleDetail
        {
            get { return _scheduleDetail; }
        }

        public TimeSpan StartTime
        {
            //get { return _timeEntry.StartTime.ToSage100TimeSpan(); }
            get { return _currentTechnician.CurrentStartTime.ToSage100TimeSpan(); }
        }

        public bool IsRepairItemAnEquipmentAsset
        {
            get { return _workTicket.RepairItem.IsEquipmentAsset; }
        }

        // dch rkl 12/02/2016 Billable Flags
        public List<App_Billable> BillableList
        {
            get
            {
                return _billableList;
            }

            set
            {
                _billableList = value;
            }
        }

        #endregion

        public ClockOutPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _currentTechnician = App.Database.GetCurrentTechnicianFromDb();
                //_timeEntry = App.Database.GetClockedInTimeEntry();
                _workTicket = workTicket;
                _scheduleDetail = App.Database.RetrieveCurrentScheduleDetail();
                _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
                _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
                _earningsCodeList = App.Database.GetEarningsCodesFromDB();
                _activityCodeList = App.Database.GetActivityCodesFromDB();
                _erpOptions = App.Database.GetErpOptions();

                // dch rkl 12/02/2016 Billable Flags
                _billableList = new List<App_Billable>();
                _billableList.Add(new App_Billable("B", "Billable"));
                _billableList.Add(new App_Billable("N", "Do Not Bill"));
                _billableList.Add(new App_Billable("X", "No Charge"));

                // dch rkl 11/01/2016 Department Codes BEGIN
                _departmentCodesList = App.Database.GetMiscellaneousCodesFromDB("M", "DP");
                for (int i=0; i < _departmentCodesList.Count; i++)
                {
                    _departmentCodesList[i].Description = string.Format("{0} - {1}", _departmentCodesList[i].MiscellaneousCode, _departmentCodesList[i].Description);
                }
                // dch rkl 11/01/2016 Department Codes END

                // dch rkl 11/30/2016 allow blank department BEGIN
                JT_MiscellaneousCodes blankCode = new JT_MiscellaneousCodes();
                blankCode.AddtlDescNum = "";
                blankCode.CodeType = "";
                blankCode.Description = "";
                blankCode.MiscellaneousCode = "";
                blankCode.RecordType = "";
                _departmentCodesList.Add(blankCode);
                _departmentCodesList = _departmentCodesList.OrderBy(item => item.Description).ToList();
                // dch rkl 11/30/2016 allow blank department END
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ClockOut(App_WorkTicket workTicket)");
            }
        }


        // dch rkl 11/15/2016 Add clock out date
        // dch rkl 01/23/2017 Check value of CaptureTimeInTimeTracker to see if hours or timespan is entered
        // dch rkl 01/23/2017 Include Service Agreement Code
        // bk  rkl 01/26/2017 adjust to only include transaction date
        public void ClockOut(TimeSpan departTime, JT_TechnicianStatus technicianStatus, JT_MiscellaneousCodes serviceTicketStatus,
                             JT_ActivityCode activityCode, string departmentWorked, JT_EarningsCode earningsCode, double hoursBilled,
                             double meterReading, string workPerformedText, string clockOutDate, string captureTimeInTimeTracker, 
                             double hoursWorked, string svcAgmtContractCode)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                if (captureTimeInTimeTracker == "Y")
                {
                    // dch rkl 11/15/2016 Add clock out date
                    //DateTime clockOutTime = DateTime.Today;
                    DateTime clockOutTime = DateTime.Parse(clockOutDate);

                    clockOutTime = clockOutTime.Add(departTime);

                    // dch rkl 02/03/2017 Include clockOutDate
                    //App.Database.ClockOut(App.CurrentTechnician, _workTicket, clockOutTime, technicianStatus, serviceTicketStatus, 
                    //    activityCode.ActivityCode, departmentWorked, earningsCode, hoursBilled, meterReading, workPerformedText, svcAgmtContractCode);
                    App.Database.ClockOut(App.CurrentTechnician, _workTicket, clockOutTime, technicianStatus, serviceTicketStatus,
                        activityCode.ActivityCode, departmentWorked, earningsCode, hoursBilled, meterReading, workPerformedText, 
                        svcAgmtContractCode, clockOutDate);
                }
                else
                {
                    App.Database.ClockOut(App.CurrentTechnician, _workTicket, technicianStatus, serviceTicketStatus, activityCode.ActivityCode,
                        departmentWorked, earningsCode, hoursBilled, meterReading, workPerformedText, hoursWorked, svcAgmtContractCode);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.BlockOutPageViewModel.ClockOut()");
            }
        }
    }
}
