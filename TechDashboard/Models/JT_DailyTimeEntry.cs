using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models 
{
    /*********************************************************************************************************
     * JT_DailyTimeEntry.cs
     * 12/02/2016 DCH Add TODO, Correct spelling of GetApplicationSettings
     *********************************************************************************************************/
    public class JT_DailyTimeEntry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Department Number - varchar(2)
        /// </summary>
        public string DepartmentNo { get; set; }

        /// <summary>
        /// Employee Number - varchar(7)
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Service Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Service Ticket Step - varchar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Transaction Date - date
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Start Time - varchar(4)
        /// </summary>
        public string StartTime { get; set; }  

        // dch rkl 10/26/2016
        public string FormattedStartTime
        {
            get
            {
                return FormattedTime(StartTime);
            }
        }

        /// <summary>
        /// End Time - varchar(4)
        /// </summary>
        public string EndTime { get; set; }

        // dch rkl 10/26/2016
        public string FormattedEndTime
        {
            get
            {
                return FormattedTime(EndTime);
            }
        }

        /// <summary>
        /// Earnings Code - varchar
        /// </summary>
        public string EarningsCode { get; set; }

        // dch rkl 10/26/2016 format the time - Note: This logic should be added to the JT_Technician model
        private string FormattedTime(string sTimeIn)
        {
            if (sTimeIn == null) { sTimeIn = ""; }

            string sTimeOut = sTimeIn;

            string sHour = "";
            string sMin = "";
            string sAMorPM = "";
            int iHour = 0;

            if (sTimeIn.Length == 4)
            {
                sHour = sTimeIn.Substring(0, 2);
                sMin = sTimeIn.Substring(2, 2);
            }
            else if (sTimeIn.Length == 3)
            {
                sHour = "0" + sTimeIn.Substring(0, 1);
                sMin = sTimeIn.Substring(1, 2);
            }

            int.TryParse(sHour, out iHour);
            if (iHour > 0)
            {
                if (iHour < 12) { sAMorPM = "AM"; }
                else { sAMorPM = "PM"; }

                App_Settings appSettings = App.Database.GetApplicationSettings();
                if (appSettings.TwentyFourHourTime == false && iHour > 12)
                {
                    iHour = iHour - 12;
                    sHour = iHour.ToString();
                    if (sHour.Length == 1) { sHour = "0" + sHour; }
                }

                sTimeOut = string.Format("{0}:{1} {2}", sHour, sMin, sAMorPM);
            }

            return sTimeOut;
        }
    }
}
