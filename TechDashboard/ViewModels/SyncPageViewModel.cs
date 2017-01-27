using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDashboard.Models;
using RestSharp;
using TechDashboard.Data;
using System.Runtime.CompilerServices;

namespace TechDashboard.ViewModels
{
    /**************************************************************************************************
    * Page Name:   SyncPageViewModel
    * Description: Sync Page View Model
    *-------------------------------------------------------------------------------------------------
    *   Date       By      Description
    * ---------- --------- ---------------------------------------------------------------------------
    * 12/05/2016   DCH     Lot/SerialNo controlled items need to be split into 1 per row when they
    *                      are synced.
    * 12/07/2016   DCH     Add error handling
    * 01/24/2017   BK      Added property notification
    **************************************************************************************************/

    public class SyncPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<JT_TransactionImportDetail> _transactionImportDetails;

        public List<JT_TransactionImportDetail> transactionImportDetails
        {
            get { return _transactionImportDetails; }
            set { _transactionImportDetails = value;  }
        }

        private string _lastSyncDate;
        public string LastSyncDate
        {
            get {
                return (_lastSyncDate != null) ? "Last sync performed: " + _lastSyncDate : "";
            }
            set
            {
                _lastSyncDate = value;
                this.OnPropertyChanged("LastSyncDate");
            }
        }

        public int UpdateCount
        {
            get { return _transactionImportDetails.Count; }
        }

        public SyncPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _transactionImportDetails = App.Database.GetCurrentExport();
                _lastSyncDate = App.Database.GetApplicationSettings().LastSyncDate;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SyncPageViewModel");
            }
        }

        // dch rkl 12/09/2016 return number failed and number successful
        //public void syncWithServer()
        public void syncWithServer(ref int syncSuccess, ref int syncFailed)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                // dch rkl 12/09/2016 return number failed and number successful
                syncSuccess = 0;
                syncFailed = 0;

                TechDashboard.Data.RestClient restClient = new Data.RestClient(App.Database.GetApplicationSettings().IsUsingHttps, App.Database.GetApplicationSettings().RestServiceUrl);

                foreach (JT_TransactionImportDetail transaction in _transactionImportDetails)
                {
                    // dch rkl 12/05/2016 If Lot/Serial Nbr Data, sync back to JobOps with multiple rows
                    //bool updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction);
                    bool updateWorked = true;
                    if (transaction.LotSerialNo == null || transaction.LotSerialNo.Trim().Length == 0)
                    {
                        // dch rkl 12/09/2016 This now returns a results object
                        //updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction);
                        updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction).Success;
                    }
                    else
                    {
                        // Split into LotSerNo/Qty strings
                        string[] lotSerQty = transaction.LotSerialNo.Split('|');
                        double qty = 0;

                        foreach (string lsq in lotSerQty)
                        {
                            // Split each LotSerNo/Qty string into LotSerNo and Qty
                            string[] sqty = lsq.Split('~');
                            if (sqty.GetUpperBound(0) > 0)
                            {
                                double.TryParse(sqty[1], out qty);
                                if (qty > 0)
                                {
                                    transaction.QuantityUsed = qty;
                                    transaction.LotSerialNo = sqty[0];
                                    // dch rkl 12/09/2016 This now returns a results object
                                    //bool updateWorkedLS = restClient.InsertTransactionImportDetailRecordSync(transaction);
                                    bool updateWorkedLS = 
                                        restClient.InsertTransactionImportDetailRecordSync(transaction).Success;
                                    if (updateWorkedLS == false)
                                    {
                                        updateWorked = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if(updateWorked)
                    {
                        App.Database.DeleteExportRow(transaction);

                        // dch rkl 12/09/2016 return number failed and number successful
                        syncSuccess++;
                    }
                    // dch rkl 12/09/2016 return number failed and number successful
                    else
                    {
                        syncFailed++;
                    }
                }

                _transactionImportDetails = App.Database.GetCurrentExport();
                PropertyChanged(this, new PropertyChangedEventArgs("UpdateCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("transactionImportDetails"));

                JT_Technician technician = App.Database.GetCurrentTechnicianFromDb();

                var techUpdateWorked = restClient.UpdateTechnicianRecordSync(technician);

                PropertyChanged(this, new PropertyChangedEventArgs("UpdateCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("transactionImportDetails"));

                App_Settings appSettings = App.Database.GetApplicationSettings();
                appSettings.LastSyncDate = DateTime.Now.ToString();
                LastSyncDate = appSettings.LastSyncDate;
                App.Database.SaveAppSettings(appSettings);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SyncPageViewModel.syncWithServer");
            }
        }
        
    }
}
