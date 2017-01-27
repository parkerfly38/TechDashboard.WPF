using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * PartsEditPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     * 01/13/2017 DCH Return the ID of the Inserted Part, so it can be used to save Lot/Serial Number allocation.
     * 01/13/2017 DCH Add the ExtendedDescriptionKey and ExtendedDescriptionText
     * 01/23/2017 DCH Add Unit of Measure List
     *********************************************************************************************************/
    public class PartsEditPageViewModel : INotifyPropertyChanged
    {
        #region properties

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        App_RepairPart _partToEdit;
        public App_RepairPart PartToEdit
        {
            get { return _partToEdit; }
        }

        // dch rkl 12/01/2016 return warehouse code and description
        //protected List<string> _warehouseList;
        //public List<string> WarehouseList
        //{
        //    get { return _warehouseList; }
        //}
        protected List<IM_Warehouse> _warehouseList;
        public List<IM_Warehouse> WarehouseList
        {
            get { return _warehouseList; }
        }

        // dch rkl 01/23/2017 add unit of measure list
        protected List<CI_UnitOfMeasure> _unitOfMeasureList;
        public List<CI_UnitOfMeasure> UnitOfMeasureList
        {
            get { return _unitOfMeasureList; }
        }

        protected string _qtyAvailable;
        public string QtyAvailable
        {
            get { return _qtyAvailable; }
            set
            {
                _qtyAvailable = value;
                NotifyPropertyChanged();
            }
        }

        // DCH 01/13/2017 Add the ExtendedDescriptionKey and ExtendedDescriptionText BEGIN
        protected int _extendedDescriptionKey;
        public int ExtendedDescriptionKey
        {
            get { return _extendedDescriptionKey; }
        }
        protected string _extendedDescriptionText;
        public string ExtendedDescriptionText
        {
            get { return _extendedDescriptionText; }
        }
        // DCH 01/13/2017 Add the ExtendedDescriptionKey and ExtendedDescriptionText END

        #endregion

        public PartsEditPageViewModel(App_WorkTicket workTicket, CI_Item partToEdit)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = workTicket;

                // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse BEGIN
                //_warehouseList = new List<string>();
                //List<IM_ItemWarehouse> lsItmWhse = GetItemWarehouses(partToEdit.ItemCode);
                //foreach (IM_ItemWarehouse itemWhse in lsItmWhse)
                //{
                //    _warehouseList.Add(itemWhse.WarehouseCode);
                //}
                _warehouseList = GetWarehouses();
                foreach(IM_Warehouse whs in _warehouseList)
                {
                    whs.WarehouseDesc = string.Format("{0} - {1}", whs.WarehouseCode, whs.WarehouseDesc);
                }
                // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse END

                // dch rkl 01/23/2017 added Unit of Measure List
                _unitOfMeasureList = App.Database.GetCI_UnitOfMeasureFromDB();
                _unitOfMeasureList.Add(new CI_UnitOfMeasure() { UnitOfMeasure = "EACH" });
                _unitOfMeasureList.Sort((x, y) => x.UnitOfMeasure.CompareTo(y.UnitOfMeasure));

                // DCH 01/13/2017 Add the ExtendedDescriptionKey and ExtendedDescriptionText BEGIN
                _extendedDescriptionKey = partToEdit.ExtendedDescriptionKey;
                _extendedDescriptionText = "";
                if (partToEdit.ExtendedDescriptionKey > 0)
                {
                    CI_ExtendedDescription extdDesc = App.Database.GetExtendedDescription(partToEdit.ExtendedDescriptionKey);
                    if (extdDesc.ExtendedDescriptionText != null) { _extendedDescriptionText = extdDesc.ExtendedDescriptionText; }
                }
                // DCH 01/13/2017 Add the ExtendedDescriptionKey and ExtendedDescriptionText END
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel(App_WorkTicket workTicket, CI_Item partToEdit");
            }
        }

        public PartsEditPageViewModel(App_WorkTicket workTicket, App_RepairPart partToEdit)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicket = workTicket;
                _partToEdit = partToEdit;

                // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse BEGIN
                //_warehouseList = GetTechnicianWarehouses();
                //_warehouseList = new List<string>();
                // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse
                //List<IM_ItemWarehouse> lsItmWhse = GetItemWarehouses(partToEdit.PartItemCode);
                //foreach (IM_ItemWarehouse itemWhse in lsItmWhse)
                //{
                //    _warehouseList.Add(itemWhse.WarehouseCode);
                //}
                _warehouseList = GetWarehouses();
                foreach (IM_Warehouse whs in _warehouseList)
                {
                    whs.WarehouseDesc = string.Format("{0} - {1}", whs.WarehouseCode, whs.WarehouseDesc);
                }
                // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse END

                // dch rkl 01/23/2017 added Unit of Measure List
                _unitOfMeasureList = App.Database.GetCI_UnitOfMeasureFromDB();
                _unitOfMeasureList.Add(new CI_UnitOfMeasure() { UnitOfMeasure = "EACH" });
                _unitOfMeasureList.Sort((x, y) => x.UnitOfMeasure.CompareTo(y.UnitOfMeasure));
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel(App_WorkTicket workTicket, App_RepairPart partToEdit");
            }
        }

        public void DeletePart()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.DeleteRepairPart(_partToEdit); //, _workTicket, App.CurrentTechnician);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.DeletePart");
            }
        }

        // dch rkl 01/13/2017 Return the ID of the Inserted Part
        //public void AddPartToPartsList()
        public int AddPartToPartsList()
        {
            // dch rkl 01/13/2017 Return the ID of the Inserted Part
            int iId = 0;

            // dch rkl 12/07/2016 catch exception
            try
            {
                // dch rkl 01/13/2017 Return the ID of the Inserted Part
                //App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
                JT_TransactionImportDetail jtDtl = App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
                iId = jtDtl.ID;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.AddPartToPartsList");
            }

            // dch rkl 01/13/2017 Return the ID of the Inserted Part
            return iId;
        }

        public void UpdatePartOnPartsList()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
                //App.Database.UpdatePartOnPartsList(_partToEdit);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.UpdatePartOnPartsList");
            }
        }

        protected List<string> GetTechnicianWarehouses()
        {
            List<string> warehouseList = App.Database.GetTechnicianWarehouses();

            // dch rkl 12/07/2016 catch exception
            try
            {
                App_Technician technician = App.CurrentTechnician;

                if((PartToEdit.Warehouse != null) &&
                   (!warehouseList.Contains(PartToEdit.Warehouse)))
                {
                    warehouseList.Insert(0, PartToEdit.Warehouse);
                }
                if ((technician.DefaultWarehouse != null) &&
                    (!warehouseList.Contains(technician.DefaultWarehouse)))
                {
                    warehouseList.Insert(0, technician.DefaultWarehouse);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.GetTechnicianWarehouses");
            }

            return warehouseList;
        }

        protected List<IM_ItemWarehouse> GetItemWarehouses(string itemCode)
        {
            // dch rkl 12/07/2016 catch exception
            //List<IM_ItemWarehouse> warehouseList = App.Database.GetWarehousesForItem(itemCode);
            List<IM_ItemWarehouse> warehouseList = new List<IM_ItemWarehouse>();

            try
            {
                warehouseList = App.Database.GetWarehousesForItem(itemCode);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.GetItemWarehouses");
            }

            return warehouseList;
        }

        // dch rkl 12/01/2016 Get Warehouse List from IM_Warehouse instead of IM_ItemWarehouse
        protected List<IM_Warehouse> GetWarehouses()
        {
            // dch rkl 12/07/2016 catch exception
            //List<IM_Warehouse> warehouseList = App.Database.GetIMWarehouseFromDB();
            List<IM_Warehouse> warehouseList = new List<IM_Warehouse>();
            try
            {
                warehouseList = App.Database.GetIMWarehouseFromDB();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.GetWarehouses");
            }

            return warehouseList;
        }

        public void UpdateQuantityOnHand(string wareHouse, string serialNumber)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                QtyAvailable = App.Database.GetQuantityOnHand(PartToEdit.PartItemCode, wareHouse, serialNumber);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.UpdateQuantityOnHand");
            }
        }

        // dch rkl 11/27/2016 Include Qty Available
        //public List<string> GetMfgSerialNumbersForPart()
        public List<Data.LotQavl> GetMfgSerialNumbersForPart()
        {
            // dch rkl 12/07/2016 catch exception
            List<Data.LotQavl> serialNumberList = new List<Data.LotQavl>();
            try
            {
                // dch rkl 11/27/2016 Include Qty Available
                //List<string> serialNumberList = App.Database.GetMfgSerialNumbersForItem(_partToEdit.PartItemCode, PartToEdit.Warehouse, _workTicket.SalesOrderNo, _workTicket.WTNumber, _workTicket.WTStep);
                serialNumberList = App.Database.GetMfgSerialNumbersForItem(_partToEdit.PartItemCode, PartToEdit.Warehouse, _workTicket.SalesOrderNo, _workTicket.WTNumber, _workTicket.WTStep);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditPageViewModel.GetMfgSerialNumbersForPart");
            }

            return serialNumberList;
        }
    }
}
