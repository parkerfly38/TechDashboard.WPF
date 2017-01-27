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
        #region App_Technician

        /// <summary>
        /// Retreives the App_Technician record marked as the current technician.
        /// </summary>
        /// <returns>The current App_Technician object.</returns>
        public App_Technician RetrieveCurrentTechnician()
        {
            App_Technician techToReturn = null;

            lock (_locker)
            {
                JT_Technician currentTech = _database.Table<JT_Technician>().Where(t => t.IsCurrent == true).FirstOrDefault();
                if (currentTech != null)
                {
                    techToReturn = new App_Technician(currentTech);
                }
            }

            return techToReturn;
        }

        /// <summary>
        /// Retreives all technicians from the local database table.
        /// </summary>
        /// <returns>A List collection of App_Technician objects.</returns>
        public List<App_Technician> GetTechniciansFromDB()
        {
            List<App_Technician> listOfTechnicians = new List<App_Technician>();
            lock (_locker)
            {
                foreach (JT_Technician technician in _database.Table<JT_Technician>().OrderBy(t => t.LastName).ToList())
                {
                    listOfTechnicians.Add(new App_Technician(technician));
                }
            }

            return listOfTechnicians;
        }

        /// <summary>
        /// Marks the given JT_Technician record as being the currently-selected
        /// technician by setting the IsCurrent flag on the local database record.
        /// </summary>
        /// <param name="technician">The JT_Technician object/record to mark as current.</param>
        public void SaveTechnicianAsCurrent(App_Technician technician)
        {
            int rows = 0;

            lock (_locker)
            {
                // Unset any techs that are marked as "current"
                List<JT_Technician> currentTechnicians = _database.Table<JT_Technician>().Where(t => t.IsCurrent == true).ToList();
                if (currentTechnicians.Count > 0)
                {
                    foreach (JT_Technician technicianInList in currentTechnicians)
                    {
                        technicianInList.IsCurrent = false;
                    }
                    _database.UpdateAll(currentTechnicians);
                }

                // find the corresponding JT_Technician record 
                JT_Technician technicianToUse =
                    _database.Table<JT_Technician>().Where(
                        t => (t.TechnicianDeptNo == technician.TechnicianDeptNo) &&
                             (t.TechnicianNo == technician.TechnicianNo)
                    ).FirstOrDefault();

                // Set the tech as current.
                technicianToUse.IsCurrent = true;
                rows = _database.Update(technicianToUse);
            }

            if (rows > 0)
            {
                OnCurrentTechnicianChanged(EventArgs.Empty);
            }
        }

        public void UpdateTechnicianStatus(App_Technician technician, string newStatusCode)
        {
            JT_TechnicianStatus newStatus;

            lock (_locker)
            {
                // First, be sure this status code is valid
                newStatus =
                    _database.Table<JT_TechnicianStatus>().Where(
                        ts => (ts.StatusCode == newStatusCode)
                    ).FirstOrDefault();

                if (newStatus != null)
                {
                    UpdateTechnicianStatus(technician, newStatus);
                }
            }
        }

        public async void UpdateTechnicianStatus(App_Technician technician, JT_TechnicianStatus newStatusCode)
        {
            int rows = 0;
            JT_Technician techToUpdate = null;

            lock (_locker)
            {
                if (newStatusCode != null)
                {
                    // Get the technician record to update
                    techToUpdate = GetTechnician(technician.TechnicianDeptNo, technician.TechnicianNo);

                    if (techToUpdate != null)
                    {
                        // Set the status and attempt the update
                        techToUpdate.CurrentStatus = newStatusCode.StatusCode;
                        rows = _database.Update(techToUpdate);

                        System.Diagnostics.Debug.WriteLine("Rows updated = " + rows.ToString());

                        if (rows > 0)
                        {
                            // update successful, so set app object, too.
                            technician.CurrentStatus = newStatusCode.StatusCode;
                        }
                    }
                }
            }

            // Now that the DB is updated, send same info back to HQ
            if (techToUpdate != null)
            {
                try {
                    bool result = await UpdateErpTechnicianStatus(techToUpdate);
                }
                catch (Exception exception)
                {
                    // this may fail absent a data connection, could return false
                }
            }        
        }

        #endregion

        #region Technician

        /// <summary>
        /// Retreives Technician data from the ERP connection and uses
        /// it to fill the local JT_Technician table.
        /// </summary>
        public void FillTechnicianTable()
        {
            int rows = 0;

            List<JT_Technician> technicians = GetErpData<JT_Technician>(string.Empty, string.Empty); 

            lock (_locker)
            {
                _database.DeleteAll<JT_Technician>();

				if (technicians.Any() && technicians[0].TechnicianNo == null) {
					technicians.RemoveAt(0);
				}

                rows = _database.InsertAll(technicians);
            }

            System.Diagnostics.Debug.WriteLine("Rows added = " + rows.ToString());
        }

        /// <summary>
        /// Gets a technician record from the database's JT_Technician table based
        /// upon the department number and technician number provided.
        /// </summary>
        /// <param name="technicianDeptNo">The technician's department number</param>
        /// <param name="technicianNumber">The technician's employee number</param>
        /// <returns>A JT_Technician object for the technician specified in the parameters' data.</returns>
        public JT_Technician GetTechnician(string technicianDeptNo, string technicianNumber)
        {
            JT_Technician technician;
            //can't validate if table doesn't exist so let's do that first
            if (!TableExists<JT_Technician>())
                return null;
            lock (_locker)
            {
                technician = _database.Table<JT_Technician>().Where(
                    t => (t.TechnicianNo == technicianNumber) &&
                         (t.TechnicianDeptNo == technicianDeptNo)
                    ).FirstOrDefault();
            }

            return technician;
        }



        /// <summary>
        /// Marks the given JT_Technician record as being the currently-selected
        /// technician by setting the IsCurrent flag on the local database record.
        /// </summary>
        /// <param name="technician">The JT_Technician object/record to mark as current.</param>
        public void SaveTechnicianAsCurrent(JT_Technician technician)
        {
            // dch rkl 12/08/2016 add try/catch to capture exception

            try
            {
                int rows = 0;

                lock (_locker)
                {
                    // Unset any techs that are marked as "current"
                    List<JT_Technician> currentTechnicians = _database.Table<JT_Technician>().Where(t => t.IsCurrent == true).ToList();
                    if (currentTechnicians.Count > 0)
                    {
                        foreach (JT_Technician technicianInList in currentTechnicians)
                        {
                            technicianInList.IsCurrent = false;
                        }
                        _database.UpdateAll(currentTechnicians);
                    }

                    // Set this tech as current.
                    technician.IsCurrent = true;
                    rows = _database.Update(technician);
                }

                if (rows > 0)
                {
                    OnCurrentTechnicianChanged(EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ErrorReporting oErrRpt = new Data.ErrorReporting();
                oErrRpt.sendException(ex, "TechDashboard.Data.TechDashboardDB_Technician.cs/SaveTechnicianAsCurrent");
            }
        }

        /// <summary>
        /// Retreives the JT_Technician record marked as the current technician.
        /// </summary>
        /// <returns>The current JT_Technician object.</returns>
        public JT_Technician GetCurrentTechnicianFromDb()
        {// ... make not-public at some point.
            lock (_locker)
            {
                return _database.Table<JT_Technician>().Where(t => t.IsCurrent == true).FirstOrDefault();
            }
        }

        #endregion

        #region TechnicianStatus

        /// <summary>
        /// Retreives Technician Status data from the ERP connection and uses
        /// it to fill the local JT_TechnicianStatus table.
        /// </summary>
        public void FillTechnicianStatusTable()
        {
            FillLocalTable<JT_TechnicianStatus>(string.Empty, string.Empty);
        }

        /// <summary>
        /// Retreives all technician statuses from the local database table.
        /// </summary>
        /// <returns>A List collection of JT_TechnicianStatus objects.</returns>
        public List<JT_TechnicianStatus> GetTechnicianStatusesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_TechnicianStatus>().OrderBy(ts => ts.StatusCode).ToList();
            }
        }

        public JT_TechnicianStatus GetTechnicianStatusFromDB(string statusCode)
        {
            lock (_locker)
            {
                return _database.Table<JT_TechnicianStatus>().Where(ts => ts.StatusCode == statusCode).FirstOrDefault();
            }
        }

        #endregion

        public List<string> GetTechnicianWarehouses()
        {
            List<string> warehouseList =
                _database.Table<JT_Technician>().GroupBy(t => t.DefaultWarehouse).Select(group => group.First().DefaultWarehouse).ToList();

            return warehouseList;
        }
    }
}
