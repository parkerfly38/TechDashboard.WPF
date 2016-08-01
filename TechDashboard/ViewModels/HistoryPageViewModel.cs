using System;
using TechDashboard.Models;
using System.Collections.Generic;
using System.Linq;


namespace TechDashboard.ViewModels
{
	public class HistoryPageViewModel
	{
		protected App_WorkTicket _workTicket;
		public App_WorkTicket WorkTicket
		{
			get { return _workTicket; }
		}

		protected List<App_History> _history;
		public List<App_History> History {
			get { return _history; }
		}

		public HistoryPageViewModel (string formattedTicketNumber)
		{
			_workTicket = App.Database.GetWorkTicketPUKE(formattedTicketNumber);
            _history = new List<App_History>();
			CI_Item item = App.Database.GetItemFromDB(_workTicket.DtlRepairItemCode);
			JT_EquipmentAsset equipmentAsset = App.Database.GetEquipmentAsset(_workTicket.DtlRepairItemCode);

            //get any possible work tickets and associated work tickets
            List<JT_WorkTicket> workTickets = App.Database.GetWorkTickets(_workTicket.DtlRepairItemCode, _workTicket.DtlMfgSerialNo);

            foreach (var workTicket in workTickets)
            {
                List<JT_Transaction> transactionCode = App.Database.GetTransactions(workTicket.SalesOrderNo);
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
                    _history.Add(history);
                }
            }

            _history = _history.OrderBy(x => x.TransactionDate).ToList();
		}
	}
}

