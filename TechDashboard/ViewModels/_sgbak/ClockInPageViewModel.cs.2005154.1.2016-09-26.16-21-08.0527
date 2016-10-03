using System;
using System.Collections.Generic;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class ClockInPageViewModel
    {
        protected App_ErpOptions _erpOptions;
        public string DefaultArriveStatusCode
        {
            get { return _erpOptions.DefaultTechnicianArriveStatusCode; }
        }

        public string DefaultArriveStatusCodeDescription
        {
            get { return _erpOptions.DefaultTechnicianArriveStatusCodeDescription; }
        }

        public string DefaultDepartStatusCode
        {
            get { return _erpOptions.DefaultTechnicianDepartStatusCode; }
        }

        public string DefaultDepartStatusCodeDescription
        {
            get { return _erpOptions.DefaultTechnicianDepartStatusCodeDescription; }
        }

        public string DefaultServiceTicketArriveStatusCode
        {
            get { return _erpOptions.DefaultServiceTicketArriveStatusCode; }
        }

        public string DefaultServiceTicketArriveStatusCodeDescription
        {
            get { return _erpOptions.DefaultServiceTicketArriveStatusCodeDescription; }
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

        protected App_ScheduledAppointment _scheduleDetail;
        public App_ScheduledAppointment ScheduleDetail
        {
            get { return _scheduleDetail; }
        }

        public ClockInPageViewModel()
        {
            _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
            _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
            _erpOptions = App.Database.GetErpOptions();
        }

        public ClockInPageViewModel(App_ScheduledAppointment scheduleDetail)
        {
            _scheduleDetail = scheduleDetail;
            _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
            _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
            _erpOptions = App.Database.GetErpOptions();
        }

        public bool IsClockedIn()
        {
            return App.Database.IsClockedIn();
        }

        public void ClockIn(TimeSpan arriveTime, JT_TechnicianStatus technicianStatus, JT_MiscellaneousCodes serviceTicketStatus)
        {
            DateTime clockInTime = DateTime.Today;
            clockInTime = clockInTime.Add(arriveTime);
            App.Database.ClockIn(App.CurrentTechnician, _scheduleDetail, clockInTime, technicianStatus, serviceTicketStatus);
            _scheduleDetail.IsCurrent = true;
        }
    }
}
