using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * PartsEditExtdDescPageViewModel.cs
     * 01/13/2017 DCH Created
     *********************************************************************************************************/
    public class PartsEditExtdDescPageViewModel : INotifyPropertyChanged
    {
        #region properties

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        App_RepairPart _partToEdit;
        public App_RepairPart PartToEdit
        {
            get { return _partToEdit; }
        }

        #endregion

        public PartsEditExtdDescPageViewModel(App_RepairPart partToEdit, App_WorkTicket workTicket)
        {
            try
            {
                _partToEdit = partToEdit;
                _workTicket = workTicket;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditExtdDescPageViewModel(App_RepairPart partToEdit, App_WorkTicket workTicket)");
            }
        }

        public void UpdateExtdDesc(string extdDescr)
        {
            // Save the Description
            try
            {
                App.Database.SaveRepairPartDesc(_partToEdit, _workTicket, App.CurrentTechnician, extdDescr);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditExtdDescPageViewModel.UpdatePartOnPartsList");
            }
        }
    }
}
