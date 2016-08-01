using System;
using System.Collections.Generic;
using System.Text;

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

        public App_Settings()
        {
            // empty
        }
    }
}
