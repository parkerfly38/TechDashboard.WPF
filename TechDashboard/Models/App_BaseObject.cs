using System;
using System.Collections.Generic;
using System.Text;

using TechDashboard.Data;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_BaseObject.cs
     * 12/01/2016 DCH Add TODO
     *********************************************************************************************************/
    public class App_BaseObject
    {
        protected TechDashboardDatabase _database;

        public App_BaseObject(TechDashboardDatabase database)
        {
            _database = database;
            // TODO... at some point, we should create an abstrated data access layer object/interface
        }
    }
}
