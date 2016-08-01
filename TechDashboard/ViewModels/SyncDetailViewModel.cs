using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
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
            _transactionImportDetails = App.Database.GetCurrentExport();
        }
    }
}
