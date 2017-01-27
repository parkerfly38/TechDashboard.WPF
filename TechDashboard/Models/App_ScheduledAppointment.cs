using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_ScheduledAppointment.cs
     * 12/02/2016 DCH Correct misspelling of GetApplicationSettings
     *********************************************************************************************************/
    public class App_ScheduledAppointment
    {
        private string _salesOrderNumber;
        private string _workTicketNumber;
        private string _workTicketStep;
        private DateTime _scheduleDate;
        private string _startTime;
        private string _actualStartTime;
        private string _name;
        private string _location;
        private string _phone;
        private bool _isCurrent;
        private string _endTime;

        public string ServiceTicketNumber
        {
            get
            {
                return App_WorkTicket.FormatWorkTicketNumber(_salesOrderNumber, _workTicketNumber, _workTicketStep);
            }
        }

        public string SalesOrderNumber
        {
            get { return _salesOrderNumber; }
        }

        public string WorkTicketNumber
        {
            get { return _workTicketNumber; }
        }

        public string WorkTicketStep
        {
            get { return _workTicketStep; }
        }

        /// <summary>
        /// Current Start Date
        /// </summary>
        public DateTime ScheduleDate
        {
            get { return _scheduleDate; }
        }

        public string StartTime
        {
            get { return _startTime; }
        }

        // dch rkl 10/12/2016 formatted start time
        public string StartTimeFormatted
        {
            get { return FormattedTime(_startTime); }
        }

        public string EndTime
        {
            get { return _endTime; }
        }

        // dch rkl 10/12/2016 formatted start time
        public string EndTimeFormatted
        {
            get { return FormattedTime(_endTime); }
        }

        public string ActualStartTime
        {
            get { return _actualStartTime; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Location
        {
            get { return _location; }
        }

        public string Phone
        {
            get { return _phone; }
        }

        public string SchedDateStartTime
        {
            get
            {
                App_Settings appSettings = App.Database.GetApplicationSettings();

                string sTime = StartTime;
                if (sTime.Length == 4) { sTime = string.Format("{0}:{1}", sTime.Substring(0, 2), sTime.Substring(2, 2)); }
                else if (sTime.Length == 3) { sTime = string.Format("0{0}:{1}", sTime.Substring(0, 1), sTime.Substring(1, 2)); }

                if (int.Parse(sTime.Substring(0, 2)) >= 12)
                {
                    sTime += " PM";
                    // dch rkl 11/15/2016 consider 24 hour time
                    if (appSettings.TwentyFourHourTime == false)
                    {
                        int iHr = int.Parse(sTime.Substring(0,2));
                        if (iHr > 12)
                        {
                            iHr = iHr - 12;
                            if (iHr < 10) { sTime =  "0" + iHr.ToString() + sTime.Substring(2,6);  }
                            else { sTime = iHr.ToString() + sTime.Substring(2, 6); }
                        }
                    }
                }
                else { sTime += " AM"; }

                string eTime = _endTime;
                if (eTime.Length == 4) { eTime = string.Format("{0}:{1}", eTime.Substring(0, 2), eTime.Substring(2, 2)); }
                else if (eTime.Length == 3) { eTime = string.Format("0{0}:{1}", eTime.Substring(0, 1), eTime.Substring(1, 2)); }

                if (int.Parse(eTime.Substring(0, 2)) >= 12)
                {
                    eTime += " PM";
                    // dch rkl 11/15/2016 consider 24 hour time
                    if (appSettings.TwentyFourHourTime == false)
                    {
                        int iHr = int.Parse(eTime.Substring(0, 2));
                        if (iHr > 12)
                        {
                            iHr = iHr - 12;
                            if (iHr < 10) { eTime = "0" + iHr.ToString() + eTime.Substring(2, 6); }
                            else { eTime = iHr.ToString() + eTime.Substring(2, 6); }
                        }
                    }
                }
                else { eTime += " AM"; }

                string schdttim = string.Format("{0} - {1} - {2}", ScheduleDate.ToString("MM-dd-yyyy"), sTime, eTime);
                return schdttim;
            }
        }

       

        public string NameLocPhone
        {
            get
            {
                return string.Format("{0} - {1} - {2}", Name, Location, Phone);
            }
        }

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set { _isCurrent = value; }
        }

        public string CurrentImageFileName
        {
            get { return (IsCurrent ? "Resources/CheckMark.png" : null); }
        }

        /// <summary>
        /// Override of base ToString() method.
        /// </summary>
        /// <returns>String value of ServiceTicketNumber field</returns>
        public override string ToString()
        {
            return ServiceTicketNumber;
        }

        // dch rkl 10/12/2016 format the time
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

        public App_ScheduledAppointment(JT_TechnicianScheduleDetail scheduleDetail, SO_SalesOrderHeader salesOrderHeader)
        {
            _salesOrderNumber = salesOrderHeader.SalesOrderNo; //salesOrderHeader.SalesOrderNo;
            _workTicketNumber = scheduleDetail.WTNumber;
            _workTicketStep = scheduleDetail.WTStep;
            _scheduleDate = scheduleDetail.ScheduleDate;
            _startTime = scheduleDetail.StartTime;
            _actualStartTime = string.Empty;
            _isCurrent = scheduleDetail.IsCurrent;

            //get scheduled end time from number of hours scheduled
            string sTime = _startTime;
            if (sTime.Length == 4) { sTime = string.Format("{0}:{1}", sTime.Substring(0, 2), sTime.Substring(2, 2)); }
            else if (sTime.Length == 3) { sTime = string.Format("0{0}:{1}", sTime.Substring(0, 1), sTime.Substring(1, 2)); }
            //if (int.Parse(sTime.Substring(0, 2)) >= 12) { sTime += " PM"; }
            //else { sTime += " AM"; }
            TimeSpan EndTime = TimeSpan.Parse(sTime);
            EndTime = EndTime + TimeSpan.FromHours((double)scheduleDetail.HoursScheduled);
            _endTime = EndTime.ToString(@"hhmm");

            // Get ticket customer phone
            _phone = "";
            if (salesOrderHeader.CustomerNo != null && salesOrderHeader.CustomerNo.Length > 0)
            {
                AR_Customer cust = App.Database.GetCustomer(salesOrderHeader.CustomerNo);
                if (cust.TelephoneNo != null && cust.TelephoneNo.Length > 0)
                {
                    _phone = cust.TelephoneNo;
                    if (cust.TelephoneExt != null && cust.TelephoneExt.Length > 0)
                    {
                        _phone += " ext. " + cust.TelephoneExt;
                    }
                }
            }

            if ((salesOrderHeader.ShipToCode != null) && (salesOrderHeader.ShipToCode.Length > 0))
            {
                _name = salesOrderHeader.ConfirmTo;
                _location = salesOrderHeader.ShipToCity + ", " + salesOrderHeader.ShipToState;
            }
            else
            {
                _name = salesOrderHeader.BillToName;
                _location = salesOrderHeader.BillToCity + ", " + salesOrderHeader.BillToState;
            }
        }
    }
}
