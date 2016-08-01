using System;

using TechDashboard.Tools;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region Daily Time Entry

        #region ERP JT_DailyTimeEntry

        public void FillDailyTimeEntryTable()
        {
            JT_Technician currentTechnician = GetCurrentTechnicianFromDb();

            FillLocalTable<JT_DailyTimeEntry>("where", "(DepartmentNo eq '" + currentTechnician.TechnicianDeptNo + "') and (EmployeeNo eq '" + currentTechnician.TechnicianNo + "')");  // puke filter
        }

        #endregion

        #region Clock In

        /// <summary>
        /// Sets the given technician as being clocked-in to a ticket represented by the
        /// specified scheduled appointment object.  The time and status codes provided
        /// will also be used to update the technician data.
        /// </summary>
        /// <param name="technician">Technician to clock in</param>
        /// <param name="scheduledAppointment">Scheduled Appointment object to clock into</param>
        /// <param name="startDateTime">Start date/time</param>
        /// <param name="technicianStatus">Technician status object for the new, clocked-in status</param>
        /// <param name="serviceTicketStatusCode">Work ticket status for the new, clocked-in status</param>
        public void ClockIn(App_Technician technician, App_ScheduledAppointment scheduledAppointment, 
                            DateTime startDateTime, JT_TechnicianStatus technicianStatus, 
                            JT_MiscellaneousCodes serviceTicketStatusCode)
        {
            // Is this tech already clocked into a ticket?
            if (IsClockedIn(technician)) 
            {
                throw new Exception("Technician is already clocked into a ticket!");
            }

            int rows = 0;

            // Update the erp record with the ticket info
            JT_Technician erpTech = GetTechnician(technician.TechnicianDeptNo, technician.TechnicianNo);
            erpTech.CurrentStartDate = startDateTime.Date;
            erpTech.CurrentStartTime = startDateTime.ToSage100TimeString();
            erpTech.CurrentSalesOrderNo = scheduledAppointment.SalesOrderNumber;
            erpTech.CurrentWTNumber = scheduledAppointment.WorkTicketNumber;
            erpTech.CurrentWTStep = scheduledAppointment.WorkTicketStep;
            rows = _database.Update(erpTech);

            // create the JT_TransactionImportDetail record
            JT_TransactionImportDetail importDetail = new JT_TransactionImportDetail();
            importDetail.RecordType = "S";
            importDetail.SalesOrderNo = scheduledAppointment.SalesOrderNumber;
            importDetail.WTNumber = scheduledAppointment.WorkTicketNumber;
            importDetail.WTStep = scheduledAppointment.WorkTicketStep;
            importDetail.StatusCode = serviceTicketStatusCode.MiscellaneousCode;
            //importDetail.statusDate PUKE>>> not in table spec!
            importDetail.StatusTime = startDateTime.ToSage100TimeString();
            //importDetail.StatusComment = ""; // not used at this time but might be in the future
            rows = _database.Insert(importDetail);


            // puke... do we need this anymore now that we are tracking in/out
            //  on technician record?
            JT_DailyTimeEntry newTimeEntry = new JT_DailyTimeEntry();
            newTimeEntry.DepartmentNo = technician.TechnicianDeptNo;
            newTimeEntry.EmployeeNo = technician.TechnicianNo;
            newTimeEntry.SalesOrderNo = scheduledAppointment.SalesOrderNumber;
            newTimeEntry.WTNumber = scheduledAppointment.WorkTicketNumber;
            newTimeEntry.WTStep = scheduledAppointment.WorkTicketStep;
            newTimeEntry.StartTime = startDateTime.ToSage100TimeString();
            newTimeEntry.IsModified = true;

            rows = _database.Insert(newTimeEntry);
        }

        /// <summary>
        /// Checks to see if the currently-logged-in technician is
        /// clocked into a ticket.
        /// </summary>
        /// <returns>True if current technician is logged into a ticket, False otherwise.</returns>
        public bool IsClockedIn()
        {
            JT_Technician technician = GetCurrentTechnicianFromDb();
            if (technician != null)
            {
                return IsClockedIn(new App_Technician(technician));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the specified technician is clocked into a ticket.
        /// </summary>
        /// <returns>True if technician is logged into a valid ticket, False otherwise.</returns>
        public bool IsClockedIn(App_Technician technician)
        {
            lock(_locker)
            {
                JT_Technician erpTech =
                    _database.Table<JT_Technician>().Where(
                        t => (t.TechnicianDeptNo == technician.TechnicianDeptNo) &&
                             (t.TechnicianNo == technician.TechnicianNo)
                    ).FirstOrDefault();

                if (((erpTech.CurrentSalesOrderNo != null) && (erpTech.CurrentSalesOrderNo.Length > 0)) &&
                    ((erpTech.CurrentWTNumber != null) && (erpTech.CurrentWTNumber.Length > 0)) &&
                    ((erpTech.CurrentWTStep != null) && (erpTech.CurrentWTStep.Length > 0)))
                {
                    // it's possible this tech is clocked in to a ticket.  But, do we have an actual
                    //  ticket or is this left-over data?
                    JT_TechnicianScheduleDetail scheduleDetail =
                        _database.Table<JT_TechnicianScheduleDetail>().Where(
                            sd => (sd.SalesOrderNo == erpTech.CurrentSalesOrderNo) &&
                                  (sd.WTNumber == erpTech.CurrentWTNumber) &&
                                  (sd.WTStep == erpTech.CurrentWTStep)
                        ).FirstOrDefault();

                    if (scheduleDetail != null)
                    {
                        // we do have a clocked-in work ticket for this tech
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Clock Out

        /// <summary>
        /// Sets the given technician as being clocked-out of a ticket represented by the
        /// specified work ticket object.  The time and status codes provided
        /// will also be used to update the technician data.
        /// </summary>
        /// <param name="technician">Technician to clock out</param>
        /// <param name="workTicket">Work ticket clocking out of</param>
        /// <param name="stopDateTime">Stop date/time</param>
        /// <param name="technicianStatus">Technician status object for the new, clocked-out status</param>
        /// <param name="serviceTicketStatusCode">Work ticket status for the new, clocked-out status</param>
        /// <param name="activityCode">Activity code to report</param>
        /// <param name="deptWorked">Department in which work was performed</param>
        /// <param name="earningsCode">Earnings code to report</param>
        /// <param name="meterReading">Meter reading (if any) to report</param>
        /// <param name="workPerformedText">Text/notes</param>
        public void ClockOut(App_Technician technician, App_WorkTicket workTicket,
                             DateTime stopDateTime, JT_TechnicianStatus technicianStatus, JT_MiscellaneousCodes serviceTicketStatusCode,
                             string activityCode, string deptWorked, JT_EarningsCode earningsCode, double hoursBilled, double meterReading,
                             string workPerformedText)
        {
            int rows = 0;
            DateTime startDateTime;

            lock(_locker)
            {
                // Clear out fields for JT_Technician record
                JT_Technician erpTech = GetTechnician(technician.TechnicianDeptNo, technician.TechnicianNo);

                // puke... record starting date/time in local variable
                startDateTime = erpTech.CurrentStartDate; // puke... add time
                startDateTime = startDateTime.Add(erpTech.CurrentStartTime.ToSage100TimeSpan());

                if(hoursBilled <= 0)
                {
                    hoursBilled = (stopDateTime - startDateTime).TotalHours;
                }

                erpTech.CurrentStartDate = new DateTime();
                erpTech.CurrentStartTime = null;
                erpTech.CurrentSalesOrderNo = null;
                erpTech.CurrentWTNumber = null;
                erpTech.CurrentWTStep = null;
                rows = _database.Update(erpTech);

                // insert the JT_TransactionImportDetail record
                JT_TransactionImportDetail importDetail = new JT_TransactionImportDetail();
                importDetail.RecordType = "S";
                importDetail.SalesOrderNo = workTicket.SalesOrderNo;
                importDetail.WTNumber = workTicket.WTNumber;
                importDetail.WTStep = workTicket.WTStep;
                importDetail.StatusCode = serviceTicketStatusCode.MiscellaneousCode;
                //importDetail.statusDate PUKE>>> not in table spec!
                importDetail.StatusTime = stopDateTime.ToSage100TimeString();
                //importDetail.StatusComment = "PUKE"; // not used at this time but might be in the future
                rows = _database.Insert(importDetail);

                // insert another JT_TransactionImportDetail record to record the labor
                importDetail = new JT_TransactionImportDetail();
                importDetail.RecordType = "L";
                importDetail.SalesOrderNo = workTicket.SalesOrderNo;
                importDetail.WTNumber = workTicket.WTNumber;
                importDetail.WTStep = workTicket.WTStep;
                importDetail.EmployeeDeptNo = technician.TechnicianDeptNo;
                importDetail.EmployeeNo = technician.TechnicianNo;
                importDetail.ActivityCode = activityCode;
                importDetail.DeptWorkedIn = deptWorked;
                importDetail.EarningsCode = earningsCode.EarningsCode;
                importDetail.StartTime = startDateTime.ToSage100TimeString();
                importDetail.EndTime = stopDateTime.ToSage100TimeString();
                importDetail.HoursWorked = hoursBilled; // (stopDateTime - startDateTime).TotalHours;
                importDetail.MeterReading = meterReading;
                importDetail.WorkPerformed = workPerformedText;
                rows = _database.Insert(importDetail);
            }
        }


        /// <summary>
        /// Checks to see if the specified technician is clocked into a ticket.
        /// </summary>
        /// <param name="technician">The technician to check</param>
        /// <returns>True if technician is logged into a ticket, False otherwise.</returns>
        public bool IsClockedOut(JT_Technician technician)
        {
            lock(_locker)
            {
                return (!IsClockedIn(new App_Technician(technician)));
            }
        }

        #endregion

        public JT_DailyTimeEntry GetClockedInTimeEntry()
        {
            JT_DailyTimeEntry returnData = null;

            lock (_locker)
            {
                JT_Technician technician = GetCurrentTechnicianFromDb();
                if (technician != null)
                {
                    returnData = GetClockedInTimeEntry(new App_Technician(technician));
                }
            }

            return returnData;
        }

        public JT_DailyTimeEntry GetClockedInTimeEntry(App_Technician technician)
        {
            JT_DailyTimeEntry returnData = null;

            lock (_locker)
            {
                returnData = _database.Table<JT_DailyTimeEntry>().Where(
                dte => (dte.DepartmentNo == technician.TechnicianDeptNo) &&
                       (dte.EmployeeNo == technician.TechnicianNo) &&
                       (dte.StartTime != null) &&
                       (dte.EndTime == null)
                ).FirstOrDefault();
            }

            return returnData;
        }

        public JT_DailyTimeEntry GetTimeEntryData(App_ScheduledAppointment scheduledAppointment)
        {
            JT_DailyTimeEntry returnData = null;
            JT_Technician technician = null;

            lock (_locker)
            {
                technician = GetCurrentTechnicianFromDb();

                if (technician != null)
                {
                    returnData = _database.Table<JT_DailyTimeEntry>().Where(
                        x => x.EmployeeNo == technician.TechnicianNo &&
                        x.DepartmentNo == technician.TechnicianDeptNo &&
                        x.WTNumber == scheduledAppointment.WorkTicketNumber &&
                        x.WTStep == scheduledAppointment.WorkTicketStep
                    ).FirstOrDefault();
                }
            }

            return returnData;
        }

        #endregion
    }
}
