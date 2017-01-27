using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * PartsListPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class PartsListPageViewModel
    {
        #region properties

        protected App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }
                
        protected List<App_RepairPart> _partsList;
        public List<App_RepairPart> PartsList
        {
            get { return _partsList; }
        }

        protected ObservableCollection<App_RepairPart> _observablePartsList;
        public ObservableCollection<App_RepairPart> ObservablePartsList
        {
            get { return _observablePartsList; }
        }

        #endregion

        public PartsListPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = workTicket;
                _partsList = App.Database.RetrievePartsListFromWorkTicket(workTicket);
                _observablePartsList = new ObservableCollection<App_RepairPart>();
                SetPartsList();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsListPageViewModel(App_WorkTicket workTicket)");
            }
        }

        protected void SetPartsList()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                List<App_RepairPart> parts = App.Database.RetrievePartsListFromWorkTicket(_workTicket);

                _observablePartsList.Clear();
                if (parts != null)
                {
                    foreach (App_RepairPart part in parts)
                    {
                        _observablePartsList.Add(part);
                    }
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsListPageViewModel.SetPartsList");
            }
        }

        //public PartsListPageViewModel()
        //{
        //    _partsList = App.Database.RetrievePartsListFromCurrentWorkTicket();
        //}
    }
}
