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
            IsSignedIn = false;

            var listOfTechnicians = App.Database.GetTechniciansFromDB();

            TechnicianList = new ObservableCollection<App_Technician>(listOfTechnicians.ToList());
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
            App.Database.CreateDependentTables(technician);

            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            appSettings.LoggedInTechnicianNo = technician.TechnicianNo;
            appSettings.LoggedInTechnicianDeptNo = technician.TechnicianDeptNo;
            App.Database.SaveAppSettings(appSettings);

            IsSignedIn = true;
        }

        public void SignIn(App_Technician technician)
        {
            App.Database.SaveTechnicianAsCurrent(technician);
            App.Database.CreateDependentTables(technician);

            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            appSettings.LoggedInTechnicianNo = technician.TechnicianNo;
            appSettings.LoggedInTechnicianDeptNo = technician.TechnicianDeptNo;
            App.Database.SaveAppSettings(appSettings);

            IsSignedIn = true;
        }

    }
}

