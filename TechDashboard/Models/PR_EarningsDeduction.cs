using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class PR_EarningsDeduction

    {
        /// <summary>
        /// Record Type char(1) : E=Earnings, D=Deductions
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Deduction Code varchar(2) 
        /// </summary>
        public string DeductionCode { get; set; }

        /// <summary>
        /// Earnings Code varchar(2) 
        /// </summary>
        public string EarningsCode { get; set; }

        /// <summary>
        /// Earnings Deduction Description varchar(30) 
        /// </summary>
        public string EarningsDeductionDesc { get; set; }

        /// <summary>
        /// Expense Account Key (9) 
        /// </summary>
        public string ExpenseAcctKey { get; set; }

        /// <summary>
        /// Type of Earnings (1) : Valid:R,S,O,V,B,M,X,F
        /// </summary>
        public string TypeOfEarnings { get; set; }

        /// <summary>
        /// Method of Entry (1) :  Valid:S,F,A
        /// </summary>
        public string MethodOfEntry { get; set; }

        /// <summary>
        /// Use Pay Rate 1 or 2 : Valid:1,2
        /// </summary>
        public string UsePayRate1Or2 { get; set; }

        /// <summary>
        /// Post Earnings Expense By Depart varchar(1) : Y, N 
        /// </summary>
        public string PostEarningsExpenseByDept { get; set; }

        /// <summary>
        /// Subject to Federal Withholding varchar(1) : Y, N 
        /// </summary>
        public string ECSubjectToFedWithholding { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToEmployeeFica { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToEmployerFica { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToFedUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToStateWithholding { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToStateSdi { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToStateUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToLocalWithholding { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToLocalLdi { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToLocalUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToEmployeeMedicare { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToEmployerMedicare { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECBenefitTypesToAccrue { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToOtherLocal { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjectToEmployeeSui { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string ECSubjToWorkersCompensation { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DeductionAcctKey { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string CalculationMethod { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string EqualToEarningsCode1 { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string EqualToEarningsCode2 { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string EqualToEarningsCode3 { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string FrequencyOfDeduction { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string AutoEarningsAsEarningsCode { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DeductionType { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string AccrualAcctKeyForContribution { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToFedWithholding { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToEmployeeFica { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToEmployerFica { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToFederalUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToStateSdi { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToStateUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToLocalWithholding { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToLocalLdi { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToLocalUnemployment { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToEmployeeMedicare { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToEmployerMedicare { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToOtherLocal { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCSubjectToEmployeeSui { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string W2Label { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string RetainResetAtEndOfYear { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string StandardAmount { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string PayRateMultiplier { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DCStdAmountOrRate { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string StandardLimit { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DateCreated { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string TimeCreated { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string UserCreatedKey { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string DateUpdated { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string TimeUpdated { get; set; }

        /// <summary>
        /// Record Type varchar(2) 
        /// </summary>
        public string UserUpdatedKey { get; set; }
    }
}
