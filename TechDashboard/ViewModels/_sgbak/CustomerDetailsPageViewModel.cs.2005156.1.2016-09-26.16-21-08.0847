using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sage.SData.Client;
using TechDashboard.Models;
using TechDashboard.Data;
using TechDashboard.Services;

namespace TechDashboard.ViewModels
{
    public class CustomerDetailsPageViewModel
    {
        protected App_Customer _customer;
        public App_Customer Customer
        {
            get { return _customer; }
        }

        protected List<App_CustomerContact> _customerContacts;
        public List<App_CustomerContact> CustomerContacts
        {
            get { return _customerContacts; }
        }

        public CustomerDetailsPageViewModel()
        {
            _customer = App.Database.GetCustomerFromCurrentWorkTicket();
            _customerContacts = App.Database.GetAppCustomerContacts(_customer.CustomerNo);
        }

        public CustomerDetailsPageViewModel(App_Customer customer)
        {
            _customer = customer;
            _customerContacts = App.Database.GetAppCustomerContacts(_customer.CustomerNo);
        }
    }
}
