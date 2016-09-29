using System.Runtime.Serialization;

namespace TechDashboard.Models
{
    public class SO_SalesOrderDetail
    {
        public string SalesOrderNo { get; set; }

        public string LineKey { get; set; }

        public string LineSeqNo { get; set; }

        public string ItemCode { get; set; }

        public string ItemType { get; set; }

        public string ItemCodeDesc { get; set; }

        public string ExtendedDescriptionKey { get; set; }

        public string Discount { get; set; }

        public string WarehouseCode { get; set; }

        public string Valuation { get; set; }

        public string JT158_WTPart { get; set; }

        public string JT158_WTNumber { get; set; }

        public string JT158_WTStep { get; set; }

    }
}