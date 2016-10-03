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
    public class TechnicianPageViewModel : INotifyPropertyChanged
    {
        protected App_Technician _technician;
        public App_Technician Technician
        {
            get { return _technician; }
        }

        protected List<JT_TechnicianStatus> _technicianStatusList;
        public List<JT_TechnicianStatus> TechnicianStatusList
        {
            get { return _technicianStatusList; }
        }

        public TechnicianPageViewModel()
        {
            _technician = App.Database.RetrieveCurrentTechnician();
            _technicianStatusList = App.Database.GetTechnicianStatusesFromDB();
        }

        public void UpdateTechnicianStatus(JT_TechnicianStatus newStatus)
        {
            App.Database.UpdateTechnicianStatus(_technician, newStatus);            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }    
    }
}
