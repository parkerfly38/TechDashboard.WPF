using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;
using System.Data;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_ServiceAgreement.cs
     * 12/01/2016 DCH Exclude Service Agreements with null division
     * 01/31/2017 DCH Make sure there aren't duplicates in the filtering for service agreements.
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region Service Agreement Header

        public void FillServiceAgreementHeaderTable()
        {
            // Fill Service Agreement Details based on Tickets
            //FillLocalTable<JT_ServiceAgreementHeader>();  // TODO filter

            // dch rkl 01/31/2017 prevent duplicate filters, which generates long query string
            DataTable dtFilter = new DataTable("Filter");
            dtFilter.Columns.Add("ContractCode", typeof(string));
            dtFilter.Columns.Add("CustomerNo", typeof(string));
            dtFilter.Columns.Add("ARDivisionNo", typeof(string));

            StringBuilder sbH = new StringBuilder();

            List<JT_TechnicianScheduleDetail> scheduledTickets = GetTechnicianScheduleDetailFromDB();

           List<JT_WorkTicket> tickets = GetWorkTicketsFromDB();
            List<SO_SalesOrderHeader> sohs = GetSalesOrderHeadersFromDB();

            for (int i = 0; i < scheduledTickets.Count; i++)
            {
                JT_WorkTicket ticket = tickets.FirstOrDefault(s => s.SalesOrderNo == scheduledTickets[i].SalesOrderNo &&
                    s.WTNumber == scheduledTickets[i].WTNumber && s.WTStep == "000" && s.HdrServiceContractCode != null);
                if (ticket != null)
                {
                    SO_SalesOrderHeader soHdr = sohs.FirstOrDefault(s => s.SalesOrderNo == scheduledTickets[i].SalesOrderNo);
                    if (soHdr != null)
                    {
                        // dch rkl 01/31/2017 prevent duplicate filters, which generates long query string
                        DataView dvFilter = new DataView(dtFilter);
                        dvFilter.RowFilter = "ContractCode = '" + ticket.HdrServiceContractCode + "' and CustomerNo = '" + soHdr.CustomerNo + "' and ARDivisionNo = '" + soHdr.ARDivisionNo + "'";
                        if (dvFilter.Count == 0)
                        {
                            dtFilter.Rows.Add(ticket.HdrServiceContractCode, soHdr.CustomerNo, soHdr.ARDivisionNo);
                        }
                        //if (sbH.Length > 0)
                        //{
                        //    sbH.Append(" or ");
                        //}
                        //sbH.Append("(ContractCode eq '");
                        //sbH.Append(ticket.HdrServiceContractCode);
                        //sbH.Append("' and CustomerNo eq '");
                        //sbH.Append(soHdr.CustomerNo);
                        //sbH.Append("' and ARDivisionNo eq '");
                        //sbH.Append(soHdr.ARDivisionNo);

                        //sbH.Append("')");
                        // dch rkl 01/31/2017 prevent duplicate filters, which generates long query string END
                    }
                }
            }

            // dch rkl 01/31/2017 prevent duplicate filters, which generates long query string BEGIN
            foreach (DataRow row in dtFilter.Rows)
            {
                if (sbH.Length > 0)
                {
                    sbH.Append(" or ");
                }
                sbH.Append("(ContractCode eq '");
                sbH.Append(row["ContractCode"].ToString());
                sbH.Append("' and CustomerNo eq '");
                sbH.Append(row["CustomerNo"].ToString());
                sbH.Append("' and ARDivisionNo eq '");
                sbH.Append(row["ARDivisionNo"].ToString());

                sbH.Append("')");
            }
            // dch rkl 01/31/2017 prevent duplicate filters, which generates long query string END

            FillLocalTable<JT_ServiceAgreementHeader>("where", sbH.ToString());
            FillLocalTable<JT_ServiceAgreementDetail>("where", sbH.ToString());
            FillLocalTable<JT_ServiceAgreementPMDetail>("where", sbH.ToString());
        }

        #endregion

        #region App_ServiceAgreement

        public App_ServiceAgreement GetServiceAgreement(App_Customer customer)
        {
            JT_ServiceAgreementHeader serviceAgreementHeader = null;
            JT_ServiceAgreementDetail serviceAgreementDetail = null;
            JT_ServiceAgreementPMDetail serviceAgreementPmDetail = null;

            lock (_locker)
            {
                serviceAgreementHeader =
                    _database.Table<JT_ServiceAgreementHeader>().Where(
                        sah => (sah.ARDivisionNo == customer.ARDivisionNo) &&
                               (sah.CustomerNo == customer.CustomerNo)
                    ).FirstOrDefault();

                serviceAgreementDetail =
                    _database.Table<JT_ServiceAgreementDetail>().Where(
                        sad => (sad.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                               (sad.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                               (sad.ContractCode == serviceAgreementHeader.ContractCode)
                    ).FirstOrDefault();

                serviceAgreementPmDetail =
                    _database.Table<JT_ServiceAgreementPMDetail>().Where(
                        sapmd => (sapmd.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                 (sapmd.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                 (sapmd.ContractCode == serviceAgreementHeader.ContractCode)
                    ).FirstOrDefault();
            }

            return new App_ServiceAgreement(serviceAgreementHeader, serviceAgreementDetail, serviceAgreementPmDetail);
        }

        public App_ServiceAgreement GetServiceAgreement(JT_WorkTicket workTicket)
        {
			JT_ServiceAgreementHeader serviceAgreementHeader = new JT_ServiceAgreementHeader();
			JT_ServiceAgreementDetail serviceAgreementDetail = new JT_ServiceAgreementDetail();
			JT_ServiceAgreementPMDetail serviceAgreementPmDetail = new JT_ServiceAgreementPMDetail();

            lock (_locker)
            {
                serviceAgreementHeader =
                    _database.Table<JT_ServiceAgreementHeader>().Where(
                        sah => (sah.ContractCode == workTicket.HdrServiceContractCode && sah.ARDivisionNo != null)
                    ).FirstOrDefault();

                if (serviceAgreementHeader != null)
                {
                    serviceAgreementDetail =
                        _database.Table<JT_ServiceAgreementDetail>().Where(
                            sad => (sad.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                   (sad.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                   (sad.ContractCode == serviceAgreementHeader.ContractCode)
                        ).FirstOrDefault();

                    serviceAgreementPmDetail =
                        _database.Table<JT_ServiceAgreementPMDetail>().Where(
                            sapmd => (sapmd.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                     (sapmd.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                     (sapmd.ContractCode == serviceAgreementHeader.ContractCode)
                        ).FirstOrDefault();
                }
            }

            return new App_ServiceAgreement(serviceAgreementHeader, serviceAgreementDetail, serviceAgreementPmDetail);
        }

        #endregion
    }
}
