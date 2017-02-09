using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * ExpensesListPageViewModel.cs
     * 11/21/2016 DCH Add TODO
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class ExpensesListPageViewModel : INotifyPropertyChanged
    {
        #region properties

        protected App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        protected ObservableCollection<App_Expense> _expensesList;
        public ObservableCollection<App_Expense> ExpensesList
        {
            get { return _expensesList; }
        }

        public bool IsWorkTicketSelected
        {
            get { return (_workTicket != null); }
        }

        #endregion

        public ExpensesListPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expensesList = new ObservableCollection<App_Expense>();
                SetWorkTicket(App.CurrentWorkTicket);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel()");
            }
        }

        public ExpensesListPageViewModel(JT_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expensesList = new ObservableCollection<App_Expense>();
                SetWorkTicket(workTicket);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel(JT_WorkTicket workTicket)");
            }
        }

        public void SetWorkTicket(JT_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                //_workTicket = workTicket; TODO
                SetExpensesList();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.SetWorkTicket(JT_WorkTicket workTicket)");
            }
        }

        public ExpensesListPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expensesList = new ObservableCollection<App_Expense>();
                SetWorkTicket(workTicket);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel(App_WorkTicket workTicket)");
            }
        }

        public ExpensesListPageViewModel(App_ScheduledAppointment scheduledAppointment)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expensesList = new ObservableCollection<App_Expense>();
                SetWorkTicket(App.Database.GetWorkTicket(scheduledAppointment));
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel(App_ScheduledAppointment scheduledAppointment)");
            }
        }

        public void SetWorkTicket(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = workTicket;
                SetExpensesList();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.SetWorkTicket(App_WorkTicket workTicket)");
            }
        }

        public void SetWorkTicket(string formattedWorkTicketNumber)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = App.Database.GetWorkTicket2(formattedWorkTicketNumber);
                OnPropertyChanged("WorkTicket");
                SetExpensesList();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.SetWorkTicket(string formattedWorkTicketNumber)");
            }
        }

        public List<App_ScheduledAppointment> GetScheduledAppointments()
        {
            // dch rkl 12/07/2016 catch exception
            List<App_ScheduledAppointment> lsAppts = new List<App_ScheduledAppointment>();
            try
            {
                lsAppts = App.Database.GetScheduledAppointmentsNoDupes();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.GetScheduledAppointments(string formattedWorkTicketNumber)");
            }

            //return App.Database.GetScheduledAppointments();
            return lsAppts;
        }

        public void RefreshExpensesList()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                SetExpensesList();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.RefreshExpensesList()");
            }
        }

        protected void SetExpensesList()
        {
            _expensesList.Clear();
            try
            {
                foreach (App_Expense expense in App.Database.GetExpensesForWorkTicket2(_workTicket))
                {
                    _expensesList.Add(expense);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesListPageViewModel.SetExpensesList()");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
