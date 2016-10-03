using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
	public partial class TechDashboardDatabase
	{
		public void SaveMiscellaneousTimeEntry(JT_DailyTimeEntry dailyTimeEntry, double hoursWorked)
		{
            JT_TransactionImportDetail transactionDetail = new JT_TransactionImportDetail()
            {
                RecordType = "L",
                SalesOrderNo = dailyTimeEntry.SalesOrderNo,
                WTNumber = dailyTimeEntry.WTNumber,
                WTStep = dailyTimeEntry.WTStep,
                EmployeeDeptNo = dailyTimeEntry.DepartmentNo,
                EmployeeNo = dailyTimeEntry.EmployeeNo,
                EarningsCode = dailyTimeEntry.EarningsCode,
                StartTime = dailyTimeEntry.StartTime,
                EndTime = dailyTimeEntry.EndTime,
                HoursWorked = hoursWorked
            };
            _database.Insert(transactionDetail);
			_database.Insert(dailyTimeEntry);
		}

		public List<JT_DailyTimeEntry> GetTimeEntriesByTech(string employeeNo)
		{
			return _database.Table<JT_DailyTimeEntry>().Where(x => x.EmployeeNo == employeeNo).ToList();
		}
	}
}

