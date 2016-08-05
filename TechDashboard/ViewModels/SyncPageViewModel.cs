using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDashboard.Models;
using RestSharp;

namespace TechDashboard.ViewModels
{
    public class SyncPageViewModel : INotifyPropertyChanged
    {
        private List<JT_TransactionImportDetail> _transactionImportDetails;

        public List<JT_TransactionImportDetail> transactionImportDetails
        {
            get { return _transactionImportDetails; }
            set { _transactionImportDetails = value;  }
        }

        private string _lastSyncDate;
        public string LastSyncDate
        {
            get { return (_lastSyncDate != null) ? "Last sync performed: " + _lastSyncDate : ""; }
        }

        public int UpdateCount
        {
            get { return _transactionImportDetails.Count; }
        }

        public SyncPageViewModel()
        {
            _transactionImportDetails = App.Database.GetCurrentExport();
            _lastSyncDate = App.Database.GetApplicatioinSettings().LastSyncDate;
        }

        public void syncWithServer()
        {
            TechDashboard.Data.RestClient restClient = new Data.RestClient(App.Database.GetApplicatioinSettings().IsUsingHttps, App.Database.GetApplicatioinSettings().RestServiceUrl);

            foreach (var transaction in _transactionImportDetails)
            {
                var updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction);
                
                if(updateWorked)
                {
                    App.Database.DeleteExportRow(transaction);
                }                
            }

            _transactionImportDetails = App.Database.GetCurrentExport();
            PropertyChanged(this, new PropertyChangedEventArgs("UpdateCount"));
            PropertyChanged(this, new PropertyChangedEventArgs("transactionImportDetails"));

            JT_Technician technician = App.Database.GetCurrentTechnicianFromDb();

            var techUpdateWorked = restClient.UpdateTechnicianRecordSync(technician);

            PropertyChanged(this, new PropertyChangedEventArgs("UpdateCount"));
            PropertyChanged(this, new PropertyChangedEventArgs("transactionImportDetails"));

            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            appSettings.LastSyncDate = DateTime.Now.ToString();
            App.Database.SaveAppSettings(appSettings);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
