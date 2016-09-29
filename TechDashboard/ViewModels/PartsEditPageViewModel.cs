using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class PartsEditPageViewModel : INotifyPropertyChanged
    {
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

        protected List<string> _warehouseList;
        public List<string> WarehouseList
        {
            get { return _warehouseList; }
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


        public PartsEditPageViewModel(App_WorkTicket workTicket, CI_Item partToEdit)
        {
            _workTicket = workTicket;
            //_partToEdit = partToEdit;

            //_warehouseList = GetTechnicianWarehouses();
            List<IM_ItemWarehouse> lsItmWhse = GetItemWarehouses(partToEdit.ItemCode);
            _warehouseList = new List<string>();
            foreach (IM_ItemWarehouse itemWhse in lsItmWhse)
            {
                _warehouseList.Add(itemWhse.WarehouseCode);
            }
        }

        public PartsEditPageViewModel(App_WorkTicket workTicket, App_RepairPart partToEdit)
        {
            _workTicket = workTicket;
            _partToEdit = partToEdit;

            //_warehouseList = GetTechnicianWarehouses();
            List<IM_ItemWarehouse> lsItmWhse = GetItemWarehouses(partToEdit.PartItemCode);
            _warehouseList = new List<string>();
            foreach (IM_ItemWarehouse itemWhse in lsItmWhse)
            {
                _warehouseList.Add(itemWhse.WarehouseCode);
            }
        }

        public void AddPartToPartsList()
        {
            App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
        }

        public void UpdatePartOnPartsList()
        {
            App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
            //App.Database.UpdatePartOnPartsList(_partToEdit);
        }

        protected List<string> GetTechnicianWarehouses()
        {
            App_Technician technician = App.CurrentTechnician;
            List<string> warehouseList = App.Database.GetTechnicianWarehouses();

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

            return warehouseList;
        }

        protected List<IM_ItemWarehouse> GetItemWarehouses(string itemCode)
        {
            List<IM_ItemWarehouse> warehouseList = App.Database.GetWarehousesForItem(itemCode);

            return warehouseList;
        }

        public void UpdateQuantityOnHand(string wareHouse, string serialNumber)
        {
            QtyAvailable = App.Database.GetQuantityOnHand(PartToEdit.PartItemCode, wareHouse, serialNumber);
        }

        public List<string> GetMfgSerialNumbersForPart()
        {
            List<string> serialNumberList = App.Database.GetMfgSerialNumbersForItem(_partToEdit.PartItemCode, PartToEdit.Warehouse, _workTicket.SalesOrderNo, _workTicket.WTNumber, _workTicket.WTStep);

            return serialNumberList;
        }
    }
}
