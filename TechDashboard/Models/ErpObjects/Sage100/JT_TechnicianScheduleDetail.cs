using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_TechnicianScheduleDetail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// puke
        /// </summary>
        [Ignore]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Department Number - varchar(2)
        /// </summary>
        public string TechnicianDeptNo { get; set; }

        /// <summary>
        /// Technician Number - varchar(7)
        /// </summary>
        public string TechnicianNo { get; set; }

        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Work Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Work Ticket Step Number - varchar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Current Start Date - date
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// Current Start Time - varchar(4)
        /// </summary>
        public string StartTime { get; set; } // puke... how separate date/time?

        /// <summary>
        /// Hours Scheduled - numeric(15, 6)
        /// </summary>
        public decimal HoursScheduled { get; set; }

        /// <summary>
        /// Service ticket number.
        /// Created by concatenating the sales order number, work ticket numer, and work ticket step
        /// </summary>
        [Ignore]
        public string ServiceTicketNumber
        {
            get
            {
                return (SalesOrderNo + "-" + WTNumber + "-" + WTStep);
            }
        }            
    }
}
