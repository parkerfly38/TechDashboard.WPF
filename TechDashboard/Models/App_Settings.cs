using System;
using System.Collections.Generic;
using System.Text;
using TechDashboard.Data;

using SQLite;

namespace TechDashboard.Models
{
    public class App_Settings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public bool IsUsingHttps { get; set; }

        public ConnectionType ErpConnectionType { get; set; }

        public string SDataUrl { get; set; }

        public string RestServiceUrl { get; set; }

        public string SDataUserId { get; set; }

        public string SDataPassword { get; set; } // puke... this shold be encrypted somehow

        public int ScheduleDaysBefore { get; set; }

        public int ScheduleDaysAfter { get; set; }

        public bool TwentyFourHourTime { get; set; }

        public string LoggedInTechnicianNo { get; set; }

        public string LoggedInTechnicianDeptNo { get; set; }

        public string DeviceName { get; set; }

        public string DeviceID { get; set; }

        public App_Settings()
        {
            // empty
        }
    }
}
