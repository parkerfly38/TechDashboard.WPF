using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class IM_ItemWarehouse
    {
        /// <summary>
        /// Item Code - varchar(30)
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Warehouse Code - varchar(3)
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Quantity on Hand - numeric(15, 6)
        /// </summary>
        public decimal QuantityOnHand { get; set; }

        /// <summary>
        /// Quantity on Sales Order - numeric(15,6)
        /// </summary>
        public decimal QuantityOnSalesOrder { get; set; }

        /// <summary>
        /// Quantity On Back Order - numeric(15,6)
        /// </summary>
        public decimal QuantityOnBackOrder { get; set; }
    }
}
