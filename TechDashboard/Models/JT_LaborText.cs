using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * JT_LaborText.cs
     * 12/02/2016 DCH Add TODO
     *********************************************************************************************************/
    public class JT_LaborText
    {
        // TODO - does not exist in jobops2015dev database!

        /// <summary>
        /// Sales Order Number -
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Service Ticket Number -
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Service Ticket Step Number - 
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Record Type -
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Transaction Date - 
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Employee Number - 
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// Billing Text - 
        /// </summary>
        public string BillingText { get; set; }
    }
}
