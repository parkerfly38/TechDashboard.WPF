using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_FSDCommunication
    {
        /// <summary>
        /// Sent From User - varchar(50)
        /// </summary>
        public string SentFrom { get; set; }

        /// <summary>
        /// Date Sent - date
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Time Sent - varchar(8)
        /// </summary>
        public DateTime TimeCreated { get; set; }

        /// <summary>
        /// Message text - varchar(2048)
        /// </summary>
        public string Comment { get; set; }

        // PUKE -- how to implement date/time created here?
    }
}
