using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        /// <summary>
        /// Retreives Field Service Option data from the ERP connection and uses
        /// it to fill the local JT_FieldServiceOptions table.
        /// </summary>
        protected void FillFieldServiceOptionsTable()
        {
            FillLocalTable<JT_FieldServiceOptions>("where", "ModuleCode eq 'J/T'");
        }

        protected JT_FieldServiceOptions GetFieldServiceOption()
        {
            JT_FieldServiceOptions option = null;

            lock (_locker)
            {
                // there should only be one record
                option = _database.Table<JT_FieldServiceOptions>().FirstOrDefault();
            }

            return option;
        }

        public App_ErpOptions GetErpOptions()
        {
            App_ErpOptions erpOptions = null;

            lock(_locker)
            {
                JT_FieldServiceOptions fieldServiceOption = GetFieldServiceOption();
                JT_TechnicianStatus defaultArriveStatus = null;
                JT_TechnicianStatus defaultDepartStatus = null;
                JT_MiscellaneousCodes defaultServiceTicketArriveStatus = null;

                if(fieldServiceOption != null)
                {
                    if(fieldServiceOption.ArriveStatusCode != null)
                    {
                        defaultArriveStatus = GetTechnicianStatusFromDB(fieldServiceOption.ArriveStatusCode);
                    }
                    if(fieldServiceOption.DepartStatusCode != null)
                    {
                        defaultDepartStatus = GetTechnicianStatusFromDB(fieldServiceOption.DepartStatusCode);
                    }
                    if(fieldServiceOption.ServiceStartedStatusCode1 != null)
                    {
                        defaultServiceTicketArriveStatus = GetMiscellaneousCodeFromDB("ST", fieldServiceOption.ServiceStartedStatusCode1);
                    }

                    erpOptions = new App_ErpOptions(fieldServiceOption, defaultArriveStatus, defaultDepartStatus, defaultServiceTicketArriveStatus);
                }
            }

            return erpOptions;
        }

    }
}
