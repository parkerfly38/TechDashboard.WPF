using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_RepairParts.cs
     * 11/30/2016 DCH carry the SO Detail CommentText, Unit Price, Unit Cost, Quantity Required to the 
     *                JT Transaction Import Detail part
     * 12/05/2016 DCH allow for cases where the same part number exists on multiple lines
     * 01/13/2017 DCH Create function to save extended description to JT_TransactionImportDetail table.
     * 01/13/2017 DCH Part Description should come from sales order
     * 01/20/2017 DCH If Warehouse on Sales Order, use that, instead of the part default warehouse.
     * 01/20/2017 DCH Capture the Sales Order Line Key to the JT_TransactionImportDetail table.
     * 01/25/2017 BK  Adding itemcodedesc to SaveRepairPart to avoid null issue
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region App_RepairPart

        public List<App_RepairPart> RetrievePartsListFromWorkTicket(App_WorkTicket workTicket)
        {
            List<JT_TransactionImportDetail> importDetailList = null;
            List<SO_SalesOrderDetail> salesOrderDetaillist = null;
            List<App_RepairPart> partsList = new List<App_RepairPart>();

            lock (_locker)
            {
                importDetailList =
                    _database.Table<JT_TransactionImportDetail>().Where(
                        tid => (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                               (tid.RecordType == "P") &&
                               (tid.WTNumber == workTicket.WTNumber) &&
                               (tid.WTStep == workTicket.WTStep) //&&
                               //((tid.RemovePart == null) || (tid.RemovePart.ToUpper() != "Y"))  we're going to remove those downstream
                    ).ToList();

                if ((importDetailList != null) && (importDetailList.Count > 0))
                {
                    partsList = new List<App_RepairPart>();
                    foreach (JT_TransactionImportDetail detail in importDetailList)
                    {
                        partsList.Add(new App_RepairPart(detail, workTicket));
                    }
                }
            }

            // now let's check for default parts
            lock (_locker)
            {
                salesOrderDetaillist =
                    _database.Table<SO_SalesOrderDetail>().Where(
                        tid => (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                               (tid.JT158_WTPart == "Y") &&
                               (tid.JT158_WTNumber == workTicket.WTNumber) &&
                               (tid.JT158_WTStep == workTicket.WTStep))
                     .ToList();


                if ((salesOrderDetaillist != null) && (salesOrderDetaillist.Count > 0))
                {
                    foreach (SO_SalesOrderDetail detail in salesOrderDetaillist)
                    {
                        if (importDetailList.Count(x => x.ItemCode == detail.ItemCode) <= 0 && detail.ItemCode != "/EXPENSE")
                        {                            
                            var servicePart = _database.Table<JT_ServiceEquipmentParts>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            var equipmentAsset = _database.Table<JT_EquipmentAsset>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            var ciItem = _database.Table<CI_Item>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            App_RepairPart newRepairPart = new App_RepairPart(ciItem, workTicket);
                            newRepairPart.PartItemCode = (servicePart != null) ? servicePart.PartItemCode : detail.ItemCode;
                            newRepairPart.ProblemCode = (servicePart != null) ? servicePart.ProblemCode : "";

                            // dch rkl 11/30/2016 carry the SO Detail CommentText, Unit Price, Unit Cost, Quantity Required to the JT Transaction Import Detail part 
                            if (detail.CommentText != null) { newRepairPart.Comment = detail.CommentText; }
                            newRepairPart.UnitPrice = (double)detail.UnitPrice;
                            newRepairPart.UnitCost = (double)detail.UnitCost;
                            newRepairPart.QuantityReqd = detail.QuantityOrdered;
                            newRepairPart.Quantity = servicePart.Quantity;

                            // dch rkl 01/20/2017 If Warehouse on Sales Order, use that
                            if (detail.WarehouseCode != null)
                            {
                                newRepairPart.Warehouse = detail.WarehouseCode;
                            }
                            else
                            {
                                if (ciItem != null)
                                {
                                    newRepairPart.Warehouse = ciItem.DefaultWarehouseCode;
                                } else
                                {
                                    //bk assuming that this is an expense or the like
                                    newRepairPart.Warehouse = "";
                                }
                            }

                            // dch rkl 01/13/2017 Part Description should come from sales order
                            if (detail.ItemCodeDesc != null) { newRepairPart.PartItemCodeDescription = detail.ItemCodeDesc; }

                            // dch rkl 12/05/2016
                            newRepairPart.QuantityShipped = (double)detail.QuantityShipped;
                            
                            // dch rkl 11/23/2016 Use Unit of Measure from SO_SalesOrderDetail
                            newRepairPart.UnitOfMeasure = (detail.UnitOfMeasure != null) ? detail.UnitOfMeasure : "";

                            // dch rkl 01/23/2017 Capture SO Line Key
                            newRepairPart.SoLineKey = detail.LineKey;

                            JT_TransactionImportDetail newdetail = SaveRepairPart(newRepairPart, workTicket, new App_Technician(GetCurrentTechnicianFromDb()));
                            newRepairPart.ID = newdetail.ID;

                            partsList.Add(newRepairPart);
                        }
                    }
                }
            }

            // now let's cull removed parts from our list
            // dch rkl 12/05/2016 allow for cases where the same part number exists on multiple lines
            foreach (var part in partsList.ToList())
            {
                var importDetail =
                    _database.Table<JT_TransactionImportDetail>().Where(
                        tid => (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                               (tid.RecordType == "P") &&
                               (tid.WTNumber == workTicket.WTNumber) &&
                               (tid.WTStep == workTicket.WTStep) &&
                               (tid.ItemCode == part.PartItemCode) &&
                               (tid.QuantityUsed == part.Quantity) &&           // dch rkl 12/05/2016 match on qty
                               (tid.WarehouseCode == part.Warehouse) &&          // dch rkl 12/05/2016 match on whse
                               (tid.LotSerialNo == part.LotSerialNo) &&          // dch rkl 12/05/2016 match on lot/serial
                               (tid.UnitPrice == part.UnitPrice) &&              // dch rkl 12/05/2016 match on price
                               (tid.UnitOfMeasure == part.UnitOfMeasure) &&      // dch rkl 12/05/2016 match on UM
                               (tid.RemovePart == "Y")).FirstOrDefault();
                if (importDetail != null)
                    partsList.Remove(part);

            }

            return partsList;
        }

        public List<App_RepairPart> RetrievePartsListForRepairItem(App_RepairItem repairItem)
        {
            lock (_locker)
            {
                //App_CurrentSelectionData currentData = _database.Table<App_CurrentSelectionData>().FirstOrDefault();
                List<App_RepairPart> returnData = new List<App_RepairPart>();

                if (repairItem != null)
                {
                    // if ticket is prev. maint. AND template # is attached...
                    //if ((workTicket.IsPreventativeMaintenance) &&
                    //    (workTicket.HeaderTemplateNumber != null) &&
                    //    (workTicket.HeaderTemplateNumber.Trim().Length > 0))
                    if (false) // TODO
                    {
                        List<JT_EquipmentAsset> partsList =
                            _database.Table<JT_EquipmentAsset>().Where(
                                part => repairItem.ItemCode.Contains(part.ItemCode)
                            ).ToList();

                        foreach (JT_EquipmentAsset part in partsList)
                        {
                            //returnData.Add(new App_RepairPart(part, workTicket));
                        }
                    }
                    else
                    {

                        List<JT_ServiceEquipmentParts> partsList =
                            _database.Table<JT_ServiceEquipmentParts>().Where(
                                part => repairItem.ItemCode.Contains(part.ItemCode)
                            ).ToList();

                        foreach (JT_ServiceEquipmentParts part in partsList)
                        {
                            returnData.Add(new App_RepairPart(part, repairItem));
                        }
                    }
                }

                return returnData;
            }
        }

        public int DeleteRepairPart(App_RepairPart part, App_WorkTicket workTicket, App_Technician technician)
        {
            int rows = 0;
            JT_TransactionImportDetail detail = new JT_TransactionImportDetail();

            lock (_locker)
            {

                detail.ID = part.ID;
                detail.RecordType = "P";
                detail.SalesOrderNo = workTicket.SalesOrderNo;
                detail.WTNumber = workTicket.WTNumber;
                detail.WTStep = workTicket.WTStep;
                detail.EmployeeDeptNo = technician.TechnicianDeptNo;
                detail.EmployeeNo = technician.TechnicianNo;
                detail.TransactionDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                detail.ItemCode = part.PartItemCode;
                detail.WarehouseCode = part.Warehouse;
                detail.QuantityUsed = part.Quantity;
                detail.UnitCost = part.UnitCost;
                detail.UnitPrice = part.UnitPrice;
                detail.ChargePart = (part.IsChargeable ? "Y" : "N");
                detail.PrintPart = (part.IsPrintable ? "Y" : "N");
                detail.PurchasePart = (part.IsPurchased ? "Y" : "N");
                detail.Overhead = (part.IsOverhead ? "Y" : "N");
                detail.UnitOfMeasure = part.UnitOfMeasure;
                detail.CommentText = part.Comment;
                detail.LotSerialNo = part.LotSerialNo;

                rows = _database.Delete<JT_TransactionImportDetail>(detail.ID);              
            }
            return rows;

        }

        public JT_TransactionImportDetail SaveRepairPart(App_RepairPart part, App_WorkTicket workTicket, App_Technician technician)
        {
            int rows = 0;
            JT_TransactionImportDetail detail = new JT_TransactionImportDetail();

            lock(_locker)
            {

                detail.ID = part.ID;
                detail.RecordType = "P";
                detail.SalesOrderNo = workTicket.SalesOrderNo;
                detail.WTNumber = workTicket.WTNumber;
                detail.ItemCodeDesc = part.ItemCodeDesc;
                detail.WTStep = workTicket.WTStep;
                detail.EmployeeDeptNo = technician.TechnicianDeptNo;
                detail.EmployeeNo = technician.TechnicianNo;
                detail.TransactionDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                detail.TransactionDateAsDateTime = System.DateTime.Now;
                detail.ItemCode = part.PartItemCode;
                detail.WarehouseCode = part.Warehouse;
                detail.QuantityUsed = part.Quantity;
                detail.UnitCost = part.UnitCost;
                detail.UnitPrice = part.UnitPrice;
                detail.ChargePart = (part.IsChargeable ? "Y" : "N");
                detail.PrintPart = (part.IsPrintable ? "Y" : "N");
                detail.PurchasePart = (part.IsPurchased ? "Y" : "N");
                detail.Overhead = (part.IsOverhead ? "Y" : "N");
                detail.UnitOfMeasure = part.UnitOfMeasure;
                detail.CommentText = part.Comment;
                detail.LotSerialNo = part.LotSerialNo;
                detail.SOLineKey = part.SoLineKey;                          // dch rkl 01/23/2017 Save SOLineKey

                detail.QuantityRequired = (double)part.QuantityReqd;        // dch rkl 11/30/2016 save quantity required

                detail.QuantityCompleted = part.QuantityShipped;            // dch rkl 12/05/2016 save quantity shipped

                // do save
                if (detail.ID != 0)
                {
                    // update existing
                    rows = _database.Update(detail);
                }
                else
                {
                    // insert new
                    rows = _database.Insert(detail);
                }
            }

            return detail;
        }

        // dch rkl 01/13/2017 Create to save extended comment
        public JT_TransactionImportDetail SaveRepairPartDesc(App_RepairPart part, App_WorkTicket workTicket, App_Technician technician, string extdDesc)
        {
            int rows = 0;
            JT_TransactionImportDetail detail = new JT_TransactionImportDetail();

            lock (_locker)
            {

                detail.ID = part.ID;
                detail.RecordType = "P";
                detail.SalesOrderNo = workTicket.SalesOrderNo;
                detail.WTNumber = workTicket.WTNumber;
                detail.WTStep = workTicket.WTStep;
                detail.EmployeeDeptNo = technician.TechnicianDeptNo;
                detail.EmployeeNo = technician.TechnicianNo;
                detail.TransactionDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                detail.TransactionDateAsDateTime = System.DateTime.Now;
                detail.ItemCode = part.PartItemCode;
                detail.WarehouseCode = part.Warehouse;
                detail.QuantityUsed = part.Quantity;
                detail.UnitCost = part.UnitCost;
                detail.UnitPrice = part.UnitPrice;
                detail.ChargePart = (part.IsChargeable ? "Y" : "N");
                detail.PrintPart = (part.IsPrintable ? "Y" : "N");
                detail.PurchasePart = (part.IsPurchased ? "Y" : "N");
                detail.Overhead = (part.IsOverhead ? "Y" : "N");
                detail.UnitOfMeasure = part.UnitOfMeasure;
                detail.CommentText = part.Comment;
                detail.LotSerialNo = part.LotSerialNo;
                detail.ItemCodeDesc = extdDesc;         // Save the Extended Comment
                detail.QuantityRequired = (double)part.QuantityReqd;        
                detail.QuantityCompleted = part.QuantityShipped;
                detail.SOLineKey = part.SoLineKey;                          // dch rkl 01/23/2017 Save SOLineKey

                // do save
                if (detail.ID != 0)
                {
                    // update existing
                    rows = _database.Update(detail);
                }
                else
                {
                    // insert new
                    rows = _database.Insert(detail);
                }
            }

            return detail;
        }

        public int DeleteRepairPart(App_RepairPart part)
        {
            int rows = 0;
            JT_TransactionImportDetail detail = new JT_TransactionImportDetail();

            lock (_locker)
            {
                detail =
                    _database.Table<JT_TransactionImportDetail>().Where(
                        tid => (tid.ID == part.ID)
                    ).FirstOrDefault();

                detail.RemovePart = "Y";
                detail.TransactionDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                detail.TransactionDateAsDateTime = System.DateTime.Now;

                rows = _database.Update(detail);
            }

            return rows;
        }

        #endregion
    }
}
