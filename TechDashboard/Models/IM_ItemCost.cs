using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rkl.Erp.Sage.Sage100.TableObjects;

namespace TechDashboard.Models
{
    public class IM_ItemCost
    {
        public string ItemCode { get; set; }

        public string WarehouseCode { get; set; }

        public string TierType { get; set; }

        public string GroupSort { get; set; }

        public string TransactionDate { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptNo { get; set; }

        public string LotSerialNo { get; set; }

        public string NegativeQty { get; set; }

        public string TierGroup { get; set; }

        public decimal? QuantityOnHand { get; set; }

        public decimal? UnitCost { get; set; }

        public decimal? AllocatedCost { get; set; }

        public decimal? ExtendedCost { get; set; }

        public decimal? QuantityCommitted { get; set; }

        public decimal? CostCalcQtyCommitted { get; set; }

        public decimal? CostCalcCostCommitted { get; set; }

        public string DateCreated { get; set; }

        public string TimeCreated { get; set; }

        public string UserCreatedKey { get; set; }

        public string DateUpdated { get; set; }

        public string TimeUpdated { get; set; }

        public string UserUpdatedKey { get; set; }
    }
}
