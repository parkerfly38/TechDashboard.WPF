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
     * 02/03/2017 DCH Add try/catch on page initialization.
     *********************************************************************************************************/
    public class PartsListPageViewModel
    {
        #region properties

        CI_Options _ciOptions;
        public string quantityFormatString { get; set; }
        public string umFormatString { get; set; }
        public string costFormatString { get; set; }
        public string priceFormatString { get; set; }

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
            // dch rkl 02/06/2017 Add try/catch here
            try
            {
                _ciOptions = App.Database.GetCIOptions();
                quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
                umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
                costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
                priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");
               // dch rkl 12/07/2016 catch exception

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
