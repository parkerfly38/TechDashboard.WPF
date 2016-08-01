using System;
using System.Collections.Generic;

namespace TechDashboard.Models
{
	public class App_History
	{
		protected App_WorkTicket _workTicket;
		protected CI_Item _item;
		protected JT_EquipmentAsset _equipmentAsset;
		protected JT_ServiceAgreementHeader _serviceAgreementHeader;
		protected JT_ServiceAgreementDetail _serviceAgreementDetail;
		protected JT_Transaction _transaction;
		protected JT_TransactionHistory _transactionHistory;
		protected JT_LaborText _laborText;
	
		public App_History(App_WorkTicket workTicket, CI_Item item, JT_EquipmentAsset equipmentAsset,
			JT_ServiceAgreementHeader serviceAgreementHeader, JT_ServiceAgreementDetail serviceAgreementDetail,
			JT_TransactionHistory transactionHistory, JT_LaborText laborText)
		{
			_workTicket = workTicket;
			_item = item;
			_equipmentAsset = equipmentAsset;
			_serviceAgreementHeader = serviceAgreementHeader;
			_serviceAgreementDetail = serviceAgreementDetail;
			_transactionHistory = transactionHistory;
			_laborText = laborText;
		}

		public App_History(App_WorkTicket workTicket, CI_Item item, JT_EquipmentAsset equipmentAsset,
			JT_ServiceAgreementHeader serviceAgreementHeader, JT_ServiceAgreementDetail serviceAgreementDetail,
			JT_Transaction transaction, JT_LaborText laborText)
		{
			_workTicket = workTicket;
			_item = item;
			_equipmentAsset = equipmentAsset;
			_serviceAgreementHeader = serviceAgreementHeader;
			_serviceAgreementDetail = serviceAgreementDetail;
			_transaction = transaction;
			_laborText = laborText;
		}

		/// <summary>
		/// ItemCode - varchar(30)
		/// </summary>
		public string ItemCode
		{
			get { return _workTicket.DtlRepairItemCode; }
		}

		/// <summary>
		/// Item Desc - varchar(30)- source CI_Item
		/// </summary>
		public string ItemDesc 
		{
			get { 
				if (_item == null || _item.ItemCodeDesc == null)
				{
					return "";
				} else
				{
					return _item.ItemCodeDesc; 
				}
			}
		}

		/// <summary>
		/// MfgSerialNo = varchar(20), from JT_WorkTicket
		/// </summary>
		public string MfgSerialNo 
		{
			get { return _workTicket.DtlMfgSerialNo; }
		}

		/// <summary>
		/// EA Desc - varchar from JT_EquipmentAsset
		/// </summary>
		public string EADesc
		{
			get { 
				if (_equipmentAsset != null)
				{
					return _equipmentAsset.Description; 
				} else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// IntSerialNo - varchar
		/// </summary>
		public string IntSerialNo
		{
			get { return _workTicket.DtlRepairItemCode; }
		}

		public string ModelNo
		{ 
			get {
				if (_equipmentAsset != null)
				{
					return _equipmentAsset.ModelNo;
				} else
				{
					return "";
				}
			}
		}

		public string SvcAgreement
		{
			get { 
				if (_equipmentAsset != null)
				{
					return _equipmentAsset.ContractCode; 
				} else
				{
					return "";
				}
			}
		}

		public string SvcAgreementDescription
		{
			get {
                if (_serviceAgreementHeader != null && _serviceAgreementHeader.ContractDescription != null)
                {
                    return _serviceAgreementHeader.ContractDescription;
                } else
                {
                    return "";
                }
            }
		}

		public DateTime CoveredThrough
		{
			get {
                if (_serviceAgreementDetail != null && _serviceAgreementDetail.EndDate != null)
                {
                    return _serviceAgreementDetail.EndDate;
                } else
                {
                    return DateTime.Now;
                }
            }
		}

		/// <summary>
		/// Parts - varchar(1)
		/// </summary>
		/// <value>"Y" or "N"</value>
		public string Parts
		{
			get {
                if (_serviceAgreementDetail != null && _serviceAgreementDetail.PartsCovered != null)
                {
                    return _serviceAgreementDetail.PartsCovered;
                } else
                {
                    return "";
                }
            }
		}

		/// <summary>
		/// Labor - varchar(1)
		/// </summary>
		/// <value>Y/N</value>
		public string Labor
		{
			get {
                if (_serviceAgreementDetail != null && _serviceAgreementDetail.LaborCovered != null)
                {
                    return _serviceAgreementDetail.LaborCovered;
                } else
                {
                    return "";
                }
            }
		}

		public DateTime? TransactionDate
		{
			get { 
				if (_transactionHistory != null)
				{
					return _transactionHistory.TransactionDate;
				} else
				{
					return _transaction.TransactionDate;
				}
				return null;
			}
		}

		public string Trx
		{
			get {
				if (_transactionHistory != null)
				{
					if (_transactionHistory.RecordType == "LD")
					{
						return "Labor";
					} else
					{
						return "Parts";
					}
				}
				if (_transaction != null)
				{
					if (_transaction.RecordType == "LD")
					{
						return "Labor";
					} else
					{
						return "Parts";
					}
				}
				return "";
			}
		}

		public string ServiceTicketNo
		{
			get { return _transaction.SalesOrderNo + "-" + _transaction.WTNumber + "-" + _transaction.WTStep; }
		}

		public string ItemEmployee
		{
			get {
				if (_transactionHistory != null)
				{
					if (_transactionHistory.RecordType == "LD")
					{
						return _transactionHistory.Comment;
					} else
					{
						return _transactionHistory.ItemCode;
					}
				}
				if (_transaction != null)
				{
					if (_transaction.RecordType == "LD")
					{
						return _transaction.Comment;
					} else
					{
						return _transaction.ItemCode;
					}
				}
				return "";
			}
		}

		public string Description
		{
			get {
				if (_transactionHistory != null ) {
					if ( _transactionHistory.RecordType == "LD")
					{
						return _laborText.BillingText;
					} else
					{
						return _transactionHistory.ItemCodeDesc;
					}
				}
				if (_transaction != null)
				{
					if (_transaction.RecordType == "LD")
					{
						return _laborText.BillingText;
					} else
					{
						return _transaction.ItemCodeDesc;
					}
				}
				return "";
			}
		}

		public string Quantity
		{
			get { 
				if (_transactionHistory != null)
				{
					if (_transactionHistory.RecordType == "LD")
					{
						return _transactionHistory.HoursWorked.ToString();
					} else
					{
						return _transactionHistory.QuantityUsed.ToString();
					}
				}
				if (_transaction != null)
				{
					if (_transaction.RecordType == "LD")
					{
						return _transaction.HoursWorked.ToString();
					} else
					{
						return _transaction.QuantityUsed.ToString();
					}
				}
				return "0";
			}
		}

	}

}

