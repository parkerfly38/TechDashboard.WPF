using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Data
{
    public class ErrorReporting
    {

        public ErrorReporting() { }

        // dch rkl 12/07/2016 add the call/sub/proc to log
        //public void sendException(Exception exception)
        public void sendException(Exception exception, string thread)
        {
            // dch rkl 12/07/2016 only log error if there is a connection
            // TODO - may want to log errors locally, and send over in Sync when connected
            TechDashboardDatabase oDB = new TechDashboardDatabase();
            bool bHasConnection = oDB.HasDataConnection();

            if (bHasConnection)
            {
                BusinessLogic.ApplicationLog appLog = new BusinessLogic.ApplicationLog();
                appLog.log_date = DateTime.Now.ToLongTimeString();
                appLog.log_level = "Error";
                appLog.log_logger = "REST SERVICE";
                appLog.log_message = exception.Message;
                appLog.log_machine_name = System.Environment.MachineName;
                appLog.log_user_name = "WPF TechDashboard";
                appLog.log_call_site = App.Database.GetApplicationSettings().SDataUrl;     // we're going to use their SDATA root

                // dch rkl 12/07/2016 add the call/sub/proc to log
                //appLog.log_thread = "";
                appLog.log_thread = thread;

                appLog.log_stacktrace = exception.StackTrace;

                // dch rkl 12/07/2016 change to new API
                //var client = new RestSharp.RestClient("http://50.200.65.158/tdwsnew/tdwsnew.svc/i/ApplicationLog"); //hard coded to get it back to us
                var client = new RestSharp.RestClient("http://50.200.65.158/techdashboardapi/v1-6/i/ApplicationLog"); //hard coded to get it back to us

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
}
