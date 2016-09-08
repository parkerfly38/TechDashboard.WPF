using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Sage.SData.Client;
using TechDashboard.Models;
using TechDashboard.Data;
using TechDashboard.Services;

namespace TechDashboard.ViewModels
{
	public class ScheduleDetailPageViewModel
	{
		protected App_ScheduledAppointment _scheduleDetail;
		public App_ScheduledAppointment ScheduleDetail
		{
			get { return _scheduleDetail;}
		}

		protected JT_TechnicianScheduleDetail _technicianScheduleDetail;
		public JT_TechnicianScheduleDetail TechnicianScheduleDetail {
			get { return _technicianScheduleDetail; }
		}

		protected JT_DailyTimeEntry _timeEntryDetail;
		public JT_DailyTimeEntry TimeEntryDetail {
			get { return _timeEntryDetail; }
		}

        //protected JT_TransactionImportDetail _timeEntryActual;
        //public JT

        protected JT_TransactionImportDetail _timportDetail;
        public JT_TransactionImportDetail ImportDetail
        {
            get { return _timportDetail;  }
        }

		public ScheduleDetailPageViewModel(App_ScheduledAppointment scheduleDetail)
		{
			_scheduleDetail = scheduleDetail;
			_technicianScheduleDetail = App.Database.GetTechnicianScheduleDetailFromDB().Where(x => x.WTNumber == _scheduleDetail.WorkTicketNumber
				&& x.WTStep == _scheduleDetail.WorkTicketStep).FirstOrDefault();
			_timeEntryDetail = App.Database.GetTimeEntryData(scheduleDetail);
            _timportDetail = App.Database.GetCurrentExport().Where(x => x.RecordType == "L" && x.WTNumber == _scheduleDetail.WorkTicketNumber
                && x.WTStep == _scheduleDetail.WorkTicketStep && x.SalesOrderNo == _scheduleDetail.SalesOrderNumber).FirstOrDefault();
		}
	}
}

