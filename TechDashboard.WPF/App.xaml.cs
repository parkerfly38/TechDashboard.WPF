using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected static TechDashboardDatabase _database;

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

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
        protected async override void OnStartup(StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("/Resources/td_sq.png");
            splashScreen.Show(true);
            bool hasValidSetup = await Database.HasValidSetup();
            if (!hasValidSetup)
            {
                // now we also need to verify that it's not just missing a data connection
                // in which case we'll avoid this startup ui, because we're not going to need/want to destroy our global tables
                if (Database.HasDataConnection())
                {
                    this.StartupUri = new Uri("/SettingsPage.xaml", UriKind.Relative);
                }
            }
            else {
                string loggedintechnicianno = (Database.GetApplicatioinSettings().LoggedInTechnicianNo != null) ? Database.GetApplicatioinSettings().LoggedInTechnicianNo : "";

                if (loggedintechnicianno.Length <= 0 && Database.HasDataConnection())
                    Database.CreateGlobalTables();
            }
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            sendException(exception);
        }

        public static void sendException(Exception exception)
        {
            BusinessLogic.ApplicationLog appLog = new BusinessLogic.ApplicationLog();
            appLog.log_date = DateTime.Now.ToLongTimeString();
            appLog.log_level = "Error";
            appLog.log_logger = "REST SERVICE";
            appLog.log_message = exception.Message;
            appLog.log_machine_name = System.Environment.MachineName;
            appLog.log_user_name = "WPF TechDashboard";
            appLog.log_call_site = App.Database.GetApplicatioinSettings().SDataUrl;     // we're going to use their SDATA root
            appLog.log_thread = "";
            appLog.log_stacktrace = exception.StackTrace;

            var client = new RestSharp.RestClient("http://50.200.65.158/tdwsnew/tdwsnew.svc/i/ApplicationLog"); //hard coded to get it back to us
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(appLog);

            try
            {
                var response = client.Execute(request) as RestSharp.RestResponse;
            }
            catch (Exception restException)
            {
                // can't do much
            }
        }
    }
    
}
