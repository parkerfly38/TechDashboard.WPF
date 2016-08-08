﻿using System;
using TechDashboard.Data;
using TechDashboard.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TechDashboard.ViewModels
{
	public class MiscellaneousTimePageViewModel
    {
		protected App_Technician _appTechnician;
		public App_Technician AppTechnician {
			get {
				return _appTechnician;
			}
		}

		private List<JT_EarningsCode> _jtearningscode;
		public List<JT_EarningsCode> EarningsCode {
			get {
				return _jtearningscode;
			}
		}

		private List<JT_DailyTimeEntry> _timeEntries;
		public List<JT_DailyTimeEntry> TimeEntries
		{
			get {
				return _timeEntries;
			}
            set {
                _timeEntries = value;
            }
		}

		private JT_DailyTimeEntry _dailyTimeEntry;
		public JT_DailyTimeEntry DailyTimeEntry {
			get { return _dailyTimeEntry; }
			set { _dailyTimeEntry = value; }
		}

		public MiscellaneousTimePageViewModel ()
		{
			_appTechnician = new App_Technician(App.Database.GetCurrentTechnicianFromDb());
			_jtearningscode = App.Database.GetEarningsCodesFromDBforMisc();
			_timeEntries = App.Database.GetTimeEntriesByTech(_appTechnician.TechnicianNo);

		}
		public void SaveDailyTimeEntry(double hoursWorked)
		{
			App.Database.SaveMiscellaneousTimeEntry(_dailyTimeEntry, hoursWorked);
            TimeEntries = App.Database.GetTimeEntriesByTech(_appTechnician.TechnicianNo);
        }
        

    }
}
