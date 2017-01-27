using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    // dch rkl 12/01/2016 Add IM_Warehouse
    public class IM_Warehouse
    {
        /// <summary>
        /// Warehouse Code - varchar(3)
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Warehouse Descr - varchar(30)
        /// </summary>
        public string WarehouseDesc { get; set; }

        public IM_Warehouse()
        {
            WarehouseDesc = "";
            WarehouseCode = "";
        }
    }
}
