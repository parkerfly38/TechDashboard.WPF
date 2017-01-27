using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * SyncDetailViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class SyncDetailViewModel
    {
        private List<JT_TransactionImportDetail> _transactionImportDetails;
        public List<JT_TransactionImportDetail> transactionImportDetails
        {
            get { return _transactionImportDetails; }
            set { _transactionImportDetails = value; }
        }

        public SyncDetailViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _transactionImportDetails = App.Database.GetCurrentExport();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SyncDetailViewModel");
            }

        }
    }
}
