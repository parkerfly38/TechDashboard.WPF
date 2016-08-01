using System;
using System.Collections.Generic;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class AppSettingsPageViewModel
    {
        private App_Settings _appSettings;

        public bool IsUsingHttps
        {
            get { return _appSettings.IsUsingHttps; }
            set { _appSettings.IsUsingHttps = value; }
        }
        public string SDataUrl
        {
            get { return _appSettings.SDataUrl; }
            set { _appSettings.SDataUrl = value; }
        }

        public string SDataUserId
        {
            get { return _appSettings.SDataUserId; }
            set { _appSettings.SDataUserId = value; }
        }

        public string SDataPassword
        {
            get { return _appSettings.SDataPassword; }
            set { _appSettings.SDataPassword = value; }
        }

        public int ScheduleDaysBefore
        {
            get { return _appSettings.ScheduleDaysBefore; }
            set { _appSettings.ScheduleDaysBefore = value; }
        }

        public int ScheduleDaysAfter
        {
            get { return _appSettings.ScheduleDaysAfter; }
            set { _appSettings.ScheduleDaysAfter = value; }
        }

        public string RestServiceUrl
        {
            get { return _appSettings.RestServiceUrl; }
            set { _appSettings.RestServiceUrl = value; }
        }

        public bool Use24HourTime
        {
            get { return _appSettings.TwentyFourHourTime; }
            set { _appSettings.TwentyFourHourTime = value; }
        }

        public string LoggedInTechnicianNo
        {
            get { return _appSettings.LoggedInTechnicianNo; }
            set { _appSettings.LoggedInTechnicianNo = value; }
        }

        public string LoggedInTechnicianDeptNo
        {
            get { return _appSettings.LoggedInTechnicianDeptNo; }
            set { _appSettings.LoggedInTechnicianDeptNo = value; }
        }

        public string DeviceName
        {
            get { return _appSettings.DeviceName;  }
            set { _appSettings.DeviceName = value; }
        }

        public AppSettingsPageViewModel()
        {
            _appSettings = App.Database.GetApplicatioinSettings();
            if(_appSettings == null)
            {
                _appSettings = new App_Settings();
            }
        }

        public void SaveAppSettings()
        {
            App.Database.SaveAppSettings(_appSettings);
        }
    }
}