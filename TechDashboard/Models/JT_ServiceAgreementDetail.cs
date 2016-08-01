using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_ServiceAgreementDetail
    {
        /// <summary>
        /// Division Number - varchar(2)
        /// </summary>
        public string ARDivisionNo { get; set; }

        /// <summary>
        /// Customer Number - varchar(20)
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// Contract Code - varchar(10)
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Item Code - varchar(30)
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Serial Number - varchar(20)
        /// </summary>
        public string MfgSerialNo { get; set; }

        /// <summary>
        /// Service Agreement Start Date - date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Service Agreement End Date - date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Labor Covered Y/N - varchar(1)
        /// </summary>
        public string LaborCovered { get; set; }

        /// <summary>
        /// Parts Covered Y/N - varchar(1)
        /// </summary>
        public string PartsCovered { get; set; }

        /// <summary>
        /// Billing Type - varchar(1)
        /// </summary>
        public string BillingType { get; set; }

        /// <summary>
        /// Rate per Hour - numeric(15, 6)
        /// </summary>
        public decimal Rate { get; set; }
    }
}
