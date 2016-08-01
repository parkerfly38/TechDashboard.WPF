using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
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
            get { return _startTime; } // puke... make DateTime?
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
                string sTime = StartTime;
                if (sTime.Length == 4) { sTime = string.Format("{0}:{1}", sTime.Substring(0, 2), sTime.Substring(2, 2)); }
                else if (sTime.Length == 3) { sTime = string.Format("0{0}:{1}", sTime.Substring(0, 1), sTime.Substring(1, 2)); }
                if (int.Parse(sTime.Substring(0,2)) >= 12) { sTime += " PM";  }
                else { sTime += " AM"; }
                string schdttim = string.Format("{0} - {1}", ScheduleDate.ToString("MM-dd-yyyy"), sTime);
                return schdttim;
            }
        }

        public string NameLocPhone
        {
            get
            {
                return string.Format("{0} - {1} - {2}",Name, Location, Phone);
            }
        }

        public bool IsCurrent
        {
            get { return _isCurrent; }
        }

        public string CurrentImageFileName
        {
            get { return (IsCurrent ? "CheckMark.png" : null); }
        }

        /// <summary>
        /// Override of base ToString() method.
        /// </summary>
        /// <returns>String value of ServiceTicketNumber field</returns>
        public override string ToString()
        {
            return ServiceTicketNumber;
        }

        public App_ScheduledAppointment(JT_TechnicianScheduleDetail scheduleDetail, SO_SalesOrderHeader salesOrderHeader)
        {
            // puke
            _salesOrderNumber = salesOrderHeader.SalesOrderNo;
            _workTicketNumber = scheduleDetail.WTNumber;
            _workTicketStep = scheduleDetail.WTStep;
            _scheduleDate = scheduleDetail.ScheduleDate;
            _startTime = scheduleDetail.StartTime;
            _actualStartTime = string.Empty;
            _isCurrent = scheduleDetail.IsCurrent;

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
