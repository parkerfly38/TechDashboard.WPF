using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using SQLite;

namespace TechDashboard.Models
{
    public class App_Expense : INotifyPropertyChanged
    {
        protected int _id;
        App_WorkTicket _workTicket;

        protected DateTime _date;
        protected string _category;
        protected string _chargeCode;
        protected string _billingDescription;
        protected string _unitOfMeasure;
        protected double _quantity;
        protected double _unitPrice;
        protected double _unitCost;
        protected bool _isReimbursable;
        protected bool _isChargeableToCustomer;
        protected double _markupPercentage;

        public int ID
        {
            get { return _id; }
            //set { _id = value; }
        }

        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value.Date; }
        }

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public string ChargeCode
        {
            get { return _chargeCode; }
            set
            {
                _chargeCode = value;
                OnPropertyChanged("ChargeCode");
            }
        }

        public string BillingDescription
        {
            get { return _billingDescription; }
            set { _billingDescription = value; }
        }

        public string UnitOfMeasure
        {
            get { return _unitOfMeasure; }
            set { _unitOfMeasure = value; }
        }

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public double UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }

        public bool IsReimbursable
        {
            get { return _isReimbursable; }
            set { _isReimbursable = value; }
        }

        public bool IsChargeableToCustomer
        {
            get { return _isChargeableToCustomer; }
            set { _isChargeableToCustomer = value; }
        }

        public double MarkupPercentage
        {
            get { return _markupPercentage; }
            set
            {
                _markupPercentage = value;
            }
        }




        public App_Expense(JT_TransactionImportDetail importDetail, App_WorkTicket workTicket)
        {
            _id = importDetail.ID;
            _workTicket = workTicket;
            if (_id == 0)
            {
                Date = System.DateTime.Now.Date;
            }
            else
            {
                Date = importDetail.TransactionDateAsDateTime;
            }
            Category = null;
            BillingDescription = importDetail.BillingDescription;
            ChargeCode = importDetail.ItemCode;
            Quantity = importDetail.QuantityUsed;
            UnitOfMeasure = importDetail.UnitOfMeasure;
            UnitPrice = importDetail.UnitPrice;
            UnitCost = importDetail.UnitCost;
            IsReimbursable = ((importDetail.ReimburseEmployee != null) && (importDetail.ReimburseEmployee.Trim().ToUpper() == "Y"));
            IsChargeableToCustomer = ((importDetail.ChargePart != null) && (importDetail.ChargePart.Trim().ToUpper() == "Y"));
        }

        public App_Expense(JT_MiscellaneousCodes miscCode, App_WorkTicket workTicket)
        {
            _id = 0;
            _workTicket = workTicket;

            Date = System.DateTime.Now.Date;
            Category = miscCode.MiscellaneousCode;
            BillingDescription = miscCode.Description;

            string[] brokenDescription = BreakDescription(miscCode.AddtlDescNum);
            if(brokenDescription != null)
            {
                ChargeCode = brokenDescription[0];
                Quantity = 0;
                UnitOfMeasure = brokenDescription[1];
                UnitPrice = double.Parse(brokenDescription[2]);
            }

            UnitCost = 0; // puke
            IsReimbursable = false;
            IsChargeableToCustomer = false;
        }

        /// <summary>
        /// Breaks apart (or unformats) any additional expense data kept in the 
        /// JT_MiscellaneousCodes.AddlDescNum field.
        /// </summary>
        /// <param name="addlDescNum">The JT_MiscellaneousCodes.AddlDescNum field
        /// to break out.</param>
        /// <returns>String array with the following indexes:
        /// 0 = Expense Category,
        /// 1 = Unit of Measure,
        /// 2 = Unit Price</returns>
        public static string[] BreakDescription(string addlDescNum)
        {
            string[] brokenDescription = null;
            char[] delimeters = new char[] { ';' };

            if (addlDescNum != null)
            {
                brokenDescription = addlDescNum.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
            }

            if ((brokenDescription == null) || (brokenDescription.Length != 3))
            {
                return null;
            }
            else
            {
                return brokenDescription;
            }
        }

        protected void SetChargeCode(string chargeCode)
        {
            _chargeCode = chargeCode;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
