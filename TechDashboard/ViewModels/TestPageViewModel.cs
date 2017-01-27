using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using TechDashboard.Models;
using TechDashboard.Data;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * TestPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class TestPageViewModel : INotifyPropertyChanged
    {
        private TechDashboardDatabase _database;
        private SDataClient _sDataClient;

        private AR_Customer _customer;       
        public AR_Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged();
            }
        }

        public TestPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _sDataClient = new SDataClient(App.Database);
                _database = new TechDashboardDatabase();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TestPageViewModel");
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

        public void GetCustomerFromSData(string customerNumber)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                List<AR_Customer> customers = _sDataClient.GetData<AR_Customer>("where", @"CustomerNo eq '" + customerNumber + @"'");
            
                if (customers.Count == 0)
                {
                    customers.Add(new AR_Customer());
                    customers[0].ARDivisionNo = "000000";
                    customers[0].CustomerNo = "999999";
                    customers[0].CustomerName = "FAKE SDATA CUSTOMER";
                    customers[0].TelephoneNo = "717-555-1212";
                    customers[0].TelephoneExt = "123";
                }

                Customer = customers[0];
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TestPageViewModel.GetCustomerFromSData");
            }
        }

        public void GetCustomerFromDB(string customerNumber)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                AR_Customer customer = _database.GetCustomer(customerNumber);

                if (customer == null)
                {
                    customer = new AR_Customer();
                    customer.ARDivisionNo = "111111";
                    customer.CustomerNo = "888888";
                    customer.CustomerName = "FAKE DB CUSTOMER";
                    customer.TelephoneNo = "610-555-9898";
                    customer.TelephoneExt = "987";
                }

                Customer = customer;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.TestPageViewModel.GetCustomerFromDB");
            }
        }
        
    }

    

}

