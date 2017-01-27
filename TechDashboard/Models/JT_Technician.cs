using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * JT_ServiceEquipmentParts.cs
     * 12/02/2016 DCH Add TODO
     *********************************************************************************************************/
    public class JT_Technician : Rkl.Erp.Sage.Sage100.TableObjects.JT_Technician
    {
        [PrimaryKeyAttribute, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}
