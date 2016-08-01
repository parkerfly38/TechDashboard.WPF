using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_ActivityCode
    {
        /// <summary>
        /// Activity Code - varchar(4)
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// Department Worked In - varchar(2)
        /// </summary>
        public string DeptWorkedIn { get; set; }

        /// <summary>
        /// Activity Description - varchar(30)
        /// </summary>
        public string ActivityDescription { get; set; }

        /// <summary>
        /// Default Earning Code - varchar(2)
        /// </summary>
        public string DefaultEarningCode { get; set; }

        /// <summary>
        /// Miscellaneous Billing Code - varchar(30)
        /// </summary>
        public string BillingMiscCode { get; set; }

        /// <summary>
        /// Billing Rate Multiplier - numeric(15, 6)
        /// </summary>
        public decimal BillingRateMultiplier { get; set; }
    }
}
