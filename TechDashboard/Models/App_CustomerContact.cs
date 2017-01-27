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
        private string _telephoneNo1;
        private string _telephoneExt1;
        private string _telephoneNo2;
        private string _telephoneExt2;

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
        public string TelephoneNo1
        {
            get { return _telephoneNo1; }
        }

        public string TelephoneExt1
        {
            get { return _telephoneExt1; }
        }

        public string TelephoneNo2
        {
            get { return _telephoneNo2; }
        }

        public string TelephoneExt2
        {
            get { return _telephoneExt2; }
        }

        public App_CustomerContact(AR_CustomerContact contact)
        {
            // dch rkl 10/31/2016 Make Sure Contact is not Null
            if (contact != null)
            {
                _arDivisionNo = contact.ARDivisionNo;
                _customerNo = contact.CustomerNo;
                _contactCode = contact.ContactCode;
                _contactName = contact.ContactName;
                _telephoneNo1 = contact.TelephoneNo1;
                _telephoneExt1 = contact.TelephoneExt1;
                _telephoneNo2 = contact.TelephoneNo2;
                _telephoneExt2 = contact.TelephoneExt2;
            }
            else
            {
                _arDivisionNo = "";
                _customerNo = "";
                _contactCode = "";
                _contactName = "";
                _telephoneNo1 = "";
                _telephoneExt1 = "";
                _telephoneNo2 = "";
                _telephoneExt2 = "";
            }
        }
    }
}
