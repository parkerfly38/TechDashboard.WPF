using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TechDashboard.Tools
{
    public class GeoCode
    {
        // Geocode an address and return a latitude and longitude
        public XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=ArBc9gk1rx37doHgAYHLu4nTVK1N2rf1I_PxonXvcsEZi0y3VVg6Kp5a0bFX018X";


            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

    }
}
