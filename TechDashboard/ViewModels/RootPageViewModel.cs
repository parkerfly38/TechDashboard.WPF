using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using TechDashboard.Models;
using TechDashboard.Data;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * RootPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class RootPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected App_Customer _customer;
        public App_Customer Customer
        {
            get { return _customer; }
        }

        protected App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        protected CI_Item _repairItem;
        public CI_Item RepairItem
        {
            get { return _repairItem; }
        }

        protected SO_SalesOrderHeader _salesOrderHeader;
        public SO_SalesOrderHeader SalesOrderHeader
        {
            get { return _salesOrderHeader; }
        }

        public RootPageViewModel()
        {
            SetDataItems();
        }

        protected void SetDataItems()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = App.Database.GetCurrentWorkTicket();
                _salesOrderHeader = App.Database.RetrieveSalesOrderHeaderFromCurrentWorkTicket();
                _customer = App.Database.GetCustomerFromCurrentWorkTicket();
                _repairItem = App.Database.RetrieveRepairItemFromCurrentWorkTicket();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.RootPageViewModel.SetDataItems");
            }
        }

        public void RefreshData()
        {
            SetDataItems();
        }

        //public void RootPageViewModel_Refresh
    }
}
