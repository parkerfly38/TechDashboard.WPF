using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

using TechDashboard.Data;
using TechDashboard.Models;

namespace Sage.SData.Client
{
    public class SDataClient
    {
        private bool _isUsingHttps;
        private string _sDataUrl;
        private string _userId;
        private string _password;

        public SDataClient(TechDashboardDatabase database)
        {
            if (database.HasValidSetup())
            {
                App_Settings appSettings = database.GetApplicatioinSettings();
                if (appSettings != null)
                {
                    _isUsingHttps = appSettings.IsUsingHttps;
                    _sDataUrl = appSettings.SDataUrl;
                    _userId = appSettings.SDataUserId;
                    _password = appSettings.SDataPassword;
                }
            }
        }

        public SDataClient(App_Settings appSettings)
        {
            //_isUsingHttps = false;
            //_sDataUrl = @"jobops2015dev.rkldev.local/sdata/MasApp/MasContract/JOB/";
            //_userId = @"sdata";
            //_password = @"RKLsupp@rt";

            if (appSettings != null)
            {
                _isUsingHttps = appSettings.IsUsingHttps;
                _sDataUrl = appSettings.SDataUrl;
                _userId = appSettings.SDataUserId;
                _password = appSettings.SDataPassword;
            }
        }

        public SDataClient(bool isUsingHttps, string sDataUrl, string userId, string password)
        {
            _isUsingHttps = isUsingHttps;
            _sDataUrl = sDataUrl;
            _userId = userId;
            _password = password;
        }

        protected string GetFullSDataUrl()
        {
            StringBuilder sb = new StringBuilder();

            // Create the sData URL from our class member data.
            if (_isUsingHttps)
            {
                sb.Append(@"https://");
            }
            else
            {
                sb.Append(@"http://");
            }
            sb.Append(_sDataUrl);

            return sb.ToString();
        }

        public List<T> GetData<T>(string filterType, string filterText)
        {
            // Set up our return data object -- a list of typed objects.
            List<T> returnData = new List<T>();

            // Grab a listing of all the properties of this type of object
            PropertyInfo[] properties = typeof(T).GetProperties();

            //Create a SData serivce object connection.  Use the proper url and login info.
            string sDataUrl = GetFullSDataUrl();
            ISDataService svc = new SDataService(GetFullSDataUrl(), _userId, _password);

            // Now create the request, passing in the ISDataService we created above
            var req = new SDataResourceCollectionRequest(svc);

            // Tell it which kind of resource we want to access.
            // Note, this needs to match the values on the SData tab
            // of the entity in Application Architect
            // e.g., req.ResourceKind = "AR_Customer";
            Type objectType = typeof(T);
            req.ResourceKind = objectType.Name;

            // This part is optional (without it we'd be getting ALL objects of type T).
            // This is our where clause, or condition of which contacts we want.
            // In this example we want all customers whose last name starts with
            // the value 'American'. We need to use the exact property name as defined
            // in the entity (case-sensitive).
            //req.QueryValues.Add("where", @"CustomerName like 'American%'");

            req.QueryValues.Add(filterType, filterText);
            req.Count = 500; // puke - we may need to check OpenSearch numbers to see if we got it all. 

            // Now read the data (or run the query)
            AtomFeed feed = null;
            try
            {
                feed = req.Read();
            }
            catch (Exception ex)
            {
                // no data... return our empty list object
                return returnData;
            }

            // We now have the results in our AtomFeed variable, which is
            // basically a list of AtomEntry objects. To get to our data,
            // we need to read the payload from each AtomEntry and then we
            // can access the values for each field from it's Values
            // dictionary. In this example, we'll just write a few fields
            // from each contact to the console.

            foreach (AtomEntry entry in feed.Entries)
            {
                // Get the payload for this entity
                SDataPayload payload = entry.GetSDataPayload();

                // Create an instance of type T to add to our return list
                T myObj = Activator.CreateInstance<T>();

                // loop through the properties of T to find matching data from our query
                string name;
                foreach (PropertyInfo property in properties)
                {
                    name = property.Name;
                    if (!payload.Values.ContainsKey(name))
                    {
                        // the returned data doesn't contain this property at all,
                        //  so skip it.
                        continue;
                    }

                    if (payload.Values[name] == null)
                    {
                        property.SetValue(myObj, null);
                    }
                    else
                    {
                        // we found a match, so set the value of this property in this object instance
                        property.SetValue(myObj, Convert.ChangeType(payload.Values[name], property.GetMethod.ReturnType));
                    }
                }

                // add the T object instance to our return list
                returnData.Add(myObj);
            }

            // return the entire list back to the caller.
            return returnData;
        }
    }
}
