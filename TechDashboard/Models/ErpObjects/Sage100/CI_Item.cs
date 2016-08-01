using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class CI_Item
    {
        /// <summary>
        /// Item Code - varchar(30)
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Item Type - varchar(1)
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Item Code Description - varchar(30)
        /// </summary>
        public string ItemCodeDesc { get; set; }

        /// <summary>
        /// Default Warehouse Code = varchar(3)
        /// </summary>
        public string DefaultWarehouseCode { get; set; }

        /// <summary>
        /// Standard Unit of Measure - varchar(4)
        /// </summary>
        public string StandardUnitOfMeasure { get; set; }

        /// <summary>
        /// Sales Unit Of Measure - varchar(4)
        /// </summary>
        public string SalesUnitOfMeasure { get; set; }

        /// <summary>
        /// Standard Unit Cost - numeric(15, 6)
        /// </summary>
        public decimal StandardUnitCost { get; set; }

        /// <summary>
        /// Standard Unit Price - numeric(15,6)
        /// </summary>
        public decimal StandardUnitPrice { get; set; }

        /// <summary>
        /// Valuation - varchar(1)
        /// </summary>
        public string Valuation { get; set; }
    }
}
