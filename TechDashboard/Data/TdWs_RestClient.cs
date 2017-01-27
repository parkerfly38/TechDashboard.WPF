using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TdWs.RestClient
{
    /***************************************************************************
     * TdWs_RestClient.cs
     * Created By:  D. Hartman
     * Create Date: 12/07/2016
     * Description: Copied to old Web Service TdWs
     **************************************************************************/
    public class TdWs_RestClient
    {
        private bool _isUsingHttps;
        private string _restUrl;

        public bool IsUsingHttps
        {
            get { return _isUsingHttps; }
            set { _isUsingHttps = value; }
        }

        public string RestUrl
        {
            get { return _restUrl; }
            set { _restUrl = value; }
        }

        public TdWs_RestClient()
        {
            _isUsingHttps = false;
            _restUrl = null;
        }

        public TdWs_RestClient(bool isUsingHttps, string restUrl)
        {
            _isUsingHttps = isUsingHttps;
            _restUrl = restUrl;
        }

        protected string GetRestServiceUrl()
        {
            StringBuilder sb = new StringBuilder();

            if (_isUsingHttps)
            {
                sb.Append(@"https://");
            }
            else
            {
                sb.Append(@"http://");
            }
            sb.Append(_restUrl);

            return sb.ToString();
        }

        //public async Task<List<bool>> TestConnection()
        public bool TestConnection()
        {
            bool bSuccess = false;

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
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(url);

            //// Make the call and get a valid response
            //HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            //response.EnsureSuccessStatusCode();

            //// Read out the result... it better be JSON!
            //string JsonResult = response.Content.ReadAsStringAsync().Result;
            //returnData = JsonConvert.DeserializeObject<List<bool>>(JsonResult);

            //client.Dispose();
            //response.Dispose();

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
            var sData = sr.ReadToEnd();

            returnData = JsonConvert.DeserializeObject<List<bool>>(sData);
            if (returnData.Count > 0) { bSuccess = returnData[0]; }

            return bSuccess;
            //return returnData;
        }

        public async Task<List<T>> GetData<T>(string filterType, string filterText)
        {
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
                url += @"q/" + typeof(T).Name + @"/" + filterType + @"?v=" + Uri.EscapeDataString(filterText);
            }
            else
            {
                url += @"all/" + typeof(T).Name;
            }

            // Create a HTTP client to call the REST service
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            // Make the call and get a valid response
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode();

            // Read out the result... it better be JSON!
            string JsonResult = response.Content.ReadAsStringAsync().Result;
            returnData = JsonConvert.DeserializeObject<List<T>>(JsonResult);

            client.Dispose();
            response.Dispose();

            return returnData;
        }

        public async Task<bool> UpdateTechnicianRecord(JT_Technician technician)
        {
            bool returnData = false;

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

            // Make the call and get a valid response
            HttpResponseMessage response
                = await client.PutAsync(client.BaseAddress, new StringContent(JsonConvert.SerializeObject(technician, microsoftDateFormatSettings), null, "application/json")); // puke.... await
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
                errorReporting.sendException(ex, "TechDashboard.TdWs_RestClient.cs.UpdateTechnicianRecord");

            }

            client.Dispose();
            response.Dispose();

            return returnData;
        }

        public async Task<bool> InsertTransactionImportDetailRecord(JT_TransactionImportDetail importDetail)
        {
            bool returnData = false;

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

            // Make the call and get a valid response
            HttpResponseMessage response
                = await client.PostAsync(client.BaseAddress, new StringContent(JsonConvert.SerializeObject(importDetail, microsoftDateFormatSettings), null, "application/json")); // puke.... await
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
                errorReporting.sendException(ex, "TechDashboard.TdWs_RestClient.InsertTransactionImportDetailRecord");

            }

            client.Dispose();
            response.Dispose();

            return returnData;
        }
    }
}
