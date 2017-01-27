using System;
using TechDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using TechDashboard.Data;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * HistoryPageViewModel.cs
     * 11/22/2016 DCH Add Filter by Parts/Labor
     * 12/07/2016 DCH Add error handling
     * 01/16/2017 DCH Only display history details if the quantity is not zero
     *********************************************************************************************************/
    public class HistoryPageViewModel
	{
        #region properties

        protected App_WorkTicket _workTicket;
		public App_WorkTicket WorkTicket
		{
			get { return _workTicket; }
		}

        protected CI_Item _item;
        public CI_Item Item
        {
            get { return _item; }
        }

		protected List<App_History> _history;
		public List<App_History> History {
			get { return _history; }
		}

        protected string _TicketNumber;
        public string TicketNumber
        {
            get { return _TicketNumber; }
        }

        #endregion

        // dch rkl 11/22/2016 Add filter by parts or labor
        //public HistoryPageViewModel (string formattedTicketNumber)
        public HistoryPageViewModel(string formattedTicketNumber, string partsLaborAll)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _TicketNumber = formattedTicketNumber;
			    _workTicket = App.Database.GetWorkTicket2(formattedTicketNumber);
                _history = new List<App_History>();
			    CI_Item item = App.Database.GetItemFromDB(_workTicket.DtlRepairItemCode);
                _item = item;
			    JT_EquipmentAsset equipmentAsset = App.Database.GetEquipmentAsset(_workTicket.DtlRepairItemCode);

                //get any possible work tickets and associated work tickets
                List<JT_WorkTicket> workTickets = App.Database.GetWorkTickets(_workTicket.DtlRepairItemCode, _workTicket.DtlMfgSerialNo); //, _workTicket.SalesOrderNo);

                foreach (var workTicket in workTickets)
                {
                    List<JT_Transaction> transactionCode = App.Database.GetTransactions(workTicket.SalesOrderNo, workTicket.WTNumber, workTicket.WTStep);
                    List<JT_TransactionHistory> transactionHistoryCode = App.Database.GetTransactionHistory(workTicket.SalesOrderNo, workTicket.WTNumber, workTicket.WTStep);
                    //List<JT_Transaction> transactionCode = App.Database.GetTransactions(workTicket.SalesOrderNo);
                    JT_ServiceAgreementHeader serviceAgreementHeader;
                    JT_ServiceAgreementDetail serviceAgreementDetail;
                    if (equipmentAsset != null && equipmentAsset.ContractCode != null)
                    {
                        serviceAgreementHeader = App.Database.GetServiceAgreementHeader(equipmentAsset.ContractCode);
                        serviceAgreementDetail = App.Database.GetServiceAgreementDetail(equipmentAsset.ContractCode);
                    }
                    else
                    {
                        serviceAgreementHeader = new JT_ServiceAgreementHeader();
                        serviceAgreementDetail = new JT_ServiceAgreementDetail();
                    }

                    foreach (var transaction in transactionCode)
                    {
                        JT_LaborText laborText = App.Database.GetLaborText(transaction);
                        var history = new App_History(
                                          _workTicket,
                                          item,
                                          equipmentAsset,
                                          serviceAgreementHeader,
                                          serviceAgreementDetail,
                                          transaction,
                                          laborText);
                        // dch rkl 11/22/2016 Add filter by parts or labor BEGIN
                        if ((partsLaborAll == "L" && transaction.RecordType == "LD")
                            || (partsLaborAll == "P" && transaction.RecordType != "LD")
                            || partsLaborAll == "A" || partsLaborAll == "")     
                        {
                            // dch rkl 01/16/2017 only if qty used <> 0
                            if (transaction.QuantityUsed != 0) { _history.Add(history); }
                            //_history.Add(history);
                        }
                        //_history.Add(history);
                        // dch rkl 11/22/2016 Add filter by parts or labor END
                    }
                    foreach (var transaction in transactionHistoryCode)
                    {
                        JT_LaborText laborText = App.Database.GetLaborText(transaction);
                        var history = new App_History(
                                          _workTicket,
                                          item,
                                          equipmentAsset,
                                          serviceAgreementHeader,
                                          serviceAgreementDetail,
                                          transaction,
                                          laborText);
                        // dch rkl 11/22/2016 Add filter by parts or labor BEGIN
                        if ((partsLaborAll == "L" && transaction.RecordType == "LD")
                            || (partsLaborAll == "P" && transaction.RecordType != "LD")
                            || partsLaborAll == "A" || partsLaborAll == "") 
                        {
                            // dch rkl 01/16/2017 only if qty used <> 0
                            if (transaction.QuantityUsed != 0) { _history.Add(history); }
                            //_history.Add(history);
                        }
                        //_history.Add(history);
                        // dch rkl 11/22/2016 Add filter by parts or labor BEGIN

                    }
                }

            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.HistoryPageViewModel()");
            }

            _history = _history.OrderBy(x => x.TransactionDate).ToList();
		}
	}
}

