using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Data
{
    public class ErrorReporting
    {

        public ErrorReporting() { }

        public void sendException(Exception exception)
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
