using System;
using System.Collections.Generic;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard
{
    public class App
    {
        protected static TechDashboardDatabase _database;
        protected static App_Technician _currentTechnician;
        protected static App_WorkTicket _currentWorkTicket;

        public static TechDashboardDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new TechDashboardDatabase();
                }
                return _database;
            }
        }

        public static App_Technician CurrentTechnician
        {
            get
            {
                if (_currentTechnician == null)
                {
                    _currentTechnician = new App_Technician(Database.GetCurrentTechnicianFromDb());
                }

                return _currentTechnician;
            }
        }

        public static App_WorkTicket CurrentWorkTicket
        {
            get
            {
                if (_currentWorkTicket == null)
                {
                    _currentWorkTicket = Database.GetCurrentWorkTicket();
                }
                return _currentWorkTicket;
            }
        }

    }
}
