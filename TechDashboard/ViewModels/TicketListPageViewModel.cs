using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Sage.SData.Client;
using TechDashboard.Models;
using TechDashboard.Data;
using TechDashboard.Services;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * TicketListPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class TicketListPageViewModel : INotifyPropertyChanged
    {
        //public ObservableCollection<Grouping<string, JT_Technician>> TechnicianGroup;
        public ObservableCollection<JT_WorkTicket> TicketList;
        protected App _app;

        public TicketListPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                var listOfTickets = App.Database.GetWorkTicketsFromDB();

                TicketList = new ObservableCollection<JT_WorkTicket>(listOfTickets.ToList());
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TicketListPageViewModel()");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetWorkTicketForApplication(JT_WorkTicket workTicket)
        {
            //App.Database.SaveWorkTicketAsCurrent(workTicket);

           // App.SetWorkTicketAsCurrent(workTicket);
        }
    }
}

