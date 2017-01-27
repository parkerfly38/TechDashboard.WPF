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
		public void SaveMiscellaneousTimeEntry(JT_DailyTimeEntry dailyTimeEntry, double hoursWorked)
		{
            JT_TransactionImportDetail transactionDetail = new JT_TransactionImportDetail()
            {
                // dch rkl 11/01/2016 per Jeanne, this should be an "M"
                //RecordType = "L",
                RecordType = "M",

                SalesOrderNo = dailyTimeEntry.SalesOrderNo,
                WTNumber = dailyTimeEntry.WTNumber,
                WTStep = dailyTimeEntry.WTStep,
                EmployeeDeptNo = dailyTimeEntry.DepartmentNo,
                EmployeeNo = dailyTimeEntry.EmployeeNo,
                EarningsCode = dailyTimeEntry.EarningsCode,
                StartTime = dailyTimeEntry.StartTime,
                EndTime = dailyTimeEntry.EndTime,
                // dch rkl 11/01/2016 Transaction Date should come from the entry screen BEGIN
                //TransactionDate = DateTime.Now.ToSage100DateString(),
                //TransactionDateAsDateTime = DateTime.Now,
                TransactionDate = dailyTimeEntry.TransactionDate.ToSage100DateString(),
                TransactionDateAsDateTime = dailyTimeEntry.TransactionDate,
                // dch rkl 11/01/2016 Transaction Date should come from the entry screen END
                HoursWorked = hoursWorked
            };
            _database.Insert(transactionDetail);
			_database.Insert(dailyTimeEntry);
		}

		public List<JT_DailyTimeEntry> GetTimeEntriesByTech(string employeeNo)
		{
            // dch rkl 10/18/2016 This should only display misc time in the JT_TransactionImportDetail table
            List<JT_DailyTimeEntry> lsTimeEntry = new List<Models.JT_DailyTimeEntry>();
            List<JT_TransactionImportDetail> lsTransDetl = _database.Table<JT_TransactionImportDetail>().Where(x => x.EmployeeNo == employeeNo && x.RecordType == "L").ToList();
            foreach(JT_TransactionImportDetail detl in lsTransDetl)
            {
                JT_DailyTimeEntry timeEntry = new Models.JT_DailyTimeEntry();
                timeEntry.ID = detl.ID;
                timeEntry.IsModified = false;
                timeEntry.DepartmentNo = detl.EmployeeDeptNo;
                timeEntry.EmployeeNo = detl.EmployeeNo;
                timeEntry.SalesOrderNo = detl.SalesOrderNo;
                timeEntry.WTNumber = detl.WTNumber;
                timeEntry.WTStep = detl.WTStep;
                timeEntry.TransactionDate = detl.TransactionDateAsDateTime;
                timeEntry.StartTime = detl.StartTime;
                timeEntry.EndTime = detl.EndTime;
                timeEntry.EarningsCode = detl.EarningsCode;
                lsTimeEntry.Add(timeEntry);
            }
            return lsTimeEntry;
            //return _database.Table<JT_DailyTimeEntry>().Where(x => x.EmployeeNo == employeeNo).ToList();
		}
	}
}

