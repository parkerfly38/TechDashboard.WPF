using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class JT_TimeTrackerOptions
    {
        /// <summary>
        /// Module Code - varchar(3)
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// Consider Sunday as Overtime Y/N - char(1)
        /// </summary>
        public string IsSunOvertime { get; set; }

        /// <summary>
        /// Minimum Hourly Increment for Cost - decimal(16,6)
        /// </summary>
        public decimal MinHourlyCostIncrement { get; set; }

        /// <summary>
        /// Overtime Earnings Code - varchar(2)
        /// </summary>
        public string OvertimeEarningsCode { get; set; }

        /// <summary>
        /// Overtime Hours In Day - decimal(16,6)
        /// </summary>
        public decimal OvertimeHoursInDay { get; set; }

        /// <summary>
        /// Overtime Hours In Week - decimal(16,6)
        /// </summary>
        public decimal OvertimeHoursInWeek { get; set; }

        /// <summary>
        /// Overtime Rules (E/R/B/N) - char(1)
        /// </summary>
        public string OvertimeRules { get; set; }

        /// <summary>
        /// Overtime Smoothing Option (S/E/C) - char(1)
        /// </summary>
        public string OvertimeSmoothingOption { get; set; }

        /// <summary>
        /// Post Overtime Method (D/P) - char(1)
        /// </summary>
        public string PostOvertimeMethod { get; set; }

        /// <summary>
        /// Set In/Out From Direct Time (Y/N) - char(1)
        /// </summary>
        public string SetInOutTimeFromDirectTime { get; set; }

        /// <summary>
        /// Stop At Earning Code In Job Dat (Y/N) - char(1)
        /// </summary>
        public string StopAtEarningCodeInJobDE { get; set; }

        /// <summary>
        /// Allow Editing Of Earnings Code (Y/N) - char(1)
        /// </summary>
        public string AllowEditingOfEarningsCode { get; set; }

        /// <summary>
        /// Stop Entry If Actual > Budget (Y/N) - char(1)
        /// </summary>
        public string StopEntryIfBudgetExceeded { get; set; }

        /// <summary>
        /// User Multiple Pay Rates (Y/N) - char(1)
        /// </summary>
        public string UseMultiplePayRates { get; set; }

        /// <summary>
        /// Labor Import From - varchar(40)
        /// </summary>
        public string LaborImportFrom { get; set; }

        /// <summary>
        /// Archive Labor (Y/N) - char(1)
        /// </summary>
        public string ArchiveLabor { get; set; }

        /// <summary>
        /// Labor Archive To - varchar(40)
        /// </summary>
        public string LaborArchiveTo { get; set; }

        /// <summary>
        /// Allow Regular Time Entry To Hist (Y/N) - char(1)
        /// </summary>
        public string AllowRegularTimeEntryToHistWT { get; set; }

        /// <summary>
        /// Always Stop At Activity Code (Y/N) - char(1)
        /// </summary>
        public string AlwaysStopAtActivityCode { get; set; }

        /// <summary>
        /// Always Stop At Status Code (Y/N) - char(1)
        /// </summary>
        public string AlwaysStopAtStatusCode { get; set; }

        /// <summary>
        /// Automatic Release Status - varchar(3)
        /// </summary>
        public string AutomaticReleaseStatus { get; set; }

        /// <summary>
        /// Capture Time In Time Tracker (Y/N) - char(1)
        /// </summary>
        public string CaptureTimeInTimeTracker { get; set; }

        /// <summary>
        /// Extended Work Text Option (Y/N) - char(1)
        /// </summary>
        public string ExtendedWorkTextOption { get; set; }

        /// <summary>
        /// Consider Saturday as Overtime (Y/N) - char(1)
        /// </summary>
        public string IsSatOvertime { get; set; }

        /// <summary>
        /// Scrap Misc Item Code - varchar(30)
        /// </summary>
        public string ScrapItemCode { get; set; }

        /// <summary>
        /// Dilute Overlapping Time Entries (Y/N) - char(1)
        /// </summary>
        public string DiluteOverlappingTime { get; set; }

        /// <summary>
        /// Allow Entry of Overlapping Time (Y/N) - char(1)
        /// </summary>
        public string AllowOverlappingTime { get; set; }
    }
}
