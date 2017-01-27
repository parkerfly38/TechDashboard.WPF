using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * ExpensesEditPageViewModel.cs
     * 11/21/2016 DCH Make sure category is not blank/null
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class ExpensesEditPageViewModel
    {
        #region Properties

        protected App_Expense _expenseItem;
        protected ObservableCollection<string> _expenseCategories;
        protected ObservableCollection<string> _expenseChargeCodes;

        public int ExpenseId
        {
            get { return _expenseItem.ID; }
        }

        public DateTime ExpenseDate
        {
            get { return _expenseItem.Date; }
            set { _expenseItem.Date = value.Date; }
        }

        public string ExpenseCategory
        {
            get { return _expenseItem.Category; }
            set
            {
                UpdateExpenseCategory(value);
            }
        }

        public string ExpenseChargeCode
        {
            get { return _expenseItem.ChargeCode; }
            set
            {
                _expenseItem.ChargeCode = value;
            }
        }

        public double ExpenseQuantity
        {
            get { return _expenseItem.Quantity; }
            set { _expenseItem.Quantity = value; }
        }

        public string ExpenseUnitOfMeasure
        {
            get { return _expenseItem.UnitOfMeasure; }
            set { _expenseItem.UnitOfMeasure = value; }
        }

        public double ExpenseUnitCost
        {
            get { return _expenseItem.UnitCost; }
            set { _expenseItem.UnitCost = value; }
        }

        public double ExpenseUnitPrice
        {
            get { return _expenseItem.UnitPrice; }
            set { _expenseItem.UnitPrice = value; }
        }

        public string ExpenseBillingDescription
        {
            get { return _expenseItem.BillingDescription; }
            set { _expenseItem.BillingDescription = value; }
        }

        public bool ExpenseIsReimbursable
        {
            get { return _expenseItem.IsReimbursable; }
            set { _expenseItem.IsReimbursable = value; }
        }

        public bool ExpenseIsChargeableToCustomer
        {
            get { return _expenseItem.IsChargeableToCustomer; }
            set { _expenseItem.IsChargeableToCustomer = value; }
        }

        /*protected string GetExpenseCategory(string chargecode)
        {
            JT_MiscellaneousCodes categoryRecord = App.Database.GetExpenseCategory(category);

        }*/

        protected void UpdateExpenseCategory(string category)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                // dch rkl 11/21/2016 make sure category is not blank/null BEGIN
                if (category != null && category.Trim().Length > 0)
                {
                    // dch rkl 11/21/2016 make sure category is not blank/null END
                    JT_MiscellaneousCodes categoryRecord = App.Database.GetExpenseCategory(category);
                    string[] brokenDescription = App_Expense.BreakDescription(categoryRecord.AddtlDescNum);

                    try
                    {
                        _expenseItem.ChargeCode = brokenDescription[0];
                    }
                    catch
                    {
                        _expenseItem.ChargeCode = null;
                    }
                    try
                    {
                        _expenseItem.UnitOfMeasure = brokenDescription[1];
                    }
                    catch
                    {
                        _expenseItem.UnitOfMeasure = null;
                    }
                    try
                    {
                        _expenseItem.UnitPrice = double.Parse(brokenDescription[2]);
                    }
                    catch
                    {
                        _expenseItem.UnitPrice = 0;
                    }
                    // dch rkl 11/21/2016 make sure category is not blank/null BEGIN
                }
                else
                {
                    _expenseItem.ChargeCode = "";
                    _expenseItem.UnitOfMeasure = null;
                    _expenseItem.UnitPrice = 0;
                }
                // dch rkl 11/21/2016 make sure category is not blank/null END
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesEditPageViewModel.UpdateExpenseCategory");
            }
        }

        //public App_Expense ExpenseItem
        //{
        //    get { return _expenseItem; }
        //}

        public ObservableCollection<string> ExpenseCategories
        {
            get { return _expenseCategories; }
        }

        public ObservableCollection<string> ExpenseChargeCodes
        {
            get { return _expenseChargeCodes; }
        }

        #endregion

        public ExpensesEditPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expenseItem = new App_Expense(new JT_TransactionImportDetail(), workTicket);

                Initialize();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesEditPageViewModel(App_WorkTicket workTicket)");
            }
        }

        public ExpensesEditPageViewModel(App_Expense expenseItem)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expenseItem = expenseItem;
                //_expenseItem.Category = 
                Initialize();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.ExpensesEditPageViewModel(App_Expense expenseItem)");
            }
        }

        protected void Initialize()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _expenseCategories = new ObservableCollection<string>(App.Database.GetExpenseCategories());
                _expenseChargeCodes = new ObservableCollection<string>(App.Database.GetExpenseChargeCodes());
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.Initialize()");
            }
        }

        public void SaveExpenseItem()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.SaveExpense(_expenseItem);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.SaveExpenseItem()");
            }
        }  

        // dch rkl 10/14/2016 Delete Expensen
        public void DeleteExpenseItem()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.DeleteExpense(_expenseItem);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.DeleteExpenseItem()");
            }
        }
    }
}
