using System;
using System.Collections.Generic;
using System.Text;

using Sage.SData.Client;
using SQLite;
using System.Linq;
using TechDashboard.Models;


namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region App_SalesOrder

        public App_SalesOrder GetSalesOrder(App_WorkTicket workTicket, App_Customer customer)
        {
            App_SalesOrder returnData = null;
            SO_SalesOrderHeader salesOrderHeader = null;
            SO_ShipToAddress shipToAddress = null;

            if (workTicket != null)
            {
                lock (_locker)
                {
                    salesOrderHeader = GetSalesOrderHeader(workTicket);
                    if (salesOrderHeader != null)
                    {
                        shipToAddress = GetShipToAddress(salesOrderHeader);
                    }
                }
            }

            returnData = new App_SalesOrder(customer, salesOrderHeader, shipToAddress);

            return returnData;
        }

        public App_SalesOrder GetSalesOrder(App_ScheduledAppointment scheduledAppointment, App_Customer customer)
        {
            App_SalesOrder returnData = null;
            SO_SalesOrderHeader salesOrderHeader = null;
            SO_ShipToAddress shipToAddress = null;

            if (scheduledAppointment != null)
            {
                lock (_locker)
                {
                    salesOrderHeader = GetSalesOrderHeader(scheduledAppointment);
                    if (salesOrderHeader != null)
                    {
                        shipToAddress = GetShipToAddress(salesOrderHeader);
                    }
                }
            }

            returnData = new App_SalesOrder(customer, salesOrderHeader, shipToAddress);

            return returnData;
        }

        public App_SalesOrder GetSalesOrderForCurrentWorkTicket(App_Customer customer)
        {
            App_SalesOrder returnData = null;

            lock (_locker)
            {
                App_WorkTicket currentWorkTicket = GetCurrentWorkTicket();
                returnData = GetSalesOrder(currentWorkTicket, customer);
            }

            return returnData;
        }

        #endregion

        #region Sales Order Header

        protected void FillSalesOrderHeaderTable()
        {
            StringBuilder sb = new StringBuilder();

            List<JT_TechnicianScheduleDetail> scheduledTickets = GetTechnicianScheduleDetailFromDB();
            for (int i = 0; i < scheduledTickets.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(SalesOrderNo eq '");
                sb.Append(scheduledTickets[i].SalesOrderNo);
                sb.Append("')");
            }

            FillLocalTable<SO_SalesOrderHeader>("where", sb.ToString());
        }

        protected List<SO_SalesOrderHeader> GetSalesOrderHeadersFromDB()
        {

            lock (_locker)
            {
                return _database.Table<SO_SalesOrderHeader>().OrderBy(so => so.SalesOrderNo).ToList();
            }
        }

        protected SO_SalesOrderHeader GetSalesOrderHeader(App_ScheduledAppointment scheduledAppointment)
        {
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                if (scheduledAppointment != null)
                {
                    salesOrderHeader =
                        _database.Table<SO_SalesOrderHeader>().Where(
                            so => (so.SalesOrderNo == scheduledAppointment.SalesOrderNumber)
                        ).FirstOrDefault();
                }
            }

            return salesOrderHeader;
        }

        protected SO_SalesOrderHeader GetSalesOrderHeader(JT_TechnicianScheduleDetail scheduleDetail)
        {
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                if (scheduleDetail != null)
                {
                    salesOrderHeader =
                        _database.Table<SO_SalesOrderHeader>().Where(
                            so => (so.SalesOrderNo == scheduleDetail.SalesOrderNo)
                        ).FirstOrDefault();
                }
            }

            return salesOrderHeader;
        }

        protected SO_SalesOrderHeader GetSalesOrderHeader(App_WorkTicket workTicket)
        {
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    salesOrderHeader =
                        _database.Table<SO_SalesOrderHeader>().Where(
                            so => (so.SalesOrderNo == workTicket.SalesOrderNo)
                        ).FirstOrDefault();
                }
            }

            return salesOrderHeader;
        }

        protected SO_SalesOrderHeader GetSalesOrderHeader(JT_WorkTicket workTicket)
        {
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    salesOrderHeader =
                        _database.Table<SO_SalesOrderHeader>().Where(
                            so => (so.SalesOrderNo == workTicket.SalesOrderNo)
                        ).FirstOrDefault();
                }
            }

            return salesOrderHeader;
        }

        public SO_SalesOrderHeader RetrieveSalesOrderHeaderFromCurrentWorkTicket()
        {
            SO_SalesOrderHeader salesOrderHeader = null;

            lock (_locker)
            {
                JT_TechnicianScheduleDetail currentDetail = RetrieveCurrentScheduleDetail();

                if (currentDetail != null)
                {
                    salesOrderHeader =
                        _database.Table<SO_SalesOrderHeader>().Where(
                            so => so.SalesOrderNo == currentDetail.SalesOrderNo
                        ).FirstOrDefault();
                }
            }

            return salesOrderHeader;
        }

        #endregion

        #region SO_SalesOrderDetail

        #endregion

        #region SO_ShipToAddress

        protected void FillShipToAddressTable()
        {
            StringBuilder sb = new StringBuilder();

            List<SO_SalesOrderHeader> salesOrderHeaders = _database.Table<SO_SalesOrderHeader>().ToList();

            for (int i = 0; i < salesOrderHeaders.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(ARDivisionNo eq '");
                sb.Append(salesOrderHeaders[i].ARDivisionNo);
                sb.Append("' and CustomerNo eq '");
                sb.Append(salesOrderHeaders[i].CustomerNo);
                sb.Append("' and ShipToCode eq '");
                sb.Append(salesOrderHeaders[i].ShipToCode);
                sb.Append("')");
            }

            FillLocalTable<SO_ShipToAddress>("where", sb.ToString());
        }

        protected SO_ShipToAddress GetShipToAddress(SO_SalesOrderHeader salesOrderHeader)
        {
            SO_ShipToAddress returnData = null;

            if (salesOrderHeader != null)
            {
                lock (_locker)
                {
                    returnData =
                        _database.Table<SO_ShipToAddress>().Where(
                            sta => ((sta.ARDivisionNo == salesOrderHeader.ARDivisionNo) &&
                                    (sta.CustomerNo == salesOrderHeader.CustomerNo) &&
                                    (sta.ShipToCode == salesOrderHeader.ShipToCode))
                        ).FirstOrDefault();
                }
            }

            return returnData;
        }

        #endregion
    }
}
