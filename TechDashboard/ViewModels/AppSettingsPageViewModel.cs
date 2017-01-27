using System;
using System.Collections.Generic;
using System.Reflection;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * AppSettingsPageViewModel.cs
     * 12/02/2016 DCH Correct spelling of GetApplicationSettings
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/

    public class AppSettingsPageViewModel
    {
        private App_Settings _appSettings;

        #region properties

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
            _appSettings = App.Database.GetApplicationSettings();
            if(_appSettings == null)
            {
                _appSettings = new App_Settings();
            }
        }

        #endregion

        public void SaveAppSettings()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _appSettings.DbVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if ((DeviceName != null) && (_appSettings.DeviceID == null || _appSettings.DeviceID.Length <= 0))
                {
                    App.Database.SaveAppSettings(_appSettings); //need to to do get right url
                    _appSettings.DeviceID = App.Database.GetDeviceID(DeviceName);
                }
                App.Database.SaveAppSettings(_appSettings);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.AppSettingsPageViewModel()");
            }
        }
    }
}