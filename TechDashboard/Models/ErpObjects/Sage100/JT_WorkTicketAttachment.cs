using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_WorkTicketAttachment
    {
        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Work Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Work Ticket Step - varchar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Sequence Number - varchar(3)
        /// </summary>
        public string SequenceNo { get; set; }

        /// <summary>
        /// Category Code - varchar(6)
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Description - varchar(60)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// File Name - varhar(200)
        /// </summary>
        public string FileName { get; set; }
    }
}
