using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SQLite;
using System.Linq;
using TechDashboard.Data;
using TechDashboard.Models;
using Sage.SData.Client;
using RestSharp;
using System.Reflection;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDatabase.cs
     * 12/05/2016 DCH Added Try/Catch to catch and display error
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        public bool HasDataConnection()
        {
            RestClient restClient = new RestClient(GetApplicationSettings());
            bool connection = restClient.TestConnectionSync();

            return connection;

            //try
            //{
            //    using (var client = new System.Net.WebClient())
            //    {
            //        using (var stream = client.OpenRead("http://www.google.com"))
            //        {
            //            return true;
            //        }
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public CI_Options GetCIOptions()
        {
            CI_Options ciOptions = new CI_Options();
            lock (_locker)
            {
                ciOptions = _database.Table<CI_Options>().FirstOrDefault();
            }
            return ciOptions;
        }

        public bool HasValidSetup()
        //public async Task<bool> HasValidSetup()
        {
            bool hasValidSetup = false;
            App_Settings appSettings = null;

            lock (_locker)
            {
                appSettings = GetApplicationSettings();
            }
            //get version
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string dbVersion = (appSettings.DbVersion != null) ? appSettings.DbVersion : "0.0.0.0";

            if(version.Substring(0,3) != dbVersion.Substring(0,3))
            {
                return false;
            }

            if (HasDataConnection())
            {
                try
                {
                    switch (_dataConnectionType)
                    {
                        case ConnectionType.Rest:
                            hasValidSetup =
                                IsValidRestServiceConnection(appSettings.IsUsingHttps, appSettings.RestServiceUrl);
                            //await IsValidRestServiceConnection(appSettings.IsUsingHttps, appSettings.RestServiceUrl);
                            break;
                        case ConnectionType.SData:
                            hasValidSetup =
                                IsValidSDataConnection(
                                    appSettings.IsUsingHttps,
                                    appSettings.SDataUrl,
                                    appSettings.SDataUserId,
                                    appSettings.SDataPassword
                                );
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    hasValidSetup = false;

                    System.Diagnostics.Debug.WriteLine("Exception caught in HasValidSetup() method.");
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    // dch rkl 12/07/2016 Log Error
                    ErrorReporting errorReporting = new ErrorReporting();
                    errorReporting.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.HasValidSetup");
                }
            } else
            {
                // it'll either have old data or it won't.
                hasValidSetup = true;
            }



            // validate device
            

            return hasValidSetup;
        }
        
        public App_Settings GetApplicationSettings()
        {
            // dch rkl 12/06/2016 Add Try/Catch to display exceptions
            App_Settings appSettings = new Models.App_Settings();
            //App_Settings appSettings = null;

            try
            {

                lock (_locker)
                {
                    // dch rkl 12/07/2016 This is not working.  The TableMappings count is always zero, so it keeps creating a new app settings record each time. BEGIN
                    //if (_database.TableMappings.Where(tm => tm.TableName == "App_Settings").FirstOrDefault() == null)
                    //{
                    //    _database.CreateTable<App_Settings>();
                    //    _database.Insert(new App_Settings());
                    //}
                    // dch rkl 12/07/2016 This is not working.  The TableMappings count is always zero, so it keeps creating a new app settings record each time. END

                    try
                    {
                        appSettings = _database.Table<App_Settings>().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        //System.Diagnostics.Debug.WriteLine("Exception caught when querying for application settings.");
                        //System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    // dch rkl 12/07/2016 If no app settings found, add a blank record now BEGIN
                    if (appSettings == null || appSettings.ID == 0)
                    {
                        try
                        {
                            _database.CreateTable<App_Settings>();
                            _database.Insert(new App_Settings());
                            appSettings = _database.Table<App_Settings>().FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            ErrorReporting oErrRpt = new Data.ErrorReporting();
                            // dch rkl 12/07/2016 add the call/sub/proc to log
                            //oErrRpt.sendException(ex);
                            oErrRpt.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.cs/GetApplicationSettings");
                        }
                    }
                    // dch rkl 12/07/2016 If no app settings found, add a blank record now END
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.GetApplicationSettings");

                throw new Exception(String.Format(
                    "Error occurred in TechDashboardDB_AppSettings.GetApplicationSettings.  Contact your administrator.  /nMessageDetails: {0}", 
                    ex.ToString()));
            }

            return appSettings;
        }

        protected bool AreSettingsAvailable()
        {
            bool isAvailable = false;

            lock (_locker)
            {
                try
                {
                    isAvailable = (GetApplicationSettings() != null);
                }
                catch (Exception ex)
                {
                    isAvailable = false;

                    System.Diagnostics.Debug.WriteLine("Could not retrieve Application Settings record.");
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    // dch rkl 12/07/2016 Log Error
                    ErrorReporting errorReporting = new ErrorReporting();
                    errorReporting.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.AreSettingsAvailable");

                }
            }

            return isAvailable;
        }

        protected bool IsValidSDataConnection(bool isUsingHttps, string sDataUrl, string userId, string password)
        {
            List<JT_Technician> technicians = null;

            lock (_locker)
            {
                System.Diagnostics.Debug.WriteLine("Testing sData connection by querying for technicians.");

                try
                {
                    SDataClient sdataClient = new SDataClient(isUsingHttps, sDataUrl, userId, password);

                    technicians = sdataClient.GetData<JT_Technician>(string.Empty, string.Empty);

                    System.Diagnostics.Debug.WriteLine("Test call to sData returned " + technicians.Count.ToString() + " technicians.");
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception was caught!");
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    // dch rkl 12/07/2016 Log Error
                    ErrorReporting errorReporting = new ErrorReporting();
                    errorReporting.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.IsValidSDataConnection");

                }
            }

            return ((technicians != null) && (technicians.Count > 0));
        }

        protected bool IsValidRestServiceConnection(bool isUsingHttps, string restUrl)
        //protected async Task<bool> IsValidRestServiceConnection(bool isUsingHttps, string restUrl)
        {
            bool bSuccess = false;
            //List<bool> result = null;

            System.Diagnostics.Debug.WriteLine("Testing REST service connection by querying for technicians.");

            if (isUsingHttps == null || restUrl == null)
                return false;

            try
            {
                RestClient restClient = new RestClient(isUsingHttps, restUrl);

                //result = await restClient.TestConnection();             
                bSuccess = restClient.TestConnection();

                //System.Diagnostics.Debug.WriteLine("Test call to REST service returned " + result.Count.ToString() + " results.");
                System.Diagnostics.Debug.WriteLine("Test call to REST service returned " + bSuccess.ToString() + " results.");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception was caught!");
                System.Diagnostics.Debug.WriteLine(ex.Message);

                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TechDashboardDB_AppSettings.IsValidRestServiceConnection");

            }

            return bSuccess;
            //return ((result.Count > 0) && (result[0]));
        }

        public string GetDeviceID(string DeviceName)
        {
            App_Settings appSettings = GetApplicationSettings();
            RestClient restClient = new RestClient(appSettings);
            return restClient.GetDeviceID(DeviceName);
        }

        public void SaveAppSettings(App_Settings appSettings)
        {
            int rows = 0;

            lock(_locker)
            {
                if (appSettings.ID == 0)
                {
                    rows = _database.Insert(appSettings);
                    System.Diagnostics.Debug.WriteLine("Rows inserted = " + rows.ToString());
                }
                else
                {
                    rows = _database.Update(appSettings);
                    System.Diagnostics.Debug.WriteLine("Rows updated = " + rows.ToString());
                }
            }
        }
    }
}
