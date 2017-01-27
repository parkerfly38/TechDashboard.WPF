using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * PartsAddPageViewModel.cs
     * 12/01/2016 DCH Add Warehouse Filter
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/

    public class PartsAddPageViewModel
    {
        #region properties

        ObservableCollection<App_Item> _itemList;
        public ObservableCollection<App_Item> ItemList
        {
            get { return _itemList; }
        }

        App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        #endregion

        public PartsAddPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = workTicket;

                // dch rkl 12/01/2016 add warehouse filter BEGIN
                if (App.CurrentTechnician.DefaultWarehouse != null && App.CurrentTechnician.DefaultWarehouse.Trim().Length > 0)
                {
                    _itemList = new ObservableCollection<App_Item>(App.Database.GetItems(null, null, App.CurrentTechnician.DefaultWarehouse));
                }
                else
                {
                    _itemList = new ObservableCollection<App_Item>(App.Database.GetItems());
                }
                //_itemList = new ObservableCollection<App_Item>(App.Database.GetItems());
                // dch rkl 12/01/2016 add warehouse filter END
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsAddPageViewModel(App_WorkTicket workTicket)");
            }
        }

        public void FilterItemList(string filterText)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _itemList.Clear();
                // dch rkl 12/01/2016 add warehouse filter
                string whseFilter = "";
                if (App.CurrentTechnician.DefaultWarehouse != null && App.CurrentTechnician.DefaultWarehouse.Trim().Length > 0)
                {
                    whseFilter = App.CurrentTechnician.DefaultWarehouse;
                }
                //foreach (App_Item newItem in App.Database.GetItems(filterText, filterText))
                foreach (App_Item newItem in App.Database.GetItems(filterText, filterText, whseFilter))
                {
                    _itemList.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsAddPageViewModel.FilterItemList");
            }
        }
    }
}
