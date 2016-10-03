using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_Technician
    {
        [PrimaryKeyAttribute, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// puke
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Department Nuamber - varchar(2)
        /// </summary>
        public string TechnicianDeptNo { get; set; }

        /// <summary>
        /// Technician Number - varchar(7)
        /// </summary>
        public string TechnicianNo { get; set; }

        /// <summary>
        /// Technician Last Name - varchar(15)
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Technician First Name - varchar(15)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Default Warehouse -  varchar(3)
        /// </summary>
        public string DefaultWarehouse { get; set; }

        /// <summary>
        /// Current Status - varchar(6)
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// Text Messaging Address - varchar(80)
        /// </summary>
        public string TextMessagingAddress { get; set; }

        /// <summary>
        /// Mobile Phone Number - varchar(17)
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Current Start Time - varchar(4)
        /// </summary>
        public string CurrentStartTime { get; set; } // puke - separate date and time how?

        /// <summary>
        /// Current Start Date - date
        /// </summary>
        public DateTime CurrentStartDate { get; set; }

        /// <summary>
        /// Current Sales Order Number - varchar(7)
        /// </summary>
        public string CurrentSalesOrderNo { get; set; }

        /// <summary>
        /// Current Service Ticket Number - varchar(3)
        /// </summary>
        public string CurrentWTNumber { get; set; }

        /// <summary>
        /// Current Service Ticket Step - varchar(3)
        /// </summary>
        public string CurrentWTStep { get; set; }

        /// <summary>
        /// Gets the formatted technician number.
        /// </summary>
        public string FormattedTechnicianNo
        {
            get
            {
                return TechnicianDeptNo + "-" + TechnicianNo;
            }
        }
    }
}
