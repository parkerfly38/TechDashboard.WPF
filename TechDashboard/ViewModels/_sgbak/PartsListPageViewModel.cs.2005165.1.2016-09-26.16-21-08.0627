using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class PartsListPageViewModel
    {
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

        public PartsListPageViewModel(App_WorkTicket workTicket)
        {
            _workTicket = workTicket;
            _partsList = App.Database.RetrievePartsListFromWorkTicket(workTicket);
            _observablePartsList = new ObservableCollection<App_RepairPart>();
            SetPartsList();

        }

        protected void SetPartsList()
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

        //public PartsListPageViewModel()
        //{
        //    _partsList = App.Database.RetrievePartsListFromCurrentWorkTicket();
        //}
    }
}
