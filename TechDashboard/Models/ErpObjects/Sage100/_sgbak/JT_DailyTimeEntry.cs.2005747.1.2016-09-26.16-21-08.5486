using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_DailyTimeEntry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// puke
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Department Number - varchar(2)
        /// </summary>
        public string DepartmentNo { get; set; }

        /// <summary>
        /// Employee Number - varchar(7)
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Service Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Service Ticket Step - varchar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Transaction Date - date
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Start Time - varchar(4)
        /// </summary>
        public string StartTime { get; set; }  // puke - separate date/time?

        /// <summary>
        /// End Time - varchar(4)
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Earnings Code - varchar
        /// </summary>
        public string EarningsCode { get; set; }
    }
}
