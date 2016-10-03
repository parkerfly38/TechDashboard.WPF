using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using TechDashboard.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace TechDashboard.ViewModels
{

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
            List<App_ScheduledAppointment> newList = App.Database.GetScheduledAppointments().Where(x => x.ScheduleDate >= startDate && x.ScheduleDate <= endDate).ToList();
            ScheduleDetails.Clear();
            foreach (var item in newList)
            {
                ScheduleDetails.Add(item);
            }
            OnPropertyChanged("ScheduleDetails");
        }

        public SchedulePageViewModel()
        {
            _clockedInTimeEntry = App.Database.GetClockedInTimeEntry();
            _scheduleDetails = new ObservableCollection<App_ScheduledAppointment>(App.Database.GetScheduledAppointments());

            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            _defaultStartDate = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysBefore) * (-1))).Date;
            _defaultEndDate = (DateTime.Now.AddDays(Convert.ToDouble(appSettings.ScheduleDaysAfter))).Date;

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
