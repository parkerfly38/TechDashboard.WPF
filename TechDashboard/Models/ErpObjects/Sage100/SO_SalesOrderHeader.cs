using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class SO_SalesOrderHeader
    {
        /// <summary>
        /// Customer Number - varchar(20)
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// Sales Order Number varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Ship To Code - varchar(4)
        /// </summary>
        public string ShipToCode { get; set; }

        /// <summary>
        /// Ship To Name - varchar(30)
        /// </summary>
        public string ShipToName { get; set; }

        /// <summary>
        /// Ship To Address 1 - varchar(30)
        /// </summary>
        public string ShipToAddress1 { get; set; }

        /// <summary>
        /// Ship To Address 2 - varchar(30)
        /// </summary>
        public string ShipToAddress2 { get; set; }

        /// <summary>
        /// Ship To Address 3 - varchar(30)
        /// </summary>
        public string ShipToAddress3 { get; set; }

        /// <summary>
        /// Ship To City - varchar(20)
        /// </summary>
        public string ShipToCity { get; set; }

        /// <summary>
        /// Ship To State - varchar(2)
        /// </summary>
        public string ShipToState { get; set; }

        /// <summary>
        /// Ship To Zip Code - varchar(10)
        /// </summary>
        public string ShipToZipCode { get; set; }

        /// <summary>
        /// Bill To Name - varchar(30)
        /// </summary>
        public string BillToName { get; set; }

        /// <summary>
        /// Bill To Address 1 - varchar(30)
        /// </summary>
        public string BillToAddress1 { get; set; }

        /// <summary>
        ///  Bill To Address 2 - varchar(30)
        /// </summary>
        public string BillToAddress2 { get; set; }

        /// <summary>
        /// Bill To Address 3 - varchar(30)
        /// </summary>
        public string BillToAddress3 { get; set; }

        /// <summary>
        /// Bill To City - varchar(20)
        /// </summary>
        public string BillToCity { get; set; }

        /// <summary>
        /// Bill To STate - varchar(2)
        /// </summary>
        public string BillToState { get; set; }

        /// <summary>
        /// Bill To Zip Code - varchar(10)
        /// </summary>
        public string BillToZipCode { get; set; }

        /// <summary>
        /// Confirm To - varchar(30)
        /// </summary>
        public string ConfirmTo { get; set; }
    }
}
