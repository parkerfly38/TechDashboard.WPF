using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_CurrentWorkTicket 
    {
        JT_WorkTicket _workTicket;
        JT_WorkTicketText _workTicketText;
        SO_SalesOrderHeader _salesOrderHeader;
        AR_Customer _customer;
        // puke history
        //List<JT_ServiceEquipmentParts> _partsList;
        CI_Item _repairItem;
        JT_DailyTimeEntry _timeEntry;

        public JT_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        public JT_WorkTicketText Notes
        {
            get { return _workTicketText; }
        }

        public SO_SalesOrderHeader SalesOrderHeader
        {
            get { return _salesOrderHeader; }
        }

        public AR_Customer Customer
        {
            get { return _customer; }
        }

        public CI_Item RepairItem
        {
            get { return _repairItem; }
        }

        public List<JT_ServiceEquipmentParts> PartsList
        {
            //get { return _partsList; }
            get { return App.Database.RetrievePartsListFromCurrentWorkTicket(); }
        }

        public App_CurrentWorkTicket() 
        {
            // empty
            _workTicket = App.Database.RetrieveCurrentWorkTicket();       
            _workTicketText = App.Database.RetrieveTextFromCurrentWorkTicket();
            _salesOrderHeader = App.Database.RetrieveSalesOrderHeaderFromCurrentWorkTicket();
            _customer = App.Database.RetrieveCustomerFromCurrentWorkTicket();
            _repairItem = App.Database.RetrieveRepairItemFromCurrentWorkTicket();
            //_partsList = App.Database.RetrievePartsListFromCurrentWorkTicket();

            //_timeEntry = App.Database.
        }
    }
}