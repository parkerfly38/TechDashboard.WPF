using System.Runtime.Serialization;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * JT_WorkTicketText.cs
     * 12/02/2016 DCH Inherit from Rkl.Erp.Sage.Sage100.TableObjects.SO_SalesOrderDetail instead of having duplicate class
     *********************************************************************************************************/

    // dch rkl 12/02/2016 Inherit from Rkl.Erp.Sage.Sage100.TableObjects.SO_SalesOrderDetail instead of having duplicate class
    //public class SO_SalesOrderDetail
    public class SO_SalesOrderDetail : Rkl.Erp.Sage.Sage100.TableObjects.SO_SalesOrderDetail
    {
        //public string SalesOrderNo { get; set; }

        //public string LineKey { get; set; }

        //public string LineSeqNo { get; set; }

        //public string ItemCode { get; set; }

        //public string ItemType { get; set; }

        //public string ItemCodeDesc { get; set; }

        //public string ExtendedDescriptionKey { get; set; }

        //public string Discount { get; set; }

        //public string WarehouseCode { get; set; }

        //public string Valuation { get; set; }

        //public string JT158_WTPart { get; set; }

        //public string JT158_WTNumber { get; set; }

        //public string JT158_WTStep { get; set; }

        //// dch rkl 11/23/2016 Add U/M
        //public string UnitOfMeasure { get; set; }

        //// dch rkl 11/30/2016 Add Comment Text
        //public string CommentText { get; set; }

        //// dch rkl 11/30/2016 Add Unit Price
        //public decimal UnitPrice { get; set; }

        //// dch rkl 11/30/2016 Add Unit Cost
        //public decimal UnitCost { get; set; }

        //// dch rkl 11/30/2016 Add Qty Ordered
        //public decimal QuantityOrdered { get; set; }
    }
}