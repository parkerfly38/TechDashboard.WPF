using System;
using System.Collections.Generic;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * ClockInPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/

    public class ClockInPageViewModel
    {
        #region Properties
        
        // dch rkl 11/04/2016 These are dependent on JT_FieldServiceOptions or JT_Technician Values
        protected string _defaultArriveStatusCodeDescription;
        protected string _defaultArriveStatusCode;

        protected App_ErpOptions _erpOptions;

        public string DefaultArriveStatusCode
        {
            // dch rkl 11/04/2016 This is dependent on JT_FieldServiceOptions or JT_Technician Values
            //get { return _erpOptions.DefaultTechnicianArriveStatusCode; }
            get { return _defaultArriveStatusCode; }
        }

        public string DefaultArriveStatusCodeDescription
        {
           
            //get { return _erpOptions.DefaultTechnicianArriveStatusCodeDescription; }
            get { return _defaultArriveStatusCodeDescription; }
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

        #endregion

        public ClockInPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
                _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
                _erpOptions = App.Database.GetErpOptions();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ClockInPageViewModel()");
            }
        }

        public ClockInPageViewModel(App_ScheduledAppointment scheduleDetail)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _scheduleDetail = scheduleDetail;
                _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
                _serviceTicketStatusList = App.Database.GetWorkTicketStatusesFromDB();
                _erpOptions = App.Database.GetErpOptions();

                // dch rkl 11/04/2016 Default Arrive Status Code is dependent on JT_FieldServiceOptions or JT_Technician Values BEGIN
                if (_erpOptions.DefaultTechnicianArriveStatusCode != null && _erpOptions.DefaultTechnicianArriveStatusCode.Trim().Length > 0)
                {
                    _defaultArriveStatusCode = _erpOptions.DefaultTechnicianArriveStatusCode;
                }
                else
                {
                    _defaultArriveStatusCode = App.CurrentTechnician.CurrentStatus;
                }
                if (_defaultArriveStatusCode != null && _defaultArriveStatusCode.Trim().Length > 0)
                {
                    JT_TechnicianStatus techStatus = App.Database.GetTechnicianStatusFromDB(_defaultArriveStatusCode);
                    if (techStatus != null && techStatus.StatusDescription != null) { _defaultArriveStatusCodeDescription = techStatus.StatusDescription; }
                }
                else
                {
                    _defaultArriveStatusCodeDescription = "";
                }
                // dch rkl 11/04/2016 Default Arrive Status Code is dependent on JT_FieldServiceOptions or JT_Technician Values END
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ClockInPageViewModel(App_ScheduledAppointment scheduleDetail)");
            }

        }

        public bool IsClockedIn()
        {
            return App.Database.IsClockedIn();
        }

        public void ClockIn(TimeSpan arriveTime, JT_TechnicianStatus technicianStatus, JT_MiscellaneousCodes serviceTicketStatus)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                DateTime clockInTime = DateTime.Today;
                clockInTime = clockInTime.Add(arriveTime);
                App.Database.ClockIn(App.CurrentTechnician, _scheduleDetail, clockInTime, technicianStatus, serviceTicketStatus);
                _scheduleDetail.IsCurrent = true;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ClockIn");
            }
        }
    }
}
