using System;
using System.Collections.Generic;
using System.Linq;

using TechDashboard.Models;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_Item.cs
     * 11/23/2016 DCH return list with quantity available and quantity on hand
     * 11/27/2016 DCH useinso must be "Y" to include items
     * 12/01/2016 DCH add warehouse filter to get items
     * 01/18/2017 DCH Deduct Qty already allocated to tickets for this part from Qty On Hand 
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region ERP CI_Item

        /// <summary>
        /// Retreives Item data from the ERP connection and uses
        /// it to fill the local CI_Item table.
        /// </summary>
        public void FillItemTable()
        {
            FillLocalTable<CI_Item>();
            //FillLocalTable<CI_Item>("where", "ItemType eq '1'");

            FillLocalTable<IM_ItemCost>();
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

        // dch rkl 11/23/2016 return list with quantity available  LotQavl
        public List<LotQavl> GetMfgSerialNumbersForItem(string itemCode, string wareHouseCode, string SalesOrderNo, string WTNumber, string WTStep)
        //public List<string> GetMfgSerialNumbersForItem(string itemCode, string wareHouseCode, string SalesOrderNo, string WTNumber, string WTStep)
        {
            // dch rkl 11/23/2016 return list with quantity available  LotQavl
            List<LotQavl> lsLotQavl = new List<Data.LotQavl>();

            //first let's get the sales order part valuation field vis a vis Chris' instructions
            // if it doesn't have one I'm defaulting to serial
            /*string ValuationField = _database.Table<SO_SalesOrderDetail>().Where(
                    x => x.SalesOrderNo == SalesOrderNo
                    && x.ItemCode == itemCode
                    && x.WarehouseCode == wareHouseCode
                    && x.JT158_WTNumber == WTNumber
                    && x.JT158_WTPart == "Y"
                    && x.JT158_WTStep == WTStep
                ).Select(x => x.Valuation).FirstOrDefault();

            //*/
            List<string> serialNumbers = new List<string>();

            serialNumbers = _database.Table<IM_ItemCost>().Where(
                x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode && (x.TierType == "3" || x.TierType == "4")
                        ).GroupBy(y => y.LotSerialNo).Select(group => group.First().LotSerialNo).ToList();

            //if (ValuationField == "5")
            //{
            serialNumbers = _database.Table<IM_ItemCost>().Where(
                    x => x.ItemCode == itemCode 
                    && x.WarehouseCode == wareHouseCode
                    && (x.TierType == "3" || x.TierType == "4")
                    ).GroupBy(y => y.LotSerialNo).Select(group => group.First().LotSerialNo).ToList();

            if (serialNumbers.Count() == 0)
            {
                serialNumbers = _database.Table<JT_EquipmentAsset>().Where(
                    ea => (ea.ItemCode == itemCode)
                ).GroupBy(t => t.MfgSerialNo).Select(group => group.First().MfgSerialNo).ToList();
            }

            //iterate over and remove negative quantity items
            for (int i = serialNumbers.Count - 1; i >= 0; i--)
            {
                // dch rkl 01/20/107 Get Qty Available, Qty On Hand and Unit Cost using single function
                double qoh = 0;
                double avail = 0;
                decimal cost = 0;
                GetQOHAvlCost(itemCode, wareHouseCode, serialNumbers[i], ref qoh, ref avail, ref cost);
                // dch rkl 11/23/2016 return list with quantity available  LotQavl
                //double qoh = (double)GetQuantityOnHandDecimal(itemCode, wareHouseCode, serialNumbers[i]);
                //double avail = (double)GetQuantityAvailDecimal(itemCode, wareHouseCode, serialNumbers[i]);
                if (avail <= 0)
                //if (GetQuantityOnHandDecimal(itemCode, wareHouseCode, serialNumbers[i]) <= 0)
                {
                    serialNumbers.Remove(serialNumbers[i]);
                }
                // dch rkl 11/23/2016 return list with quantity available  LotQavl
                else
                {
                    // dch rkl 01/20/2017 Include Unit Cost
                    //lsLotQavl.Add(new LotQavl(serialNumbers[i], avail, qoh));
                    lsLotQavl.Add(new LotQavl(serialNumbers[i], avail, qoh, cost));
                }
            }

            // dch rkl 11/23/2016 return list with quantity available  LotQavl
            return lsLotQavl;
            //return serialNumbers;
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

        public decimal GetQuantityOnHandDecimal(string itemCode, string wareHouseCode, string serialNumber)
        {
            IM_ItemCost quantityOnHand = _database.Table<IM_ItemCost>().Where(x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode && x.LotSerialNo == serialNumber).FirstOrDefault();
            if (quantityOnHand == null)
            {
                return 0;
            }
            // dch rkl 11/27/2016 return qty on hand
            //return (Convert.ToDecimal(quantityOnHand.QuantityOnHand) - Convert.ToDecimal(quantityOnHand.QuantityCommitted));
            return (Convert.ToDecimal(quantityOnHand.QuantityOnHand));
        }

        // dch rkl 11/27/2016 return qty available
        public decimal GetQuantityAvailDecimal(string itemCode, string wareHouseCode, string serialNumber)
        {
            IM_ItemCost quantityOnHand = _database.Table<IM_ItemCost>().Where(x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode && x.LotSerialNo == serialNumber).FirstOrDefault();
            if (quantityOnHand == null)
            {
                return 0;
            }
            return (Convert.ToDecimal(quantityOnHand.QuantityOnHand) - Convert.ToDecimal(quantityOnHand.QuantityCommitted));
        }

        // dch rkl 01/20/2017 - Get Qty On Hand, Qty Available and Unit Cost for a Lot/Serial Number
        public void GetQOHAvlCost(string itemCode, string wareHouseCode, string serialNumber, ref double QOH, ref double Avail, ref decimal UnitCost)
        {
            QOH = 0;
            Avail = 0;
            UnitCost = 0;
            IM_ItemCost itemCost = _database.Table<IM_ItemCost>().Where(x => x.ItemCode == itemCode && x.WarehouseCode == wareHouseCode && x.LotSerialNo == serialNumber).FirstOrDefault();
            if (itemCost != null)
            {
                QOH = Convert.ToDouble(itemCost.QuantityOnHand);
                Avail = Convert.ToDouble(itemCost.QuantityOnHand) - Convert.ToDouble(itemCost.QuantityCommitted);
                UnitCost = Convert.ToDecimal(itemCost.UnitCost);
            }
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

        // dch rkl 12/01/2016 add warehouse filter
        //public List<App_Item> GetItems(string itemCodeFilterText, string itemDescFilterText)
        public List<App_Item> GetItems(string itemCodeFilterText, string itemDescFilterText, string whseCodeFilter)
        {
            List<App_Item> itemList = new List<App_Item>();

           // if (item.WarehouseCode != null && item.WarehouseCode.Trim().Length > 0 && item.WarehouseCode != App.CurrentTechnician.DefaultWarehouse)


                lock (_locker)
            {
                // dch rkl 11/02/2016 Per Jeanne, include misc items, which do not have a warehouse
                List<CI_Item> lsItems = GetItemsFromDB(itemCodeFilterText, itemDescFilterText);
                foreach (CI_Item item in lsItems)
                {
                    // dch rkl 11/27/2016 UseInSO must be "Y"
                    if (item.UseInSO == "Y")
                    {
                        List<IM_ItemWarehouse> lsItmWhs = GetWarehousesForItem(item.ItemCode);
                        if (lsItmWhs.Count > 0)
                        {
                            foreach (IM_ItemWarehouse itemWarehouse in lsItmWhs)
                            {
                                // dch rkl 01/18/2017 Deduct Qty already allocated to tickets for this part from Qty On Hand BEGIN
                                List<JT_TransactionImportDetail> lsTID = App.Database.GetCurrentExport();
                                List<JT_TransactionImportDetail> lsMatches = 
                                    lsTID.Where(p => p.RecordType == "P" && p.ItemCode == itemWarehouse.ItemCode && p.WarehouseCode == itemWarehouse.WarehouseCode).ToList();
                                if (lsMatches .Count > 0)
                                {
                                    foreach (JT_TransactionImportDetail match in lsMatches)
                                    {
                                        itemWarehouse.QuantityOnHand = itemWarehouse.QuantityOnHand - (decimal)match.QuantityUsed;
                                    }
                                }
                                // dch rkl 01/18/2017 Deduct Qty already allocated to tickets for this part from Qty On Hand END

                                // dch rkl 12/01/2016 add warehouse code filter. 
                                if (whseCodeFilter.Length == 0 || (whseCodeFilter.Length > 0 && whseCodeFilter == itemWarehouse.WarehouseCode))
                                {
                                    itemList.Add(new App_Item(item, itemWarehouse, null));
                                }
                            }
                        }
                        else
                        {
                            // dch rkl 12/01/2016 when filtering by warehouse, only include misc items if no warehouse item found
                            if (whseCodeFilter.Length == 0 || (whseCodeFilter.Length > 0 && (item.ItemType == "4" || item.ItemType == "5")))
                            {
                                itemList.Add(new App_Item(item, new IM_ItemWarehouse(item.ItemCode), null));
                            }
                        }
                    }
                }
            }

            return itemList;
        }

        public List<App_Item> GetItems()
        {
            // dch rkl 12/01/2016 Add Whse Filter
            //return GetItems(null, null);
            return GetItems(null, null, "");
        }
        
        #endregion
    }

    // dch rkl 11/23/2016 list of serial / qty avail
    public partial class LotQavl
    {
        private string _lotNo;          // Lot/Serial Number
        private double _qavl;           // Quantity Available from Job Ops
        private double _qoh;            // Quantity on Hand from JobOps
        private double _qtyAlloc;       // Quantity Allocated on Tickets in JT_TransactionImportDetail
        private double _qtyUsed;        // Quantity Used against the current work ticket
        private decimal _unitCost;      // dch rkl 01/20/2017 Include Unit Cost

        public string LotNo
        {
            get { return _lotNo; }
            set { _lotNo = value; }
        }

        public double QAvl
        {
            get { return _qavl; }
            set { _qavl = value; }
        }


        public double Qoh
        {
            get { return _qoh; }
            set { _qoh = value; }
        }

        public double QtyAlloc
        {
            get { return _qtyAlloc; }
            set { _qtyAlloc = value; }
        }

        // dch rkl 01/20/2017 Include Unit Cost
        public decimal UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }

        public string LotNoQAvl
        {
            get
            {
                return string.Format("{0} - {1} On Hand; {2} Avail", _lotNo,  _qoh,_qavl);
            }
        }

        public double QtyUsed
        {
            get { return _qtyUsed; }
            set { _qtyUsed = value;  }
        }

        // dch rkl 01/20/2017 Include Unit Cost
        //public LotQavl(string lotNo, double qavl, double qoh)
        public LotQavl(string lotNo, double qavl, double qoh, decimal unitCost)
        {
            _lotNo = lotNo;
            _qavl = qavl;
            _qoh = qoh;
            _qtyUsed = 0;
            _qtyAlloc = 0;
            _unitCost = unitCost;       // dch rkl 01/20/2017 Include Unit Cost
        }
    }
}
