using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace TechDashboard.Models
{
    public class AR_Customer
    {
        [PrimaryKeyAttribute, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Division Number - varchar(2)
        /// </summary>
        public string ARDivisionNo { get; set; }

        /// <summary>
        /// Customer Number - varchar(20)
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// Customer Name - varchar(30)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Address Line 1 - varchar(30)
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Address Line 2 - varchar(30)
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Address Line 3 - varchar(30)
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// City - varchar(20)
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State - varchar(2)
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Zip Code - varchar(10)
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Telephone Number - varchar(17)
        /// </summary>
        public string TelephoneNo { get; set; }

        /// <summary>
        /// Telephone Extension - varchar(5)
        /// </summary>
        public string TelephoneExt { get; set; }

        /// <summary>
        ///  Contact Code = varchar
        /// </summary>
        public string ContactCode { get; set; }

        public string FormattedCustomerNo
        {
            get { return ARDivisionNo + "-" + CustomerNo; }
        }
    }
}
