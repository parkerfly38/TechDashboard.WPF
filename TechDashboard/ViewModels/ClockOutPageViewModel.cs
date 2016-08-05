using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using TechDashboard.Models;
using TechDashboard.Tools;

namespace TechDashboard.ViewModels
{
    public class ClockOutPageViewModel : INotifyPropertyChanged
    {
        private JT_Technician _currentTechnician;

        protected App_WorkTicket _workTicket;
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

        protected JT_TechnicianScheduleDetail _scheduleDetail;
        public JT_TechnicianScheduleDetail ScheduleDetail
        {
            get { return _scheduleDetail; }
        }

        public TimeSpan StartTime
        {
            get { return _currentTechnician.CurrentStartTime.ToSage100TimeSpan(); }
        }

        public bool IsRepairItemAnEquipmentAsset
        {
            get { return _workTicket.RepairItem.IsEquipmentAsset; }
        }

        public ClockOutPageViewModel(App_WorkTicket workTicket)
        {
            _currentTechnician = App.Database.GetCurrentTechnicianFromDb();
            _workTicket = workTicket;
            _scheduleDetail = App.Database.RetrieveCurrentScheduleDetail();
            _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
            _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
            _earningsCodeList = App.Database.GetEarningsCodesFromDB();
            _activityCodeList = App.Database.GetActivityCodesFromDB();
            _erpOptions = App.Database.GetErpOptions();
        }


        public void ClockOut(TimeSpan departTime, JT_TechnicianStatus technicianStatus, JT_MiscellaneousCodes serviceTicketStatus,
                             JT_ActivityCode activityCode, string departmentWorked, JT_EarningsCode earningsCode, double hoursBilled,
                             double meterReading, string workPerformedText)
        {
            DateTime clockOutTime = DateTime.Today;
            clockOutTime = clockOutTime.Add(departTime);

            App.Database.ClockOut(
                App.CurrentTechnician, _workTicket, clockOutTime, technicianStatus, serviceTicketStatus, activityCode.ActivityCode, 
                departmentWorked, earningsCode, hoursBilled, meterReading, workPerformedText
            );
        }
    }
}
