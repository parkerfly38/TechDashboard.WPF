using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class ExpensesEditPageViewModel
    {
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

        protected void UpdateExpenseCategory(string category)
        {
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

        public ExpensesEditPageViewModel(App_WorkTicket workTicket)
        {
            _expenseItem = new App_Expense(new JT_TransactionImportDetail(), workTicket);
            
            Initialize();
        }

        public ExpensesEditPageViewModel(App_Expense expenseItem)
        {
            _expenseItem = expenseItem;
            Initialize();
        }

        protected void Initialize()
        {
            _expenseCategories = new ObservableCollection<string>(App.Database.GetExpenseCategories());
            _expenseChargeCodes = new ObservableCollection<string>(App.Database.GetExpenseChargeCodes());
        }

        public void SaveExpenseItem()
        {
            App.Database.SaveExpense(_expenseItem); 
        }  
    }
}
