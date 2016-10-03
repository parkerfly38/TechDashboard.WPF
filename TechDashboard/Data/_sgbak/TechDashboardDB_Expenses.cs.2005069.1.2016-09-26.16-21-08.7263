using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;
using TechDashboard.Tools;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {

        #region Expenses

        /// <summary>
        /// Retreives Expense data from the ERP connection and uses
        /// it to fill the local App_Expenses table.
        /// </summary>
        public void FillExpensesTable()
        {
            throw new NotImplementedException("No expenses table yet.");
        }

        public JT_MiscellaneousCodes GetExpenseCategory(string category)
        {
            JT_MiscellaneousCodes expenseCategory = GetMiscellaneousCodeFromDB("*E", category);
            return expenseCategory;
        }

        //public List<JT_MiscellaneousCodes> GetExpenseCategories()
        //{
        //    List<JT_MiscellaneousCodes> categoriesList = null;

        //    lock (_locker)
        //    {
        //        categoriesList = GetMiscellaneousCodesFromDB("*E");
        //    }

        //    return categoriesList;
        //}


        public List<string> GetExpenseCategories()
        {
            lock (_locker)
            {
                var categories = from codes in GetMiscellaneousCodesFromDB("*E")
                                 select codes.MiscellaneousCode;

                return categories.ToList();
            }
        }

        public List<string> GetExpenseChargeCodes()
        {
            List<string> chargeCodes = null;
            char[] delimeters = new char[] { ';' };
            string code = null;

            // puke
            lock (_locker)
            {
                var addtlDescNums =
                    from codes in GetMiscellaneousCodesFromDB("*E")
                    select codes.AddtlDescNum;

                if (addtlDescNums != null)
                {
                    chargeCodes = new List<string>();

                    foreach (string s in addtlDescNums)
                    {
                        code = s.Split(delimeters, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (chargeCodes.Contains(code))
                        {
                            continue;
                        }
                        chargeCodes.Add(code);
                    }
                }
            }

            return chargeCodes;
        }

        public JT_MiscellaneousCodes GetChargeCode(string code)
        {
            JT_MiscellaneousCodes chargeCode = null;

            chargeCode = GetMiscellaneousCodesFromDB("*E").Where(cc => cc.MiscellaneousCode == code).FirstOrDefault();

            return chargeCode;
        }

        #region ERP JT_TransactionImportDetails

        /// <summary>
        /// Retrieves a list of all App_Expense objects from the local database table.
        /// </summary>
        /// <returns>A List container of App_Expense objects.</returns>
        public List<JT_TransactionImportDetail> GetExpensesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_TransactionImportDetail>().Where(tid => tid.RecordType == "E").OrderBy(tid => tid.ItemCode).ToList();
            }
        }

        /// <summary>
        /// Retrieves a list of all JT_TransactionImportDetail objects from the local database table
        /// representing expense items.
        /// </summary>
        /// <returns>A List container of JT_TransactionImportDetail expense objects.</returns>
        public List<JT_TransactionImportDetail> GetExpensesForWorkTicket(JT_WorkTicket workTicket)
        {
            lock (_locker)
            {
                return _database.Table<JT_TransactionImportDetail>().Where(
                    tid =>
                        (tid.RecordType == "E") &&
                        (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                        (tid.WTNumber == workTicket.WTNumber) &&
                        (tid.WTStep == workTicket.WTStep)
                ).OrderBy(tid => tid.ItemCode).ToList();
            }
        }

        /// <summary>
        /// Retrieves an App_Expense object from the local database table
        /// for the ID provided.
        /// </summary>
        /// <returns>A JT_TransactionImportDetail expense object.</returns>
        public JT_TransactionImportDetail GetExpenseFromDB(int recordId)
        {
            lock (_locker)
            {
                return _database.Table<JT_TransactionImportDetail>().Where(tid => tid.ID == recordId).FirstOrDefault();
            }
        }

        public List<App_Expense> GetExpensesForWorkTicketPUKE(App_WorkTicket workTicket)
        {
            
            List<App_Expense> expensesList = null;

            lock (_locker)
            {
                List<JT_TransactionImportDetail> txnImportDetailList =
                    _database.Table<JT_TransactionImportDetail>().Where(
                        tid =>
                            (tid.RecordType == "E") &&
                            (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                            (tid.WTNumber == workTicket.WTNumber) &&
                            (tid.WTStep == workTicket.WTStep)
                    ).OrderBy(tid => tid.ItemCode).ToList();

                if((txnImportDetailList != null) && (txnImportDetailList.Count > 0))
                {
                    expensesList = new List<App_Expense>();

                    foreach(JT_TransactionImportDetail importDetail in txnImportDetailList)
                    {
                        expensesList.Add(new App_Expense(importDetail, workTicket));
                    }
                }
            }

            return expensesList;
        }

        /// <summary>
        /// Saves the provided App_Expense item to the local database
        /// and flags it as modified.
        /// </summary>
        /// <param name="expense">The App_Expense item to save</param>
        public void SaveExpense(JT_TransactionImportDetail expense)
        {
            int rows = 0;

            lock (_locker)
            {
                if (expense.ID != 0)
                {
                    rows = _database.Update(expense);
                    System.Diagnostics.Debug.WriteLine("Number of rows updated = " + rows.ToString());
                }
                else
                {
                    expense.RecordType = "E";
                    rows = _database.Insert(expense);
                    System.Diagnostics.Debug.WriteLine("Number of rows inserted = " + rows.ToString());
                }
            }
        }

        /// <summary>
        /// Saves the provided App_Expense item to the local database
        /// </summary>
        /// <param name="expense">The App_Expense item to save</param>
        public void SaveExpense(App_Expense expense)
        {
            int rows = 0;

            JT_TransactionImportDetail txnImportDetail = new JT_TransactionImportDetail();
            txnImportDetail.RecordType = "E";
            txnImportDetail.ID = expense.ID;
            txnImportDetail.EmployeeNo = App.Database.GetCurrentTechnicianFromDb().TechnicianNo;
            txnImportDetail.EmployeeDeptNo = App.Database.GetCurrentTechnicianFromDb().TechnicianDeptNo;
            txnImportDetail.TransactionDate = expense.Date.ToSage100DateString();
            txnImportDetail.SalesOrderNo = expense.WorkTicket.SalesOrderNo;
            txnImportDetail.WTNumber = expense.WorkTicket.WTNumber;
            txnImportDetail.WTStep = expense.WorkTicket.WTStep;
            txnImportDetail.ItemCode = expense.ChargeCode;
            txnImportDetail.UnitOfMeasure = expense.UnitOfMeasure;
            txnImportDetail.QuantityUsed = expense.Quantity;
            txnImportDetail.UnitCost = expense.UnitCost;
            txnImportDetail.ReimburseEmployee = (expense.IsReimbursable ? "Y" : "N");
            txnImportDetail.UnitPrice = expense.UnitPrice;
            txnImportDetail.BillingDescription = expense.BillingDescription;
            txnImportDetail.ChargePart = (expense.IsChargeableToCustomer ? "Y" : "N");

            lock (_locker)
            {
                if (txnImportDetail.ID != 0)
                {
                    rows = _database.Update(txnImportDetail);
                    System.Diagnostics.Debug.WriteLine("Number of rows updated = " + rows.ToString());
                }
                else
                {
                    rows = _database.Insert(txnImportDetail);
                    System.Diagnostics.Debug.WriteLine("Number of rows inserted = " + rows.ToString());
                }
            }
        }

        #endregion

        public App_WorkTicket GetWorkTicketPUKE(string formattedWorkTicketNumber)
        {
            string[] brokenTicketNumber = App_WorkTicket.BreakFormattedTicketNumber(formattedWorkTicketNumber);
            string salesOrderNumber = brokenTicketNumber[0];
            string workTicketNumber = brokenTicketNumber[1];
            string workTicketStep = brokenTicketNumber[2];

            JT_TechnicianScheduleDetail scheduleDetail =
                _database.Table<JT_TechnicianScheduleDetail>().Where(
                    sd => (sd.SalesOrderNo == salesOrderNumber) &&
                          (sd.WTNumber == workTicketNumber) &&
                          (sd.WTStep == workTicketStep)
                ).OrderByDescending(sd => sd.ScheduleDate).FirstOrDefault();

            App_ScheduledAppointment scheduledAppointment = 
                new App_ScheduledAppointment(scheduleDetail, GetSalesOrderHeader(scheduleDetail));

            App_WorkTicket workTicket = GetWorkTicket(scheduledAppointment);

            return workTicket;
        }

        public List<JT_WorkTicket> GetWorkTickets(string RepairItemCode, string MfgSerialNo)
        {
            List<JT_WorkTicket> workTickets = new List<JT_WorkTicket>();

            IEnumerable<JT_WorkTicket> dbWorkTickets = _database.Table<JT_WorkTicket>()
                .Where(x => x.DtlRepairItemCode == RepairItemCode && x.DtlMfgSerialNo == MfgSerialNo);

            IEnumerable<JT_WorkTicketHistory> dbWorkTicketsHistory = _database.Table<JT_WorkTicketHistory>()
                .Where(x => x.DtlRepairItemCode == RepairItemCode && x.DtlMfgSerialNo == MfgSerialNo);

            foreach(var item in dbWorkTickets)
            {
                workTickets.Add(item);
            }
            foreach (var item in dbWorkTicketsHistory)
            {
                workTickets.Add(new JT_WorkTicket
                {
                    ActivityCode = item.ActivityCode,
                    Description = item.Description,
                    DtlCoverageExceptionCode = item.DtlCoverageExceptionCode,
                    DtlCoveredOnContract = item.DtlCoveredOnContract,
                    DtlInternalSerialNo = item.DtlInternalSerialNo,
                    DtlMfgSerialNo = item.DtlMfgSerialNo,
                    DtlPreventitiveMaintenance = item.DtlPreventitiveMaintenance,
                    DtlProblemCode = item.DtlProblemCode,
                    DtlRepairItemCode = item.DtlRepairItemCode,
                    DtlWarrantyRepair = item.DtlWarrantyRepair,
                    HdrContactCode = item.HdrContactCode,
                    HdrServiceContractCode = item.HdrServiceContractCode,
                    HdrTemplateNo = item.HdrTemplateNo,
                    HdrWtClass = item.HdrWtClass,
                    SalesOrderNo = item.SalesOrderNo,
                    StatusCode = item.StatusCode,
                    WTNumber = item.WTNumber,
                    WTStep = item.WTStep
                });
            }
            return workTickets;
        }



        #endregion

    }
}
