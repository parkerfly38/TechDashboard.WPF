using System;
using System.Collections.Generic;
using System.Text;

using TechDashboard.Models;

namespace TechDashboard.Data
{
    public class SDataClient : Sage.SData.Client.SDataClient
    {
        public SDataClient(bool isUsingHttps, string sDataUrl, string userId, string password)
            : base(isUsingHttps, sDataUrl, userId, password)
        {
            // empty
        }

        public SDataClient(TechDashboardDatabase database) : base()
        {
            App_Settings appSettings = database.GetApplicatioinSettings();

            if (appSettings != null)
            {
                IsUsingHttps = appSettings.IsUsingHttps;
                SDataUrl = appSettings.SDataUrl;
                UserId = appSettings.SDataUserId;
                Password = appSettings.SDataPassword;
            }
        }

        public SDataClient(App_Settings appSettings) : base()
        {
            if (appSettings != null)
            {
                IsUsingHttps = appSettings.IsUsingHttps;
                SDataUrl = appSettings.SDataUrl;
                UserId = appSettings.SDataUserId;
                Password = appSettings.SDataPassword;
            }
        }
    }
}
