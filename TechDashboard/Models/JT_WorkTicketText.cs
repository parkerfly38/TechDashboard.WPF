using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * JT_WorkTicketText.cs
     * 11/30/2016 DCH Add TODO
     *********************************************************************************************************/

    public class JT_WorkTicketText
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsModified { get; set; }

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
        /// Text - varchar(100)
        /// </summary>
        public string Text { get; set; }
    }
}
