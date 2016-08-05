using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region App_RepairPart

        public List<App_RepairPart> RetrievePartsListFromWorkTicket(App_WorkTicket workTicket)
        {
            List<JT_TransactionImportDetail> importDetailList = null;
            List<SO_SalesOrderDetail> salesOrderDetaillist = null;
            List<App_RepairPart> partsList = null;

            lock(_locker)
            {
                importDetailList = 
                    _database.Table<JT_TransactionImportDetail>().Where(
                        tid => (tid.SalesOrderNo == workTicket.SalesOrderNo) &&
                               (tid.RecordType == "P") &&
                               (tid.WTNumber == workTicket.WTNumber) &&
                               (tid.WTStep == workTicket.WTStep) &&
                               ((tid.RemovePart == null) || (tid.RemovePart.ToUpper() != "Y"))
                    ).ToList();

                if((importDetailList != null) && (importDetailList.Count > 0))
                {
                    partsList = new List<App_RepairPart>();
                    foreach(JT_TransactionImportDetail detail in importDetailList)
                    {
                        partsList.Add(new App_RepairPart(detail, workTicket));
                    }
                }
            }

            // now let's check for default parts
            lock(_locker)
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
                        if ( importDetailList.Count(x => x.ItemCode == detail.ItemCode) <= 0)
                        {
                            var servicePart = _database.Table<JT_ServiceEquipmentParts>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            var equipmentAsset = _database.Table<JT_EquipmentAsset>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            var ciItem = _database.Table<CI_Item>().Where(x => x.ItemCode == detail.ItemCode).FirstOrDefault();
                            App_RepairPart newRepairPart = new App_RepairPart(ciItem, workTicket);
                            newRepairPart.PartItemCode = (servicePart != null) ? servicePart.PartItemCode : detail.ItemCode;
                            newRepairPart.ProblemCode = (servicePart != null) ? servicePart.ProblemCode : "";
                            SaveRepairPart(newRepairPart, workTicket, new App_Technician(GetCurrentTechnicianFromDb()));
                            partsList.Add(newRepairPart);
                        }
                    }
                }
            }

            return partsList;
        }

        //public List<App_RepairPart> RetrievePartsListFromWorkTicket(App_WorkTicket workTicket)
        //{
        //    lock (_locker)
        //    {
        //        //App_CurrentSelectionData currentData = _database.Table<App_CurrentSelectionData>().FirstOrDefault();
        //        List<App_RepairPart> returnData = new List<App_RepairPart>();
        //        CI_Item repairItem = RetrieveRepairItemFromWorkTicket(workTicket);

        //        if (repairItem != null)
        //        {
        //            // if ticket is prev. maint. AND template # is attached...
        //            //if ((workTicket.IsPreventativeMaintenance) &&
        //            //    (workTicket.HeaderTemplateNumber != null) &&
        //            //    (workTicket.HeaderTemplateNumber.Trim().Length > 0))
        //            if (false) // puke
        //            {
        //                List<JT_EquipmentAsset> partsList =
        //                    _database.Table<JT_EquipmentAsset>().Where(
        //                        part => repairItem.ItemCode.Contains(part.ItemCode)
        //                    ).ToList();

        //                foreach (JT_EquipmentAsset part in partsList)
        //                {
        //                    //returnData.Add(new App_RepairPart(part, workTicket));
        //                }
        //            }
        //            else
        //            {

        //                List<JT_ServiceEquipmentParts> partsList =
        //                    _database.Table<JT_ServiceEquipmentParts>().Where(
        //                        part => repairItem.ItemCode.Contains(part.ItemCode)
        //                    ).ToList();

        //                foreach (JT_ServiceEquipmentParts part in partsList)
        //                {
        //                    returnData.Add(new App_RepairPart(part, workTicket));
        //                }
        //            }
        //        }

        //        PukeTestJoin();

        //        return returnData;
        //    }
        //}

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
                    if (false) // puke
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

        public int SaveRepairPart(App_RepairPart part, App_WorkTicket workTicket, App_Technician technician)
        {
            int rows = 0;
            JT_TransactionImportDetail detail = new JT_TransactionImportDetail();

            lock(_locker)
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

            return rows;
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

                rows = _database.Update(detail);
            }

            return rows;
        }

        #endregion
    }
}
