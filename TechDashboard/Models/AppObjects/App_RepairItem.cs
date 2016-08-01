using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_RepairItem
    {
        private CI_Item _repairItem;
        private JT_EquipmentAsset _equipmentAsset;

        public string ItemCode
        {
            get { return _repairItem.ItemCode; }
        }

        public string ItemCodeDescription
        {
            get { return _repairItem.ItemCodeDesc; }
        }

        public decimal StandardUnitCost
        {
            get { return _repairItem.StandardUnitCost; }
        }

        public decimal StandardUnitPrice
        {
            get { return (decimal)0; }
        }

        public DateTime MfgPartsWarrantyPeriod
        {
            get { return _equipmentAsset.MfgPartsWarrantyPeriod; }
        }

        public DateTime MfgLaborWarrantyPeriod
        {
            get { return _equipmentAsset.MfgLaborWarrantyPeriod; }
        }

        /// <summary>
        /// Flag denoting if this item is an equipment asset 
        /// (i.e. has a corresponding JT_EquipmentAsset record).
        /// </summary>
        public bool IsEquipmentAsset
        {
            get { return (_equipmentAsset != null); }
        }

        public App_RepairItem(CI_Item repairItem, JT_EquipmentAsset equipmentAsset)
        {
            _repairItem = repairItem;
            _equipmentAsset = equipmentAsset;
        }
    }
}
