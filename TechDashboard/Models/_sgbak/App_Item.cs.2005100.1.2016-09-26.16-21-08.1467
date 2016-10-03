using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_Item
    {
        protected CI_Item _item;
        protected IM_ItemWarehouse _itemWarehouse;
        protected JT_EquipmentAsset _equipmentAsset;

        /// <summary>
        /// Item Code - varchar(30)
        /// </summary>
        public string ItemCode
        {
            get { return _item.ItemCode; }
        }

        /// <summary>
        /// Item Type - varchar(1)
        /// </summary>
        public string ItemType
        {
            get { return _item.ItemType; }
        }

        /// <summary>
        /// Item Code Description - varchar(30)
        /// </summary>
        public string ItemCodeDesc
        {
            get { return _item.ItemCodeDesc; }
        }

        /// <summary>
        /// Default Warehouse Code = varchar(3)
        /// </summary>
        public string DefaultWarehouseCode
        {
            get { return _item.DefaultWarehouseCode; }
        }

        /// <summary>
        /// Standard Unit of Measure - varchar(4)
        /// </summary>
        public string StandardUnitOfMeasure
        {
            get { return _item.StandardUnitOfMeasure; }
        }

        /// <summary>
        /// Sales Unit Of Measure - varchar(4)
        /// </summary>
        public string SalesUnitOfMeasure
        {
            get { return _item.SalesUnitOfMeasure; }
        }

        /// <summary>
        /// Standard Unit Cost - numeric(15, 6)
        /// </summary>
        public decimal StandardUnitCost
        {
            get { return _item.StandardUnitCost; }
        }

        /// <summary>
        /// Standard Unit Price - numeric(15,6)
        /// </summary>
        public decimal StandardUnitPrice
        {
            get { return _item.StandardUnitPrice; }
        }

        /// <summary>
        /// Valuation - varchar(1)
        /// </summary>
        public string Valuation
        {
            get { return _item.Valuation; }
        }

        /// <summary>
        /// Warehouse Code - varchar(3)
        /// </summary>
        public string WarehouseCode
        {
            get { return _itemWarehouse.WarehouseCode; }
        }

        /// <summary>
        /// Quantity on Hand - numeric(15, 6)
        /// </summary>
        public decimal QuantityOnHand
        {
            get { return _itemWarehouse.QuantityOnHand; }
        }

        /// <summary>
        /// Quantity on Sales Order - numeric(15,6)
        /// </summary>
        public decimal QuantityOnSalesOrder
        {
            get { return _itemWarehouse.QuantityOnSalesOrder; }
        }

        /// <summary>
        /// Quantity On Back Order - numeric(15,6)
        /// </summary>
        public decimal QuantityOnBackOrder
        {
            get { return _itemWarehouse.QuantityOnBackOrder; }
        }

        /// <summary>
        /// Quantity Available
        /// </summary>
        public decimal QuantityAvailable
        {
            get { return (QuantityOnHand - QuantityOnSalesOrder - QuantityOnBackOrder); }
        }

        /// <summary>
        /// Manufacturer's Serial Number
        /// </summary>
        public string MfgSerialNo
        {
            get
            {
                if (_equipmentAsset == null)
                {
                    return null;
                }
                else
                {
                    return _equipmentAsset.MfgSerialNo;
                }
            }
        }

        /// <summary>
        /// Whse / On Hand / Available
        /// </summary>
        public string WhseOnHandAvail
        {
            get
            {
                string sQOH = QuantityOnHand.ToString();
                string sQAvl = QuantityAvailable.ToString();
                string sDetails = string.Format("Whse: {0}   On Hand: {1}   Available: {2}", WarehouseCode, sQOH, sQAvl);
                return sDetails;
            }
        }

        /// <summary>
        /// Flag denoting if this item is an equipment asset 
        /// (i.e. has a corresponding JT_EquipmentAsset record).
        /// </summary>
        public bool IsEquipmentAsset
        {
            get { return (_equipmentAsset != null); }
        }

        public App_Item(CI_Item item, IM_ItemWarehouse itemWarehouse, JT_EquipmentAsset equipmentAsset)
        {
            _item = item;
            _itemWarehouse = itemWarehouse;
            _equipmentAsset = equipmentAsset;
        }
    }
}
