using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using TechDashboard.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TechDashboard.Data
{
    public class RestClient : TdWs.RestClient.TdWs_RestClient
    {
        public RestClient(bool isUsingHttps, string restUrl) : base(isUsingHttps, restUrl)
        {
            // empty
        }

        public RestClient(TechDashboardDatabase database) : base()
        {
            App_Settings appSettings = database.GetApplicationSettings();

            if (appSettings != null)
            {
                IsUsingHttps = appSettings.IsUsingHttps;
                RestUrl = appSettings.RestServiceUrl;
            }
        }

        public RestClient(App_Settings appSettings) : base()
        {
            if (appSettings != null)
            {
                IsUsingHttps = appSettings.IsUsingHttps;
                RestUrl = appSettings.RestServiceUrl;
            }
        }

        public bool ValidateDeviceID(string deviceId)
        {
            return false;
        }

        public string GetDeviceID(string deviceName)
        {
            string url = GetRestServiceUrl();
            if (!url.EndsWith(@"/"))
            {
                url += @"/";
            }
            url += "GetDeviceID/" + deviceName;
            RestSharp.RestClient client = new RestSharp.RestClient(url);

            var request = new RestSharp.RestRequest(Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new Exception("Bad request");
                ErrorReporting errorReporting = new ErrorReporting();

                // dch rkl 12/07/2016 add the call/sub/proc to log
                //errorReporting.sendException(new Exception(response.Content));
                errorReporting.sendException(new Exception(response.Content), "RestClient.cs.GetDeviceID");
            }

            string JsonResult = response.Content;
            string returnData = JsonConvert.DeserializeObject<string>(JsonResult);

            return returnData;
        }

        public bool TestConnectionSync()
        {
            // Set up our return data object -- a list of typed objects.
            List<bool> returnData = new List<bool>();

            // set up the proper URL
            string url = GetRestServiceUrl();
            if (!url.EndsWith(@"/"))
            {
                url += @"/";
            }
            url += @"test";

            // Create a HTTP client to call the REST service
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response;
            // Make the call and get a valid response
            try {
                response = client.GetAsync(client.BaseAddress).Result;
            }
            catch (Exception ex)
            {
                return false;
            }
            response.EnsureSuccessStatusCode();

            // Read out the result... it better be JSON!
            string JsonResult = response.Content.ReadAsStringAsync().Result;
            returnData = JsonConvert.DeserializeObject<List<bool>>(JsonResult);

            client.Dispose();
            response.Dispose();

            return returnData[0];
        }

        public List<T> GetDataSync<T>(string filterType, string filterText)
        {
            // Set up our return data object -- a list of typed objects.
            List<T> returnData = new List<T>();

            // dch rkl 12/07/2016 capture errors with try/catch
            try
            {
                App_Settings appSettings = App.Database.GetApplicationSettings();

                // set up the proper URL
                string url = GetRestServiceUrl();
                if (!url.EndsWith(@"/"))
                {
                    url += @"/";
                }
                if ((filterType != null) &&
                    (filterType.Length > 0) &&
                    (filterText != null) &&
                    (filterText.Length > 0))
                {
                    url += @"q/" + typeof(T).Name + @"/" + filterType + @"?v=" + Uri.EscapeDataString(filterText);
                }
                else
                {
                    url += @"all/" + typeof(T).Name;
                }

                // Create a HTTP client to call the REST service
                RestSharp.RestClient client = new RestSharp.RestClient(url);

                var request = new RestSharp.RestRequest(Method.GET);
                if (appSettings.DeviceID != null)
                {
                    SimpleAES encryptText = new SimpleAES("V&WWJ3d39brdR5yUh5(JQGHbi:FB@$^@", "W4aRWS!D$kgD8Xz@");
                    string authid = encryptText.EncryptToString(appSettings.DeviceID);
                    string datetimever = encryptText.EncryptToString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    request.AddHeader("x-tdws-authid", authid);
                    request.AddHeader("x-tdws-auth", datetimever);
                }

                var response = client.Execute(request);

                // dch rkl 12/09/2016 include not found
                //if (response.StatusCode != System.Net.HttpStatusCode.OK)
                if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    //throw new Exception("Bad request");
                    ErrorReporting errorReporting = new ErrorReporting();
                    // dch rkl 12/07/2016 add the call/sub/proc to log
                    //errorReporting.sendException(new Exception(response.Content));
                    errorReporting.sendException(new Exception(response.Content), "RestClient.cs.GetDataSync");
                }

                string JsonResult = response.Content;
                returnData = JsonConvert.DeserializeObject<List<T>>(JsonResult);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, string.Format("TechDashboard.RestClient.GetDataSync: FilterType={0}; FilterText={1}",
                    filterType, filterText));

            }

            return returnData;
        }

        public List<T> GetDataPostSync<T>(string filterType, string filterText)
        {

            App_Settings appSettings = App.Database.GetApplicationSettings();
            // Set up our return data object -- a list of typed objects.
            List<T> returnData = new List<T>();

            // set up the proper URL
            string url = GetRestServiceUrl();
            if (!url.EndsWith(@"/"))
            {
                url += @"/";
            }
            if ((filterType != null) &&
                (filterType.Length > 0) &&
                (filterText != null) &&
                (filterText.Length > 0))
            {
                url += @"q/" + typeof(T).Name + @"/" + filterType;// + @"?v=" + Uri.EscapeDataString(filterText);
            }
            else
            {
                url += @"all/" + typeof(T).Name;
            }

            // Create a HTTP client to call the REST service
            RestSharp.RestClient client = new RestSharp.RestClient(url);

            var request = new RestSharp.RestRequest(Method.POST);
            if (appSettings.DeviceID != null)
            {
                SimpleAES encryptText = new SimpleAES("V&WWJ3d39brdR5yUh5(JQGHbi:FB@$^@", "W4aRWS!D$kgD8Xz@");
                string authid = encryptText.EncryptToString(appSettings.DeviceID);
                string datetimever = encryptText.EncryptToString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                request.AddHeader("x-tdws-authid", authid);
                request.AddHeader("x-tdws-auth", datetimever);
            }
            request.RequestFormat = DataFormat.Json;
            request.AddBody(filterText);
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new Exception("Bad request");
                ErrorReporting errorReporting = new ErrorReporting();
                // dch rkl 12/07/2016 add the call/sub/proc to log
                //errorReporting.sendException(new Exception(response.Content));
                errorReporting.sendException(new Exception(response.Content), "RestClient.cs.GetDataPostSync");
            }

            string JsonResult = response.Content;
            returnData = JsonConvert.DeserializeObject<List<T>>(JsonResult);

            return returnData;
        }

        public bool UpdateTechnicianRecordSync(JT_Technician technician)
        {
            bool returnData = false;

            App_Settings appSettings = App.Database.GetApplicationSettings();

            // set up the proper URL
            string url = GetRestServiceUrl();
            if (!url.EndsWith(@"/"))
            {
                url += @"/";
            }
            url += @"u/JT_Technician";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings();
            microsoftDateFormatSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            if (appSettings.DeviceID != null)
            {
                SimpleAES encryptText = new SimpleAES("V&WWJ3d39brdR5yUh5(JQGHbi:FB@$^@", "W4aRWS!D$kgD8Xz@");
                string authid = encryptText.EncryptToString(appSettings.DeviceID);
                string datetimever = encryptText.EncryptToString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                client.DefaultRequestHeaders.Add("x-tdws-authid", authid);
                client.DefaultRequestHeaders.Add("x-tdws-auth", datetimever);
            }
            //client.AddHeader("x-tdws-authid", authid);
            //request.AddHeader("x-tdws-auth", datetimever);

            // Make the call and get a valid response
            HttpResponseMessage response = client.PutAsync(client.BaseAddress, new StringContent(JsonConvert.SerializeObject(technician, microsoftDateFormatSettings), null, "application/json")).Result; // TODO.... await
            response.EnsureSuccessStatusCode();

            // Read out the result... it better be JSON!
            string JsonResult = response.Content.ReadAsStringAsync().Result;
            try
            {
                returnData = JsonConvert.DeserializeObject<bool>(JsonResult);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.Data.UpdatTechnicianRecordSync");
            }

            client.Dispose();
            response.Dispose();

            return returnData;
        }

        // dch rkl 12/09/2016 This now returns an object
        //public bool InsertTransactionImportDetailRecordSync(JT_TransactionImportDetail importDetail)
        public Rkl.Erp.Sage.Sage100.TableObjects.API_Results InsertTransactionImportDetailRecordSync(JT_TransactionImportDetail importDetail)
        {
            // dch rkl 12/09/2016 This now returns an object
            //bool returnData = false;
            Rkl.Erp.Sage.Sage100.TableObjects.API_Results returnData = new Rkl.Erp.Sage.Sage100.TableObjects.API_Results();

            App_Settings appSettings = App.Database.GetApplicationSettings();

            // set up the proper URL
            string url = GetRestServiceUrl();
            if (!url.EndsWith(@"/"))
            {
                url += @"/";
            }
            url += @"i/JT_TransactionImportDetail";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings();
            microsoftDateFormatSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            if (appSettings.DeviceID != null)
            {
                SimpleAES encryptText = new SimpleAES("V&WWJ3d39brdR5yUh5(JQGHbi:FB@$^@", "W4aRWS!D$kgD8Xz@");
                string authid = encryptText.EncryptToString(appSettings.DeviceID);
                string datetimever = encryptText.EncryptToString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                client.DefaultRequestHeaders.Add("x-tdws-authid", authid);
                client.DefaultRequestHeaders.Add("x-tdws-auth", datetimever);
            }

            // Make the call and get a valid response
            HttpResponseMessage response
                = client.PostAsync(client.BaseAddress, new StringContent(JsonConvert.SerializeObject(importDetail, microsoftDateFormatSettings), null, "application/json")).Result; // TODO.... await
            response.EnsureSuccessStatusCode();

            // Read out the result... it better be JSON!
            string JsonResult = response.Content.ReadAsStringAsync().Result;
            try
            {
                // dch rkl 12/09/2016 This now returns an object
                //returnData = JsonConvert.DeserializeObject<bool>(JsonResult);
                returnData = JsonConvert.DeserializeObject<Rkl.Erp.Sage.Sage100.TableObjects.API_Results>(JsonResult);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.Data.RestClient");

            }

            client.Dispose();
            response.Dispose();

            return returnData;
        }
    }
}
