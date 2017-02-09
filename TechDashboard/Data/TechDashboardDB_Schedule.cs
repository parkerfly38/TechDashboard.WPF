using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_Schedule.cs
     * 12/01/2016 DCH Correct misspelling of GetApplicationSettings
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region Technician Schedule Detail

        /// <summary>
        /// Retreives all technician schedule detail records from the ERP connection 
        /// for a given technician and fills the local JT_TechnicianScheduleDetail table.
        /// </summary>
        /// <param name="technicianNumber">The technician's employee number</param>
        public void FillTechnicianScheduleDetailTable(string technicianNumber)
        {
            FillLocalTable<JT_TechnicianScheduleDetail>("where", "TechnicianNo eq '" + technicianNumber + "'");

            if (technicianNumber == "0000203")
            {
                JT_TechnicianScheduleDetail skedDetail = new JT_TechnicianScheduleDetail()
                {
                    TechnicianDeptNo = "13",
                    TechnicianNo = "0000202",
                    SalesOrderNo = "0001671",
                    WTNumber = "001",
                    WTStep = "001",
                    ScheduleDate = DateTime.Now,
                    StartTime = "1100",
                    HoursScheduled = 5
                };
                _database.Insert(skedDetail);
            }

            // now that we have the schedule details, remove any that don't match our date range
            // First, get the number of days before and after today that will be allowed.
            App_Settings appSettings = GetApplicationSettings();

            // Find the "bad" schedule details -- date less than allowed lower limint and 
            //  greater than allowed upper limit
            DateTime lowerLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysBefore) * (-1))).Date;
            DateTime upperLimit = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysAfter))).Date;

            List<JT_TechnicianScheduleDetail> scheduleDetails = 
                _database.Table<JT_TechnicianScheduleDetail>().Where(
                    sd => (sd.ScheduleDate < lowerLimit) ||
                          (sd.ScheduleDate > upperLimit)
                ).ToList();

            // Get rid of these records from our DB
            foreach (JT_TechnicianScheduleDetail detail in scheduleDetails)
            {
                System.Diagnostics.Debug.WriteLine("Removing JT_TechnicianScheduleDetail object with date " + detail.ScheduleDate.ToString("yyyy-MM-dd"));
                _database.Delete(detail);
            }
        }

        public List<JT_TechnicianScheduleDetail> GetTechnicianScheduleDetailFromDB()
        {
            JT_Technician technician = null;
            List<JT_TechnicianScheduleDetail> details = null;

            lock (_locker)
            {
                technician = GetCurrentTechnicianFromDb();
                if(technician != null)
                {
                    details = GetTechnicianScheduleDetailFromDB(technician);
                }                
            }

            return details;
        }

        public List<JT_TechnicianScheduleDetail> GetTechnicianScheduleDetailFromDB(JT_Technician technician)
        {
            List<JT_TechnicianScheduleDetail> details = null;

            if (technician != null)
            {
                lock (_locker)
                {
                    details =
                        _database.Table<JT_TechnicianScheduleDetail>().OrderBy(
                            sd => sd.ScheduleDate
                        ).ThenBy(
                            sd => sd.StartTime
                        ).ThenBy(
                            sd => sd.WTNumber
                        ).ThenBy(
                            sd => sd.WTStep
                        ).ToList();
                }

                if (details != null)
                {
                    foreach (JT_TechnicianScheduleDetail detail in details)
                    {
                        detail.IsCurrent =
                            ((detail.SalesOrderNo == technician.CurrentSalesOrderNo) &&
                             (detail.WTNumber == technician.CurrentWTNumber) &&
                             (detail.WTStep == technician.CurrentWTStep));
                    }
                }
            }

            return details;
        }

        protected void SaveScheduleDetailAsCurrent(JT_TechnicianScheduleDetail scheduleDetail)
        {
            int rows = 0;

            lock (_locker)
            {
                // Unset any techs that are marked as "current"
                List<JT_TechnicianScheduleDetail> currentScheduleDetails = _database.Table<JT_TechnicianScheduleDetail>().Where(t => t.IsCurrent == true).ToList();
                if (currentScheduleDetails.Count > 0)
                {
                    foreach (JT_TechnicianScheduleDetail scheduleDetailInList in currentScheduleDetails)
                    {
                        scheduleDetailInList.IsCurrent = false;
                    }
                    _database.UpdateAll(currentScheduleDetails);
                }

                // Set this tech as current.
                scheduleDetail.IsCurrent = true;
                rows = _database.Update(scheduleDetail);
            }

            if (rows > 0)
            {
                OnCurrentScheduleDetailChanged(EventArgs.Empty);
            }
        }

        public JT_TechnicianScheduleDetail RetrieveCurrentScheduleDetail()
        {
            JT_TechnicianScheduleDetail currentDetail = null;
            JT_Technician currentTechnician = null;

            lock (_locker)
            {
                currentTechnician = GetCurrentTechnicianFromDb();

                if (currentTechnician != null)
                {
                    currentDetail =
                        _database.Table<JT_TechnicianScheduleDetail>().Where(
                            sd => (sd.SalesOrderNo == currentTechnician.CurrentSalesOrderNo) &&
                                  (sd.WTNumber == currentTechnician.CurrentWTNumber) &&
                                  (sd.WTStep == currentTechnician.CurrentWTStep)
                         ).FirstOrDefault();
                }
            }

            if (currentDetail != null)
            {
                // if we found it, it must really be current
                currentDetail.IsCurrent = true;
            }

            return currentDetail;
        }

        #endregion


        #region App_ScheduledAppointment

        public App_ScheduledAppointment RetrieveCurrentScheduledAppointment()
        {
            lock(_locker)
            {
                App_Technician technician = new App_Technician(GetCurrentTechnicianFromDb());

                return RetrieveCurrentScheduledAppointment(technician);
            }
        }

        public App_ScheduledAppointment RetrieveCurrentScheduledAppointment(App_Technician technician)
        {
            lock(_locker)
            {
                JT_Technician erpTech =
                    _database.Table<JT_Technician>().Where(
                        t => (t.TechnicianDeptNo == technician.TechnicianDeptNo) &&
                             (t.TechnicianNo == technician.TechnicianNo)
                    ).FirstOrDefault();

                return RetrieveCurrentScheduledAppointment(erpTech);
            }
        }

        protected App_ScheduledAppointment RetrieveCurrentScheduledAppointment(JT_Technician technician)
        {
            JT_TechnicianScheduleDetail  currentDetail = null;
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                if ((technician != null) && (IsClockedIn()))
                {
                    currentDetail =
                        _database.Table<JT_TechnicianScheduleDetail>().Where(
                            sd => (sd.SalesOrderNo == technician.CurrentSalesOrderNo) &&
                                  (sd.WTNumber == technician.CurrentWTNumber) &&
                                  (sd.WTStep == technician.CurrentWTStep)
                         ).FirstOrDefault();

                    salesOrderHeader = GetSalesOrderHeader(currentDetail);
                }

                if (currentDetail == null)
                {
                    return null;
                }
                else
                {
                    // if we found it, it must really be current
                    currentDetail.IsCurrent = true;
                    return new App_ScheduledAppointment(currentDetail, salesOrderHeader);
                }
            }
        }

        public App_ScheduledAppointment GetScheduledAppointment()
        {
            JT_TechnicianScheduleDetail scheduledDetail = GetTechnicianScheduleDetailFromDB().Where(x => x.IsCurrent).FirstOrDefault();
            SO_SalesOrderHeader soHeader = GetSalesOrderHeader(scheduledDetail);

            return new App_ScheduledAppointment(scheduledDetail, soHeader);
        }

        public List<App_ScheduledAppointment> GetScheduledAppointments()
        {
            List<App_ScheduledAppointment> returnData;
            SO_SalesOrderHeader soHeader;

            lock (_locker)
            {
                returnData = new List<App_ScheduledAppointment>();
                if (returnData != null)
                {
                    foreach (JT_TechnicianScheduleDetail scheduledDetail in GetTechnicianScheduleDetailFromDB())
                    {
                        soHeader = GetSalesOrderHeader(scheduledDetail);
                        returnData.Add(new App_ScheduledAppointment(scheduledDetail, soHeader));
                    }
                }
            }

            return returnData;
        }

        public List<App_ScheduledAppointment> GetScheduledAppointmentsNoDupes()
        {
            List<App_ScheduledAppointment> returnData;
            SO_SalesOrderHeader soHeader;

            lock (_locker)
            {
                returnData = new List<App_ScheduledAppointment>();
                if (returnData != null)
                {
                    foreach (JT_TechnicianScheduleDetail scheduledDetail in GetTechnicianScheduleDetailFromDB())
                    {
                        soHeader = GetSalesOrderHeader(scheduledDetail);
                        if (returnData.Where(x => x.ServiceTicketNumber == scheduledDetail.ServiceTicketNumber).Count() == 0)
                            returnData.Add(new App_ScheduledAppointment(scheduledDetail, soHeader));
                    }
                }
            }

            return returnData;
        }

        #endregion
    }
}
