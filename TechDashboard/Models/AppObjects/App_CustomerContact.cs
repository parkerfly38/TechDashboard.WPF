using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_CustomerContact
    {
        private string _arDivisionNo;
        private string _customerNo;
        private string _contactCode;
        private string _contactName;

        /// <summary>
        /// Division Number
        /// </summary>
        public string ARDivisionNo
        {
            get { return _arDivisionNo; }
        }

        /// <summary>
        /// Customer Number 
        /// </summary>
        public string CustomerNo
        {
            get { return _customerNo; }
        }

        /// <summary>
        /// Contact Code
        /// </summary>
        public string ContactCode
        {
            get { return _contactCode; }
        }

        /// <summary>
        /// Contact Name 
        /// </summary>
        public string ContactName
        {
            get { return _contactName; }
        }
        public App_CustomerContact(AR_CustomerContact contact)
        {
            _arDivisionNo = contact.ARDivisionNo;
            _customerNo = contact.CustomerNo;
            _contactCode = contact.ContactCode;
            _contactName = contact.ContactName;
        }
    }
}
