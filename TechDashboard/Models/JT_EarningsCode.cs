using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_EarningsCode
    {
        /// <summary>
        /// Record Type - vachar(1)
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Earnings Code - varchar(2)
        /// </summary>
        public string EarningsCode { get; set; }

        /// <summary>
        /// Description - varchar(30)
        /// </summary>
        public string EarningsDeductionDesc { get; set; }

        /// <summary>
        /// Type of Earnings - varchar(1)
        /// </summary>
        public string TypeOfEarnings { get; set; }

        // dch rkl 01/20/2017
        public string EarningsCodeAndDesc
        {
            get
            {
                return EarningsCode + " - " + EarningsDeductionDesc;
            }
        }
    }
}
