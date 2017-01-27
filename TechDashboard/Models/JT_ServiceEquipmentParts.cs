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
     * 12/02/2016 DCH Inherit from Rkl.Erp.Sage.Sage100.TableObjects.JT_ServiceEquipmentParts instead of having duplicate class
     *********************************************************************************************************/

    // dch rkl 12/05/2016 Inherit from Rkl.Erp.Sage.Sage100.TableObjects.JT_ServiceEquipmentParts instead of having duplicate class
    //public class JT_ServiceEquipmentParts
    public class JT_ServiceEquipmentParts : Rkl.Erp.Sage.Sage100.TableObjects.JT_ServiceEquipmentParts
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Item Code (parent) - varchar(30)
        /// </summary>
        //public string ItemCode { get; set; }

        ///// <summary>
        ///// Item Code - varchar(30)
        ///// </summary>
        //public string PartItemCode { get; set; }

        ///// <summary>
        ///// Problem Code - varchar(10)
        ///// </summary>
        //public string ProblemCode { get; set; }

        ///// <summary>
        ///// Quantity - TODO added
        ///// </summary>
        //public double Quantity { get; set; }

        /// <summary>
        /// Unit Of Measure - TODO added
        /// </summary>
        //public string UnitOfMeasure { get; set; }

        ///// <summary>
        ///// Comment  - TODO added
        ///// </summary>
        //public string Comment { get; set; }

        /// <summary>
        /// Is Chargeable - TODO
        /// </summary>
        public bool IsChargeable { get; set; }

        /// <summary>
        /// Is Printable - TODO
        /// </summary>
        public bool IsPrintable { get; set; }

        /// <summary>
        /// Is Purchased - TODO
        /// </summary>
        public bool IsPurchased { get; set; }

        /// <summary>
        /// Is Chargeable - TODO
        /// </summary>
        public bool IsOverhead { get; set; }

    }
}
