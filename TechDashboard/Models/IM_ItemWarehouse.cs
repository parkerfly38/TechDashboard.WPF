using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * IM_ItemWarehouse.cs
     * 12/05/2016 DCH Inherit from Rkl.Erp.Sage.Sage100.TableObjects.IM_ItemWarehouse instead of having duplicate class
     *********************************************************************************************************/
    // dch rkl 12/05/2016 Inherit from Rkl.Erp.Sage.Sage100.TableObjects.IM_ItemWarehouse instead of having duplicate class
    //public class IM_ItemWarehouse
    public class IM_ItemWarehouse : Rkl.Erp.Sage.Sage100.TableObjects.IM_ItemWarehouse
    {
        ///// <summary>
        ///// Item Code - varchar(30)
        ///// </summary>
        //public string ItemCode { get; set; }

        ///// <summary>
        ///// Warehouse Code - varchar(3)
        ///// </summary>
        //public string WarehouseCode { get; set; }

        ///// <summary>
        ///// Quantity on Hand - numeric(15, 6)
        ///// </summary>
        //public decimal QuantityOnHand { get; set; }

        ///// <summary>
        ///// Quantity on Sales Order - numeric(15,6)
        ///// </summary>
        //public decimal QuantityOnSalesOrder { get; set; }

        ///// <summary>
        ///// Quantity On Back Order - numeric(15,6)
        ///// </summary>
        //public decimal QuantityOnBackOrder { get; set; }

        // dch rkl 11/02/2016 add constructors to initialize values BEGIN
        public IM_ItemWarehouse()
        {
            ItemCode = "";
            WarehouseCode = "";
            QuantityOnHand = 0;
            QuantityOnSalesOrder = 0;
            QuantityOnBackOrder = 0;
            AverageCost = 0;        // dch rkl 12/05/2016
        }

        public IM_ItemWarehouse(string itemCode)
        {
            ItemCode = itemCode;
            WarehouseCode = "";
            QuantityOnHand = 0;
            QuantityOnSalesOrder = 0;
            QuantityOnBackOrder = 0;
            AverageCost = 0;        // dch rkl 12/05/2016
        }
        // dch rkl 11/02/2016 add constructors to initialize values BEGIN
    }
}
