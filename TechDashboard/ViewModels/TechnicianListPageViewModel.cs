using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Sage.SData.Client;
using TechDashboard.Models;
using TechDashboard.Data;
using TechDashboard.Services;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * TechnicianListPageViewModel.cs
     * 12/01/2016 DCH Correct Misspelling of GetApplicationSettings
     *********************************************************************************************************/
    public class TechnicianListPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<App_Technician> TechnicianList;

        private bool _isSignedIn;
        public bool IsSignedIn
        {
            get
            {
                return _isSignedIn;
            }
            private set
            {
                _isSignedIn = value;
                NotifyPropertyChanged();
            }
        }

        public TechnicianListPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                IsSignedIn = false;

                var listOfTechnicians = App.Database.GetTechniciansFromDB();

                TechnicianList = new ObservableCollection<App_Technician>(listOfTechnicians.ToList());
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TechnicianListPageViewModel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SignIn(JT_Technician technician)
        {
            App.Database.SaveTechnicianAsCurrent(technician);
            if (App.Database.HasDataConnection())
                App.Database.CreateDependentTables(technician);

            App_Settings appSettings = App.Database.GetApplicationSettings();
            appSettings.LoggedInTechnicianNo = technician.TechnicianNo;
            appSettings.LoggedInTechnicianDeptNo = technician.TechnicianDeptNo;
            App.Database.SaveAppSettings(appSettings);

            IsSignedIn = true;
        }

        public void SignIn(App_Technician technician)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.SaveTechnicianAsCurrent(technician);
                if (App.Database.HasDataConnection())
                    App.Database.CreateDependentTables(technician);

                App_Settings appSettings = App.Database.GetApplicationSettings();
                appSettings.LoggedInTechnicianNo = technician.TechnicianNo;
                appSettings.LoggedInTechnicianDeptNo = technician.TechnicianDeptNo;
                App.Database.SaveAppSettings(appSettings);

                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TechnicianListPageViewModel.SignIn");
            }
        }

    }
}

