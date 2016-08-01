using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_CustomerBillingRates
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
        /// Activity Code - varchar(4)
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// Rate per Hour - numeric(15, 6)
        /// </summary>
        public decimal BillRatePerHour { get; set; }
    }
}
