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
        /*********************************************************************************************************
         * CustomerDetailsPageViewModel.cs
         * 12/07/2016 DCH Add error handling
         *********************************************************************************************************/

        #region Properties

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

        #endregion

        public CustomerDetailsPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _customer = App.Database.GetCustomerFromCurrentWorkTicket();
                _customerContacts = App.Database.GetAppCustomerContacts(_customer.CustomerNo);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.CustomerDetailsPageViewModel()");
            }
        }

        public CustomerDetailsPageViewModel(App_Customer customer)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _customer = customer;
                _customerContacts = App.Database.GetAppCustomerContacts(_customer.CustomerNo);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.CustomerDetailsPageViewModel(App_Customer customer)");
            }
        }
    }
}
