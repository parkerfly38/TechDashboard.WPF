using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * PartsEditLotSerNoPageViewModel.cs
     * 12/07/2016 DCH Add error handling
     * 01/16/2017 DCH If negative quantity entered, and it's lot-controlled, allow them to select the
     *                lot/serial number.
     * 01/20/2017 DCH Include unit cost in grid.
     *********************************************************************************************************/
    public class PartsEditLotSerNoPageViewModel : INotifyPropertyChanged
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

        List<LotQavl> _serialNumberList;
        public List<LotQavl> SerialNumberList
        {
            get { return _serialNumberList; }
        }

        #endregion

        public PartsEditLotSerNoPageViewModel(App_RepairPart partToEdit, App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _partToEdit = partToEdit;
                _workTicket = workTicket;

                double qty = 0;

                // dch rkl 01/16/2017 if negative quantity, allow them to enter the serial number
                if (PartToEdit.Quantity < 0)
                {
                    // Get Lot History for this Serial Number and Part.  Use this to select from.
                     _serialNumberList = GetHistParts(partToEdit.PartItemCode);
                }
                else
                {
                    _serialNumberList = App.Database.GetMfgSerialNumbersForItem(_partToEdit.PartItemCode, PartToEdit.Warehouse,
                        workTicket.SalesOrderNo, workTicket.WTNumber, workTicket.WTStep);
                }

                // Display previously allocated quantities in grid
                if (_partToEdit.LotSerialNo != null && _partToEdit.LotSerialNo.Trim().Length > 0)
                {
                    string[] lotSerQty = _partToEdit.LotSerialNo.Split('|');

                    foreach (LotQavl lotQ in _serialNumberList)
                    {
                        foreach(string lsq in lotSerQty)
                        {
                            string[] sqty = lsq.Split('~');
                            if (sqty.GetUpperBound(0) > 0)
                            {
                                if (sqty[0].Trim().ToUpper() == lotQ.LotNo.Trim().ToUpper())
                                {
                                    double.TryParse(sqty[1], out qty);
                                    lotQ.QtyUsed = qty;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditLotSerNoPageViewModel(App_RepairPart partToEdit, App_WorkTicket workTicket)");
            }
        }

        // dch rkl 01/16/2017 Return List of Previously Used Serial Numbers for this Mfg Serial Nbr / Part Nbr
        private List<LotQavl> GetHistParts(string partNbr)
        {
            List<LotQavl> lsLotQ = new List<LotQavl>();

            List<App_History> lsHistory = new List<App_History>();
            CI_Item item = App.Database.GetItemFromDB(_workTicket.DtlRepairItemCode);
            JT_EquipmentAsset equipmentAsset = App.Database.GetEquipmentAsset(_workTicket.DtlRepairItemCode);

            //get any possible work tickets and associated work tickets
            List<JT_WorkTicket> workTickets = App.Database.GetWorkTickets(_workTicket.DtlRepairItemCode, _workTicket.DtlMfgSerialNo); //, _workTicket.SalesOrderNo);

            foreach (var workTicket in workTickets)
            {
                List<JT_Transaction> transactionCode = App.Database.GetTransactions(workTicket.SalesOrderNo, workTicket.WTNumber, workTicket.WTStep);
                List<JT_TransactionHistory> transactionHistoryCode = App.Database.GetTransactionHistory(workTicket.SalesOrderNo, workTicket.WTNumber, workTicket.WTStep);

                foreach (var transaction in transactionCode)
                {
                    if (transaction.RecordType != "LD" && transaction.ItemCode != null && transaction.ItemCode == partNbr)
                    {
                        if (transaction.QuantityUsed != 0)
                        {
                            // Do not add duplicate lot numbers 
                            LotQavl exists = lsLotQ.Where(p => p.LotNo == transaction.LotSerialNo).FirstOrDefault();
                            // dch rkl 01/20/2017 include unit cost
                            //if (exists == null) { lsLotQ.Add(new LotQavl(transaction.LotSerialNo, 0, 0)); }
                            decimal UnitCost = 0;
                            if (transaction.UnitCost != null) { decimal.TryParse(transaction.UnitCost, out UnitCost); }
                            if (exists == null) { lsLotQ.Add(new LotQavl(transaction.LotSerialNo, 0, 0, UnitCost)); }
                        }
                    }
                }
                foreach (var transaction in transactionHistoryCode)
                {
                    if (transaction.RecordType != "LD" && transaction.ItemCode != null && transaction.ItemCode == partNbr)
                    {
                        if (transaction.QuantityUsed != 0)
                        {
                            // Do not add duplicate lot numbers 
                            LotQavl exists = lsLotQ.Where(p => p.LotNo == transaction.LotSerialNo).FirstOrDefault();
                            // dch rkl 01/20/2017 include unit cost
                            //if (exists == null) { lsLotQ.Add(new LotQavl(transaction.LotSerialNo, 0, 0)); }
                            decimal UnitCost = 0;
                            if (transaction.UnitCost != null) { decimal.TryParse(transaction.UnitCost, out UnitCost); }
                            if (exists == null) { lsLotQ.Add(new LotQavl(transaction.LotSerialNo, 0, 0, UnitCost)); }
                        }
                    }

                }
            }

            return lsLotQ;
        }

        public void UpdatePartOnPartsList()
        {
            // Save the part
            // dch rkl 12/07/2016 catch exception
            try
            {
                App.Database.SaveRepairPart(_partToEdit, _workTicket, App.CurrentTechnician);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.PartsEditLotSerNoPageViewModel.UpdatePartOnPartsList");
            }
        }
    }
}
