using System;
using TechDashboard.Models;
using System.Collections.Generic;
using System.Linq;

namespace TechDashboard.Data
{
	public partial class TechDashboardDatabase
	{
		public JT_Transaction GetTransaction(string ItemCode) 
		{
			return _database.Table<JT_Transaction>().Where(x => x.ItemCode == ItemCode).FirstOrDefault();
		}

		public JT_TransactionHistory GetTransaction(JT_TransactionHistory transaction)
		{
			return _database.Table<JT_TransactionHistory>().Where(x =>
				x.SalesOrderNo == transaction.SalesOrderNo 
				&& x.WTNumber == transaction.WTNumber
				&& x.WTStep == transaction.WTStep
				&& x.TransactionDate == transaction.TransactionDate).FirstOrDefault();
		}

		public List<JT_TransactionHistory> GetTransactionHistory()
		{
			return _database.Table<JT_TransactionHistory>().ToList();
		}

		public List<JT_TransactionHistory> GetTransactionHistory(string SalesOrderNo)
		{
			return _database.Table<JT_TransactionHistory>().Where(x => x.SalesOrderNo == SalesOrderNo).ToList();
		}

		public List<JT_Transaction> GetTransactions(string SalesOrderNo)
		{
			return _database.Table<JT_Transaction>().Where(x => x.SalesOrderNo == SalesOrderNo).ToList();
		}

		public List<JT_TransactionHistory> GetTransactionHistory(string SalesOrderNo, string WTNumber, string WTStep)
		{
			return _database.Table<JT_TransactionHistory>().Where(x => x.SalesOrderNo == SalesOrderNo && x.WTNumber == WTNumber
			&& x.WTStep == WTStep).ToList();
		}

		public JT_EquipmentAsset GetEquipmentAsset(string ItemCode)
		{
			return _database.Table<JT_EquipmentAsset>().Where(x => x.ItemCode == ItemCode).FirstOrDefault();
		}

		public JT_ServiceAgreementHeader GetServiceAgreementHeader(string ContractCode)
		{
			return _database.Table<JT_ServiceAgreementHeader>().Where(x => x.ContractCode == ContractCode).FirstOrDefault();
		}

		public JT_ServiceAgreementDetail GetServiceAgreementDetail(string ContractCode)
		{
			return _database.Table<JT_ServiceAgreementDetail>().Where(x => x.ContractCode == ContractCode).FirstOrDefault();
		}

		public JT_LaborText GetLaborText(JT_Transaction transaction)
		{
			return _database.Table<JT_LaborText>().Where(x => x.SalesOrderNo == transaction.SalesOrderNo
			&& x.WTNumber == transaction.WTNumber
			&& x.WTStep == transaction.WTStep
			&& x.TransactionDate == transaction.TransactionDate
			).FirstOrDefault();
		}

		public JT_LaborText GetLaborText(JT_TransactionHistory transaction)
		{
			return _database.Table<JT_LaborText>().Where(x => x.SalesOrderNo == transaction.SalesOrderNo
				&& x.WTNumber == transaction.WTNumber
				&& x.WTStep == transaction.WTStep
				&& x.TransactionDate == transaction.TransactionDate
			).FirstOrDefault();
		}
	}
}

