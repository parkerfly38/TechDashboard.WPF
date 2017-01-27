using System;
using System.Collections.Generic;
using System.Text;

using TechDashboard.Data;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_Technician.cs
     * 12/02/2016 DCH Add TODO, Remove unused code
     *********************************************************************************************************/
    public class App_Technician
    {
        protected string _technicianDeptNo;
        protected string _technicianNo;
        protected string _lastName;
        protected string _firstName;
        protected string _defaultWarehouse;
        protected string _currentStatus;
        protected string _textMessagingAddress;
        protected string _mobilePhoneNumber;

        public string TechnicianDeptNo
        {
            get { return _technicianDeptNo; }
        }

        public string TechnicianNo
        {
            get { return _technicianNo; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string DefaultWarehouse
        {
            get { return _defaultWarehouse; }
        }

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }

        public string TextMessagingAddress
        {
            get { return _textMessagingAddress; }
        }

        public string MobilePhoneNumber
        {
            get { return _mobilePhoneNumber; }
        }

        /// <summary>
        /// Gets the formatted technician number.
        /// </summary>
        public string FormattedTechnicianNumber
        {
            get
            {
                return _technicianDeptNo + "-" + _technicianNo;
            }
        }

        public App_Technician(JT_Technician technician)
        {
            try
            {
                _technicianDeptNo = technician.TechnicianDeptNo;
                _technicianNo = technician.TechnicianNo;
                _lastName = technician.LastName;
                _firstName = technician.FirstName;
                _defaultWarehouse = technician.DefaultWarehouse;
                _currentStatus = technician.CurrentStatus;
                _textMessagingAddress = technician.TextMessagingAddress;
                _mobilePhoneNumber = technician.MobilePhoneNumber;
            }
            catch
            {
                // TODO
                // throw some sort of error
            }
        }

        /// <summary>
        /// Breaks apart (or unformats) the formatted ticket number provided.
        /// </summary>
        /// <param name="formattedTechnicianNumber">Formatted technician number</param>
        /// <returns>String array with the following indexes:
        /// 0 = Technician Department Number,
        /// 1 = Technician Number</returns>
        public static string[] BreakFormattedTechnicianNumber(string formattedTechnicianNumber)
        {
            string[] returnData = null;

            try
            {
                returnData = formattedTechnicianNumber.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                // we expect to get back an array with two elements.
                if ((returnData == null) || (returnData.Length != 2))
                {
                    returnData = null;
                }
            }
            catch
            {
                // empty
            }

            return returnData;
        }
    }
}
