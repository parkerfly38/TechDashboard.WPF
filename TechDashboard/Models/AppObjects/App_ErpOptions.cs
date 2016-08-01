using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace TechDashboard.Models
{
    public class App_ErpOptions
    {
        private JT_FieldServiceOptions _fieldServiceOptions;
        private JT_TechnicianStatus _defaultTechnicianArriveStatus;
        private JT_TechnicianStatus _defaultTechnicianDepartStatus;
        private JT_MiscellaneousCodes _defaultServiceTicketArriveStatus;

        /// <summary>
        /// Default status code for technician arrival
        /// </summary>
        public string DefaultTechnicianArriveStatusCode
        {
            get
            {
                if((_fieldServiceOptions != null) && (_fieldServiceOptions.ArriveStatusCode != null))
                {
                    return _fieldServiceOptions.ArriveStatusCode;
                }

                return null;
            }
        }

        /// <summary>
        /// Default status description for technician arrival
        /// </summary>
        public string DefaultTechnicianArriveStatusCodeDescription
        {
            get
            {
                if ((_defaultTechnicianArriveStatus != null) && (_defaultTechnicianArriveStatus.StatusDescription != null))
                {
                    return _defaultTechnicianArriveStatus.StatusDescription;
                }

                return null;
            }
        }

        /// <summary>
        /// Default status code for technician departure
        /// </summary>
        public string DefaultTechnicianDepartStatusCode
        {
            get
            {
                if ((_fieldServiceOptions != null) && (_fieldServiceOptions.DepartStatusCode != null))
                {
                    return _fieldServiceOptions.DepartStatusCode;
                }

                return null;
            }
        }

        /// <summary>
        /// Default status description for technician departure
        /// </summary>
        public string DefaultTechnicianDepartStatusCodeDescription
        {
            get
            {
                if ((_defaultTechnicianDepartStatus != null) && (_defaultTechnicianDepartStatus.StatusDescription != null))
                {
                    return _defaultTechnicianDepartStatus.StatusDescription;
                }

                return null;
            }
        }

        /// <summary>
        /// Default service ticket status code for arrival
        /// </summary>
        public string DefaultServiceTicketArriveStatusCode
        {
            get
            {
                if ((_fieldServiceOptions != null) && (_fieldServiceOptions.ServiceStartedStatusCode1 != null))
                {
                    return _fieldServiceOptions.ServiceStartedStatusCode1;
                }

                return null;
            }
        }

        /// <summary>
        /// Default service ticket status description for arrival
        /// </summary>
        public string DefaultServiceTicketArriveStatusCodeDescription
        {
            get
            {
                if ((_defaultServiceTicketArriveStatus != null) && (_defaultServiceTicketArriveStatus.Description != null))
                {
                    return _defaultServiceTicketArriveStatus.Description;
                }

                return null;
            }
        }

        public App_ErpOptions(
            JT_FieldServiceOptions fieldServiceOptions, 
            JT_TechnicianStatus defaultArriveStatus,
            JT_TechnicianStatus defaultDepartStatus,
            JT_MiscellaneousCodes defaultServiceTicketArriveStatus)
        {
            _fieldServiceOptions = fieldServiceOptions;
            _defaultTechnicianArriveStatus = defaultArriveStatus;
            _defaultTechnicianDepartStatus = defaultDepartStatus;
            _defaultServiceTicketArriveStatus = defaultServiceTicketArriveStatus;

        }
    }
}
