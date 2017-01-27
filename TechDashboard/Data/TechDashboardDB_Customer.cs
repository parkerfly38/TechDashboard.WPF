using System;
using System.Collections.Generic;
using System.Text;

using Sage.SData.Client;
using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_Customer.cs
     * 12/01/2016 DCH Add TODO
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region App_Customer

        public App_Customer GetAppCustomer(string customerNumber)
        {
            // TODO rename
            AR_Customer customer;

            lock (_locker)
            {
                customer = _database.Table<AR_Customer>().Where(c => c.CustomerNo == customerNumber).FirstOrDefault();
            }

            return new App_Customer(customer);
        }
        public App_Customer GetAppCustomer(App_WorkTicket workTicket)
        {
            AR_Customer currentCustomer = null;

            lock (_locker)
            {
                SO_SalesOrderHeader salesOrderHeader = GetSalesOrderHeader(workTicket);

                if (salesOrderHeader != null)
                {
                    currentCustomer =
                        _database.Table<AR_Customer>().Where(
                            c => c.CustomerNo == salesOrderHeader.CustomerNo
                        ).FirstOrDefault();
                }
            }

            if (currentCustomer == null)
            {
                return null;
            }
            else
            {
                return new App_Customer(currentCustomer);
            }
        }

        public App_Customer GetCustomerFromCurrentWorkTicket()
        {
            AR_Customer currentCustomer = null;

            lock (_locker)
            {
                SO_SalesOrderHeader salesOrderHeader = RetrieveSalesOrderHeaderFromCurrentWorkTicket();

                if (salesOrderHeader != null)
                {
                    currentCustomer =
                        _database.Table<AR_Customer>().Where(
                            c => c.CustomerNo == salesOrderHeader.CustomerNo
                        ).FirstOrDefault();
                }
            }

            if (currentCustomer == null)
            {
                return null;
            }
            else
            {
                return new App_Customer(currentCustomer);
            }
        }

        #endregion

        #region App_CustomerContact

        public App_CustomerContact GetAppCustomerContact(string contactCode)
        {
            AR_CustomerContact customerContact;

            lock (_locker)
            {
                customerContact = _database.Table<AR_CustomerContact>().Where(cc => cc.ContactCode == contactCode).FirstOrDefault();
            }

            return new App_CustomerContact(customerContact);
        }

        public List<App_CustomerContact> GetAppCustomerContacts(string customerNumber)
        {
            List<App_CustomerContact> customerContacts = new List<App_CustomerContact>();

            lock (_locker)
            {
                foreach (AR_CustomerContact contact in _database.Table<AR_CustomerContact>().Where(cc => cc.CustomerNo == customerNumber).ToList())
                {
                    customerContacts.Add(new App_CustomerContact(contact));
                }
            }

            return customerContacts;
        }

        #endregion

        #region ERP AR_Customer

        public void FillCustomerTable()
        {
            StringBuilder sb = new StringBuilder();

            List<SO_SalesOrderHeader> salesOrders = GetSalesOrderHeadersFromDB();
            for (int i = 0; i < salesOrders.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(CustomerNo eq '");
                sb.Append(salesOrders[i].CustomerNo);
                sb.Append("')");
            }

            FillLocalTable<AR_Customer>("where", sb.ToString()); 
        }



        public AR_Customer GetCustomer(string customerNumber)
        {
            AR_Customer customer;

            lock (_locker)
            {
                customer = _database.Table<AR_Customer>().Where(c => c.CustomerNo == customerNumber).FirstOrDefault();
            }

            return customer;
        }



        public AR_Customer RetrieveCustomerFromCurrentWorkTicket()
        {
            AR_Customer currentCustomer = null;

            lock (_locker)
            {
                SO_SalesOrderHeader salesOrderHeader = RetrieveSalesOrderHeaderFromCurrentWorkTicket();

                if (salesOrderHeader != null)
                {
                    currentCustomer =
                        _database.Table<AR_Customer>().Where(
                            c => c.CustomerNo == salesOrderHeader.CustomerNo
                        ).FirstOrDefault();
                }
            }

            return currentCustomer;
        }

        #endregion

        #region ERP AR_CustomerContact

        public void FillCustomerContactTable()
        {
            StringBuilder sb = new StringBuilder();

            List<SO_SalesOrderHeader> salesOrders = GetSalesOrderHeadersFromDB();
            for (int i = 0; i < salesOrders.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(CustomerNo eq '");
                sb.Append(salesOrders[i].CustomerNo);
                sb.Append("')");
            }

            FillLocalTable<AR_CustomerContact>("where", sb.ToString());  // TODO filter
        }



        public List<AR_CustomerContact> GetCustomerContacts(string customerNumber)
        {// TODO

            List<AR_CustomerContact> customerContacts = null;

            lock (_locker)
            {
                customerContacts = _database.Table<AR_CustomerContact>().Where(cc => cc.CustomerNo == customerNumber).ToList();
            }

            return customerContacts;
        }



        public AR_CustomerContact GetCustomerContact(string contactCode)
        {// TODO
            AR_CustomerContact customerContact;

            lock (_locker)
            {
                customerContact = _database.Table<AR_CustomerContact>().Where(cc => cc.ContactCode == contactCode).FirstOrDefault();
            }

            return customerContact;
        }

        #endregion
    }
}
