using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using TechDashboard.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using TechDashboard.Data;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * SchedulePageViewModel.cs
     * 12/01/2016 DCH Correct Misspelling of GetApplicationSettings
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class SchedulePageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected ObservableCollection<App_ScheduledAppointment> _scheduleDetails;
        public ObservableCollection<App_ScheduledAppointment> ScheduleDetails
        {
            get { return _scheduleDetails; }
            set
            {
                _scheduleDetails = value;
                this.OnPropertyChanged();
            }
        }

        protected JT_DailyTimeEntry _clockedInTimeEntry;
        public JT_DailyTimeEntry ClockedInTimeEntry
        {
            get { return _clockedInTimeEntry; }
        }

        protected DateTime _defaultStartDate;
        public DateTime DefaultStartDate
        {
            get { return _defaultStartDate;  }
            set
            {
                _defaultStartDate = value;
                OnPropertyChanged("DefaultStartDate");
            }
        }

        protected DateTime _defaultEndDate;
        public DateTime DefaultEndDate
        {
            get { return _defaultEndDate; }
            set
            {
                _defaultEndDate = value;
                OnPropertyChanged("DefaultEndDate");
            }
        }

        public void filterScheduledAppointments(DateTime startDate, DateTime endDate)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                List<App_ScheduledAppointment> newList = App.Database.GetScheduledAppointments().Where(x => x.ScheduleDate >= startDate && x.ScheduleDate <= endDate).ToList();
                ScheduleDetails.Clear();
                foreach (var item in newList)
                {
                    ScheduleDetails.Add(item);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SchedulePageViewModel.filterScheduledAppointments");
            }

            OnPropertyChanged("ScheduleDetails");
        }

        public void filterByTicketNumber(string ticketNumber)
        {
            try
            {
                List<App_ScheduledAppointment> newList = App.Database.GetScheduledAppointments().Where(x => x.ServiceTicketNumber.Contains(ticketNumber)).ToList();
                ScheduleDetails.Clear();
                foreach (var item in newList)
                {
                    ScheduleDetails.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SchedulePageViewModel.filterByTicketNumber");
            }

            OnPropertyChanged("ScheduleDetails");
        }

        public SchedulePageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App_Settings appSettings = App.Database.GetApplicationSettings();

                _clockedInTimeEntry = App.Database.GetClockedInTimeEntry();
                _scheduleDetails = new ObservableCollection<App_ScheduledAppointment>(App.Database.GetScheduledAppointments());

                // dch rkl 10/14/2016 if any tickets in the schedule list are marked COM (completed)
                // in the JT_TransactionImportDetail table, remove them from the schedule list
                List<JT_TransactionImportDetail> lsTrans = App.Database.GetCurrentExport();
                if (lsTrans.Count > 0)
                {
                    var copy = new ObservableCollection<App_ScheduledAppointment>(_scheduleDetails);
                    foreach (App_ScheduledAppointment sched in copy)
                    {
                        foreach (JT_TransactionImportDetail tran in lsTrans)
                        {
                            if (tran.RecordType == "S" && tran.SalesOrderNo == sched.SalesOrderNumber && tran.WTNumber == sched.WorkTicketNumber && tran.WTStep == sched.WorkTicketStep && tran.StatusCode == "COM")
                            {
                                _scheduleDetails.Remove(sched);
                                break;
                            }
                        }
                    }
                }
                // dch rkl 10/14/2016 END

                _defaultStartDate = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysBefore) * (-1))).Date;
                _defaultEndDate = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysAfter))).Date;

            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SchedulePageViewModel");
            }

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
