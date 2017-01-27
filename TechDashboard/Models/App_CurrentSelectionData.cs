using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
// TODO... need this?
namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_CurrentSelectionData.cs
     * 12/01/2016 DCH Add TODO
     *********************************************************************************************************/
    public class App_CurrentSelectionData
    {
        [PrimaryKeyAttribute, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Department Nuamber - varchar(2)
        /// </summary>
        public string TechnicianDeptNo { get; set; }

        /// <summary>
        /// Technician Number - varchar(7)
        /// </summary>
        public string TechnicianNo { get; set; }

        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Work Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Work Ticket Step - varcar(3)
        /// </summary>
        public string WTStep { get; set; }
    }
}
