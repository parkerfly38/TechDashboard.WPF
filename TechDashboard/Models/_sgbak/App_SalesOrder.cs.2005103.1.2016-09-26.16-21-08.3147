using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_SalesOrder
    {
        private App_Customer _customer;
        private SO_SalesOrderHeader _salesOrderHeader;
        private SO_ShipToAddress _shipToAddress;

        public string ShipToName
        {
            get { return _salesOrderHeader.ShipToName; }
        }

        public string ShipToAddress1
        {
            get { return _salesOrderHeader.ShipToAddress1; }
        }

        public string ShipToAddress2
        {
            get { return _salesOrderHeader.ShipToAddress2; }
        }

        public string ShipToAddress3
        {
            get { return _salesOrderHeader.ShipToAddress3; }
        }

        public string ShipToCity
        {
            get { return _salesOrderHeader.ShipToCity; }
        }

        public string ShipToState
        {
            get { return _salesOrderHeader.ShipToState; }
        }

        public string ShipToZipCode
        {
            get { return _salesOrderHeader.ShipToZipCode; }
        }

        public string TelephoneNo
        {
            get { return (_shipToAddress != null ? _shipToAddress.TelephoneNo : _customer.TelephoneNo); }
        }

        public string TelephoneExt
        {
            get { return (_shipToAddress != null ? _shipToAddress.TelephoneExt : _customer.TelephoneExt); }
        }

        public SO_SalesOrderHeader SalesOrderHeader
        {
            get { return _salesOrderHeader; }
        }

        public App_SalesOrder(App_Customer customer, SO_SalesOrderHeader salesOrderHeader, SO_ShipToAddress shipToAddress)
        {
            _customer = customer;
            _salesOrderHeader = salesOrderHeader;
            _shipToAddress = shipToAddress;
        }
    }
}
