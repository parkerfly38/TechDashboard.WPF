using System.Collections.Generic;
using System.Linq;

using TechDashboard.Models;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region ERP CI_Item

        /// <summary>
        /// Retreives Item data from the ERP connection and uses
        /// it to fill the local CI_Item table.
        /// </summary>
        public void FillItemTable()
        {
            //FillLocalTable<CI_Item>();
            FillLocalTable<CI_Item>("where", "ItemType eq '1'");
        }

        /// <summary>
        /// Retrieves a list of all CI_Item objects from the local database table.
        /// </summary>
        /// <returns>A List container of CI_Item objects.</returns>
        public List<CI_Item> GetItemsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<CI_Item>().OrderBy(item => item.ItemCode).ToList();
            }
        }

        /// <summary>
        /// Retrieves a list of all CI_Item objects from the local database table
        /// which match the filter text provided.
        /// </summary>
        /// <param name="itemCodeFilterText">Filter text for item code</param>
        /// <param name="itemDescFilterText">Filter text for item description</param>
        /// <returns>A filtered List container of CI_Item objects.</returns>
        public List<CI_Item> GetItemsFromDB(string itemCodeFilterText, string itemDescFilterText)
        {
            lock (_locker)
            {
                if (((itemCodeFilterText != null) && (itemCodeFilterText.Length > 0)) &&
                   ((itemDescFilterText != null) && (itemDescFilterText.Length > 0)))
                {
                    return
                        _database.Table<CI_Item>().Where(
                            i => i.ItemCode.Contains(itemCodeFilterText) ||
                                 i.ItemCodeDesc.Contains(itemDescFilterText)
                        ).OrderBy(item => item.ItemCode).ToList();
                }
                else if ((itemCodeFilterText != null) && (itemCodeFilterText.Length > 0))
                {
                    return
                        _database.Table<CI_Item>().Where(
                            i => i.ItemCode.Contains(itemCodeFilterText)
                        ).OrderBy(item => item.ItemCode).ToList();
                }
                else if ((itemDescFilterText != null) && (itemDescFilterText.Length > 0))
                {
                    return
                        _database.Table<CI_Item>().Where(
                            i => i.ItemCodeDesc.Contains(itemDescFilterText)
                        ).OrderBy(item => item.ItemCode).ToList();
                }
                else
                {
                    return _database.Table<CI_Item>().OrderBy(item => item.ItemCode).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of all CI_Item objects from the local database table.
        /// </summary>
        /// <returns>A List container of CI_Item objects.</returns>
        public List<CI_Item> GetItemsFromDB(string itemType)
        {
            lock (_locker)
            {
                return _database.Table<CI_Item>().Where(item => item.ItemType == itemType).OrderBy(item => item.ItemCode).ToList();
            }
        }

        /// <summary>
        /// Retreives a specific item from the local database's CI_Item
        /// table using a given item code.
        /// </summary>
        /// <param name="itemCode">The item code</param>
        /// <returns>A CI_Item object for the given item code.</returns>
        public CI_Item GetItemFromDB(string itemCode)
        {
            lock (_locker)
            {
                return _database.Table<CI_Item>().Where(item => item.ItemCode == itemCode).FirstOrDefault();
            }
        }

        public CI_Item RetrieveRepairItemFromWorkTicket(App_WorkTicket workTicket)
        {
            CI_Item currentRepairItem = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    currentRepairItem =
                        _database.Table<CI_Item>().Where(
                            ri => ri.ItemCode == workTicket.DtlRepairItemCode
                        ).FirstOrDefault();
                }
            }

            return currentRepairItem;
        }

        public CI_Item RetrieveRepairItemFromWorkTicket(JT_WorkTicket workTicket)
        {
            CI_Item currentRepairItem = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    currentRepairItem =
                        _database.Table<CI_Item>().Where(
                            ri => ri.ItemCode == workTicket.DtlRepairItemCode
                        ).FirstOrDefault();
                }
            }

            return currentRepairItem;
        }

        /// <summary>
        /// Retrieves the CI_Item item object from the local database which 
        /// corresponds to the repair item on the current work ticket.
        /// </summary>
        /// <returns>The CI_Item object representing the repair item on the
        /// current work ticket.</returns>
        public CI_Item RetrieveRepairItemFromCurrentWorkTicket()
        {
            CI_Item currentRepairItem = null;

            lock (_locker)
            {
                JT_WorkTicket workTicket = RetrieveCurrentWorkTicket();

                if (workTicket != null)
                {
                    currentRepairItem =
                        _database.Table<CI_Item>().Where(
                            ri => ri.ItemCode == workTicket.DtlRepairItemCode
                        ).FirstOrDefault();
                }
            }

            return currentRepairItem;
        }

        #endregion

        #region ERP IM_Item

        public void FillItemCostTable()
        {
            FillLocalTable<IM_ItemCost>();
        }

        /// <summary>
        /// Retreives Item Warehouse data from the ERP connection and uses
        /// it to fill the local IM_ItemWarehouse table.
        /// </summary>
        public void FillItemWarehouseTable()
        {
            FillLocalTable<IM_ItemWarehouse>();
        }

        public IM_ItemWarehouse GetItemWarehouseFromDB(string itemCode, string warehouseCode)
        {
            lock (_locker)
            {
                return
                    _database.Table<IM_ItemWarehouse>().Where(
                        iw => (iw.ItemCode == itemCode) &&
                              (iw.WarehouseCode == warehouseCode)
                    ).FirstOrDefault();
            }
        }

        public List<IM_ItemWarehouse> GetItemsForWarehouse(string warehouseCode)
        {
            lock (_locker)
            {
                return
                    _database.Table<IM_ItemWarehouse>().Where(
                        iw => (iw.WarehouseCode == warehouseCode)
                    ).ToList();
            }
        }

        public List<IM_ItemWarehouse> GetWarehousesForItem(string itemCode)
        {
            lock (_locker)
            {
                return
                    _database.Table<IM_ItemWarehouse>().Where(
                        iw => (iw.ItemCode == itemCode)
                    ).ToList();
            }
        }

        #endregion

        #region ERP JT_EquipmentAsset

        public void FillEquipmentAssetTable()
        {
            FillLocalTable<JT_EquipmentAsset>();  
        }

        protected JT_EquipmentAsset GetEquipmentAsset(string itemCode, string manufacturerSerialNumber)
        {
            JT_EquipmentAsset equipmentAsset =
                _database.Table<JT_EquipmentAsset>().Where(
                    ea => (ea.ItemCode == itemCode) &&
                          (ea.MfgSerialNo == manufacturerSerialNumber)
                ).FirstOrDefault();

            return equipmentAsset;
        }

        protected List<JT_EquipmentAsset> GetEquipmentAssetsForItem(string itemCode)
        {
            List<JT_EquipmentAsset> equipmentAsset =
                _database.Table<JT_EquipmentAsset>().Where(
                    ea => (ea.ItemCode == itemCode)
                ).ToList();

            return equipmentAsset;
        }

        public List<string> GetMfgSerialNumbersForItem(string itemCode, string wareHouseCode)
        {
            //List<string> mfgSeialNumberList =
            //    _database.Table<JT_EquipmentAsset>().Where(
            //        ea => (ea.ItemCode == itemCode)
            //    ).GroupBy(t => t.MfgSerialNo).Select(group => group.First().MfgSerialNo).ToList();

            List<string> serialNumbers = _database.Table<IM_ItemCost>().Where(
                    x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode)
                    .GroupBy(y => y.LotSerialNo).Select(group => group.First().LotSerialNo).ToList();

            if (serialNumbers.Count() == 0)
            {
                serialNumbers = _database.Table<JT_EquipmentAsset>().Where(
                    ea => (ea.ItemCode == itemCode)
                ).GroupBy(t => t.MfgSerialNo).Select(group => group.First().MfgSerialNo).ToList();
            }

            return serialNumbers;
        }

        public string GetQuantityOnHand(string itemCode, string wareHouseCode, string serialNumber)
        {
            IM_ItemCost quantityOnHand = _database.Table<IM_ItemCost>().Where(x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode && x.LotSerialNo == serialNumber).FirstOrDefault();
            if (quantityOnHand == null)
            {
                return "0";
            }
            return (quantityOnHand.QuantityOnHand - quantityOnHand.QuantityCommitted).ToString();
        }

        #endregion

        #region App_Item

        public App_Item GetItem(string itemCode, string warehouseCode)
        {
            return GetItem(itemCode, warehouseCode, null);
        }

        public App_Item GetItem(string itemCode, string warehouseCode, string manufacturerSerialNumber)
        {
            App_Item appItem = null;
            CI_Item item = null;
            IM_ItemWarehouse itemWarehouse = null;
            JT_EquipmentAsset equipmentAsset = null;

            lock (_locker)
            {
                item = GetItemFromDB(itemCode);
                itemWarehouse = GetItemWarehouseFromDB(itemCode, warehouseCode);                

                if ((item != null) && (itemWarehouse != null))
                {
                    if (manufacturerSerialNumber != null)
                    {
                        equipmentAsset = GetEquipmentAsset(item.ItemCode, manufacturerSerialNumber);
                    }

                    appItem = new App_Item(item, itemWarehouse, equipmentAsset);
                }
            }

            return appItem;
        }

        public List<App_Item> GetItems(string itemCodeFilterText, string itemDescFilterText)
        {
            List<App_Item> itemList = new List<App_Item>();

            lock (_locker)
            {
                foreach (CI_Item item in GetItemsFromDB(itemCodeFilterText, itemDescFilterText))
                {
                    foreach (IM_ItemWarehouse itemWarehouse in GetWarehousesForItem(item.ItemCode))
                    {
                        itemList.Add(new App_Item(item, itemWarehouse, null));
                    }
                }
            }

            return itemList;
        }

        public List<App_Item> GetItems()
        {
            return GetItems(null, null);
        }
        
        #endregion
    }
}
