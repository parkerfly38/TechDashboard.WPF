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
    public class TicketDetailsPageViewModel : INotifyPropertyChanged
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

		protected App_CustomerContact _customerContact;
		public App_CustomerContact CustomerContact {
			get { return _customerContact; }
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

        protected App_SalesOrder _salesOrder;
        public App_SalesOrder SalesOrder
        {
            get { return _salesOrder; }
        }

        protected App_ScheduledAppointment _scheduledAppointment;
        public App_ScheduledAppointment ScheduledAppointment
        {
            get { return _scheduledAppointment; }
        }

		protected App_Technician _appTechnician;
		public App_Technician AppTechnician 
		{
			get { return _appTechnician; }
		}

        public TicketDetailsPageViewModel(App_ScheduledAppointment scheduledAppointment)
        {
            _scheduledAppointment = scheduledAppointment;
            _workTicket = App.Database.GetWorkTicket(_scheduledAppointment);
             _customer = App.Database.GetAppCustomer(_workTicket);
            _salesOrder = App.Database.GetSalesOrder(_workTicket, _customer);
           
			if (_workTicket.DtlRepairItemCode != null) {
				_repairItem = App.Database.GetItemFromDB (_workTicket.DtlRepairItemCode);
			} else {
				_repairItem = new CI_Item ();
			}
			JT_Technician jt_technician = App.Database.GetCurrentTechnicianFromDb();
			_appTechnician = new App_Technician(jt_technician);
			AR_CustomerContact customerContact = App.Database.GetCustomerContact(_customer.ContactCode);
			_customerContact = new App_CustomerContact(customerContact);
        }

        public TicketDetailsPageViewModel()
        {
            _scheduledAppointment = App.Database.RetrieveCurrentScheduledAppointment();

            if (_scheduledAppointment != null)
            {
                _workTicket = App.Database.GetWorkTicket(_scheduledAppointment);
                _customer = App.Database.GetAppCustomer(_salesOrderHeader.CustomerNo);
                _salesOrder = App.Database.GetSalesOrder(_workTicket, _customer);
                if (_workTicket.DtlRepairItemCode != null)
                {
                    _repairItem = App.Database.GetItemFromDB(_workTicket.DtlRepairItemCode);
                }
                else {
                    _repairItem = new CI_Item();
                }
                JT_Technician jt_technician = App.Database.GetCurrentTechnicianFromDb();
                _appTechnician = new App_Technician(jt_technician);
                AR_CustomerContact customerContact = App.Database.GetCustomerContact(_customer.ContactCode);
                _customerContact = new App_CustomerContact(customerContact);
            }
        }

        //protected JT_WorkTicket RetrieveWorkTicket(string workTicketNumber)
        //{
        //    return App.Database.GetWorkTicket(workTicketNumber);
        //}


        protected AR_Customer RetrieveCustomer(string customerNumber)
        {
            return App.Database.GetCustomer(customerNumber);
        }

        
    }
}
