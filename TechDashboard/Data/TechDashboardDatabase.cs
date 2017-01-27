using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;
//using Rkl.Erp.Sage.Sage100.TableObjects;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDatabase.cs
     * 11/30/2016 DCH When data is pulled back from JobOps, remove any parts records in JT_TransactionImportDetail 
     *                where quantity used is zero.  These are the default parts, and they will get reloaded 
     *                when the parts screen is accessed for a ticket.  This will allow any changes in the Sales 
     *                Order Detail table for the default parts to get refreshed.
     * 12/01/2016 DCH Added IM_Warehouse, JT_CustomerBillingRates
     * 01/20/2017 DCH Do not create database in base ProgramData folder.  Create in ProgramData / Job Ops.
     * 01/20/2017 DCH Add CI_UnitOfMeasure table.
     * 01/20/2017 DCH Load JT_WorkTicketText as part of Work Ticket load step.
     * 01/23/2017 DCH Load AR_Options table.
     * 01/27/2017 BK  Load CI_Options table
     *********************************************************************************************************/
    public enum ConnectionType
    {
        SData,
        Rest
    }

    public partial class TechDashboardDatabase
    { 
        private SQLiteConnection _database;
        private ConnectionType _dataConnectionType = ConnectionType.Rest;
        private static object _locker = new object();

        private SDataClient _sDataClient;
        private RestClient _restClient;
        
        #region Application Object functions




        #region App_RepairItem

        #endregion

        #endregion

        public delegate void CurrentTechnicianChangedEventHandler(object sender, EventArgs e);
        public delegate void CurrentScheduleDetailChangedEventHandler(object sender, EventArgs e);
        public delegate void CurrentWorkTicketChangedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event fired when a JT_Technician object is marked as being the current technician
        /// in the local database.
        /// </summary>
        public event CurrentTechnicianChangedEventHandler CurrentTechnicianChanged;
        /// <summary>
        /// Event fired when a JT_TechnicianScheduleDetail object is marked as being the
        /// current schedule object/work ticket in the local database.
        /// </summary>
        public event CurrentScheduleDetailChangedEventHandler CurrentScheduleDetailChanged;
        public event CurrentWorkTicketChangedEventHandler CurrentWorkTicketChanged;

        protected virtual void OnCurrentTechnicianChanged(EventArgs e)
        {
            if(CurrentTechnicianChanged != null)
            {
                CurrentTechnicianChanged(this, e);
            }
        }

        protected virtual void OnCurrentScheduleDetailChanged(EventArgs e)
        {
            if (CurrentScheduleDetailChanged != null)
            {
                CurrentScheduleDetailChanged(this, e);
            }
        }

        protected virtual void OnCurrentWorkTicketChanged(EventArgs e)
        {
            if (CurrentWorkTicketChanged != null)
            {
                CurrentWorkTicketChanged(this, e);
            }
        }

        public TechDashboardDatabase()
        {
            // dch rkl 01/20/2017 Do not create database in base ProgramData folder
            //_database = new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\techdashboard.db");
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Job Ops") == false)
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Job Ops");
            }
            _database = new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Job Ops\\techdashboard.db");
        }

        public void FillLocalTable<T>()
        {
            switch(_dataConnectionType)
            {
                case ConnectionType.Rest:
                    FillLocalTableFromRestService<T>();
                    break;
                case ConnectionType.SData:
                    FillLocalTableFromSData<T>();
                    break;
                default:
                    break;

            }
        }

        public void FillLocalTable<T>(string filterType, string filterText)
        {
            switch (_dataConnectionType)
            {
                case ConnectionType.Rest:
                    FillLocalTableFromRestService<T>(filterType, filterText);
                    break;
                case ConnectionType.SData:
                    FillLocalTableFromSData<T>(filterType, filterText);
                    break;
                default:
                    break;

            }
        }

        public List<T> GetErpData<T>(string filterType, string filterText)
        {
            List<T> returnData = null;

            switch (_dataConnectionType)
            {
                case ConnectionType.Rest:
                    returnData = GetErpDataFromRestService<T>(filterType, filterText);
                    break;
                case ConnectionType.SData:                    
                    returnData = GetErpDataFromSData<T>(filterType, filterText); // TODO... await this!
                    break;
                default:
                    break;
            }

            return returnData;
        }

        protected async Task<bool> UpdateErpTechnicianStatus(JT_Technician technician)
        {
            bool result = false;

            switch (_dataConnectionType)
            {
                case ConnectionType.Rest:
                    result = await UpdateTechnicianStatusToRestService(technician);
                    break;
                case ConnectionType.SData:
                    result = UpdateTechnicianStatusToSData(technician);
                    break;
                default:
                    break;
            }
                
            return result;
        }

        // dch rkl 12/09/2016 Insert now returns object result
        //protected async Task<bool> InsertErpTransactionImportDetail(JT_TransactionImportDetail importDetail)
        protected async Task<Rkl.Erp.Sage.Sage100.TableObjects.API_Results> InsertErpTransactionImportDetail(JT_TransactionImportDetail importDetail)
        {
            // dch rkl 12/09/2016 Insert now returns object result
            //bool result = false;
            Rkl.Erp.Sage.Sage100.TableObjects.API_Results result = new Rkl.Erp.Sage.Sage100.TableObjects.API_Results();

            switch (_dataConnectionType)
            {
                case ConnectionType.Rest:
                    // dch rkl 12/09/2016 Insert now returns object result
                    //result = await InsertTransactionImportDetailToRestService(importDetail);
                    result.Success = await InsertTransactionImportDetailToRestService(importDetail);
                    break;
                case ConnectionType.SData:
                    // dch rkl 12/09/2016 Insert now returns object result
                    result = InsertTransactionImportDetailToSData(importDetail);
                    break;
                default:
                    break;
            }

            return result;
        }

        public async void InsertErpTransactionImportDetail(List<JT_TransactionImportDetail> importDetails)
        {
            foreach (var importDetail in importDetails)
            {
                // dch rkl 12/09/2016 Insert now returns object result
                //bool result = await InsertErpTransactionImportDetail(importDetail);
                Rkl.Erp.Sage.Sage100.TableObjects.API_Results result = await InsertErpTransactionImportDetail(importDetail);
                //if (result)
                if (result.Success)
                {
                    DeleteExportRow(importDetail);
                }
            }
        }

        #region SData-specific Functions

        public List<T> GetErpDataFromSData<T>(string filterType, string filterText)
        {
            if(_sDataClient == null)
            {
                _sDataClient = new SDataClient(this);
            }
           
            List<T> items = _sDataClient.GetData<T>(filterType, filterText);

            return items;
        }

        public void FillLocalTableFromSData<T>()
        {
            FillLocalTableFromSData<T>(string.Empty, string.Empty);
        }

        public void FillLocalTableFromSData<T>(string filterType, string filterText)
        {
            int rows = 0;

            List<T> items = GetErpDataFromSData<T>(filterType, filterText);

            lock (_locker)
            {
                _database.DeleteAll<T>();

                rows = _database.InsertAll(items);
            }

            System.Diagnostics.Debug.WriteLine("Rows added for " + typeof(T).Name + " = " + rows.ToString());
        }

        protected bool UpdateTechnicianStatusToSData(JT_Technician technician)
        {
            bool result = _sDataClient.UpdateTechnicianRecord(technician);

            return result;
        }

        // dch rkl 12/09/2016 Insert now returns object result
        //protected bool InsertTransactionImportDetailToSData(JT_TransactionImportDetail importDetail)
        protected Rkl.Erp.Sage.Sage100.TableObjects.API_Results InsertTransactionImportDetailToSData(JT_TransactionImportDetail importDetail)
        {
            // dch rkl 12/09/2016 Insert now returns object result
            //bool result = _sDataClient.InsertRecord(importDetail);
            return _sDataClient.InsertRecord(importDetail);

            //return result;
        }

        #endregion

        #region REST-specific Functions

        public List<T> GetErpDataFromRestService<T>(string filterType, string filterText)
        {
            List<T> items = null;

            if(_restClient == null)
            {
                _restClient = new RestClient(this);
            }

            if (typeof(T) == typeof(SO_ShipToAddress) || typeof(T) == typeof(SO_SalesOrderDetail) || typeof(T) == typeof(AR_Customer) || typeof(T) == typeof(AR_CustomerContact) ||
                typeof(T) == typeof(JT_ServiceEquipmentParts))
            {
                    items = _restClient.GetDataPostSync<T>(filterType, filterText);
            }
            else {

                items = _restClient.GetDataSync<T>(filterType, filterText);
            }
            return items;
        }

        public void FillLocalTableFromRestService<T>()
        {
            FillLocalTableFromRestService<T>(string.Empty, string.Empty);
        }

        public void FillLocalTableFromRestService<T>(string filterType, string filterText)
        {
            int rows = 0;

            List<T> items = GetErpDataFromRestService<T>(filterType, filterText);

            lock (_locker)
            {
                _database.DeleteAll<T>();

                rows = _database.InsertAll(items);
            }

            System.Diagnostics.Debug.WriteLine("Rows added for " + typeof(T).Name + " = " + rows.ToString());
        }

        protected async Task<bool> UpdateTechnicianStatusToRestService(JT_Technician technician)
        {
            bool result = await _restClient.UpdateTechnicianRecord(technician);

            return result;
        }

        protected async Task<bool> InsertTransactionImportDetailToRestService(JT_TransactionImportDetail importDetail)
        {
            bool result = await _restClient.InsertTransactionImportDetailRecord(importDetail);

            return result;
        }

        #endregion

        /// <summary>
        /// Creates the CI_Item, JT_Technician, and JT_TechnicianStatus tables
        /// in the local database.  Then, all data for these tables is retreived 
        /// </summary>
        public void CreateGlobalTables()
        {
            //return;

            // always need all data in these tables

            // Testing... first, drop the tables if they exist so all is clean
            try { _database.DropTable<CI_Options>(); } catch { }
            try { _database.DropTable<JT_FieldServiceOptions>(); } catch { }
            try { _database.DropTable<CI_Item>(); } catch { }
            try { _database.DropTable<IM_ItemWarehouse>(); } catch { }
            try { _database.DropTable<IM_ItemCost>(); } catch { }
            try { _database.DropTable<JT_MiscellaneousCodes>(); } catch { }
            try { _database.DropTable<JT_ClassificationCode>(); } catch { }
            try { _database.DropTable<JT_EarningsCode>(); } catch { }
            try { _database.DropTable<JT_ActivityCode>(); } catch { }
            try { _database.DropTable<JT_Technician>(); } catch { }
            try { _database.DropTable<JT_TechnicianStatus>(); } catch { }

            // dch rkl 10/31/2016 additional tables
            try { _database.DropTable<JT_Options>(); } catch { }
            try { _database.DropTable<PR_EarningsDeduction>(); } catch { }
            try { _database.DropTable<JT_TimeTrackerOptions>(); } catch { }

            // dch rkl 11/16/2016 additional tables
            try { _database.DropTable<SO_Options>(); } catch { }

            // dch rkl 12/01/2016 additional tables
            try { _database.DropTable<IM_Warehouse>(); } catch { }
            try { _database.DropTable<JT_CustomerBillingRates>(); } catch { }

            // Next, create the tables
            _database.CreateTable<CI_Options>();
            _database.CreateTable<JT_FieldServiceOptions>();
            _database.CreateTable<CI_Item>();
            _database.CreateTable<IM_ItemCost>();
            _database.CreateTable<IM_ItemWarehouse>();
            _database.CreateTable<JT_MiscellaneousCodes>();
            _database.CreateTable<JT_ClassificationCode>();
            _database.CreateTable<JT_EarningsCode>();
            _database.CreateTable<JT_ActivityCode>();
            _database.CreateTable<JT_Technician>();
            _database.CreateTable<JT_TechnicianStatus>();

            // dch rkl 10/31/2016 additional tables
            _database.CreateTable<JT_Options>(); 
            _database.CreateTable<PR_EarningsDeduction>();
            _database.CreateTable<JT_TimeTrackerOptions>();

            // dch rkl 11/16/2016 additional tables
            _database.CreateTable<SO_Options>();

            // dch rkl 12/01/2016 additional tables
            _database.CreateTable<IM_Warehouse>();
            _database.CreateTable<JT_CustomerBillingRates>();

            // dch rkl 01/20/2017 CI_UnitOfMeasure table
            _database.CreateTable<CI_UnitOfMeasure>();

            // dch rkl 01/23/2017 AR_Options table
            _database.CreateTable<AR_Options> ();

            //Fill CI_Options
            FillCIOptions();

            // Fill the tables with data.
            FillFieldServiceOptionsTable();
            FillItemTable();
            FillItemWarehouseTable();
            FillMiscellaneousCodesTable();
            FillEarningsCodesTable();
            FillActivityCodesTable();
            FillTechnicianTable();
            FillTechnicianStatusTable();
            FillClassificationCodeTable();

            // dch rkl 10/31/2016 additional tables
            FillJTOptionsTable();
            FillPREarningsDeductionTable();
            FillJTTimeTrackerOptions();

            // dch rkl 11/16/2016 additional tables
            FillSOOptionsTable();

            // dch rkl 12/01/2016 additional tables
            FillIMWarehouseTable();
            FillJT_CustomerBillingRatesTable();

            // dch rkl 01/20/2017 CI_UnitOfMeasure table
            FillCI_UnitOfMeasureTable();

            // dch rkl 01/23/2017 AR_Options table
            FillAR_OptionsTable();
        }

        public void CreateDependentTables(App_Technician technician)
        {
            CreateDependentTables(GetTechnician(technician.TechnicianDeptNo, technician.TechnicianNo));
        }

        public bool TableExists<T>()
        {
            string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = _database.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

        public void CreateDependentTables(JT_Technician technician)
        {
            //add a modicum of version checking
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            decimal versionNo = Convert.ToDecimal(version.Substring(0, 3));

            //ci options are now crucial, so
            if (!TableExists<CI_Options>())
            {
                _database.CreateTable<CI_Options>();
                FillCIOptions();
            }

            try { _database.DropTable<JT_TechnicianScheduleDetail>(); } catch { }
            try { _database.DropTable<SO_SalesOrderHeader>(); } catch { }
            try { _database.DropTable<SO_ShipToAddress>(); } catch { }
            try { _database.DropTable<SO_SalesOrderDetail>(); } catch { }
            try { _database.DropTable<JT_WorkTicket>(); } catch { }
            try { _database.DropTable<JT_WorkTicketText>(); } catch { }
            try { _database.DropTable<JT_WorkTicketClass>(); } catch { }
            try { _database.DropTable<JT_WorkTicketHistory>(); } catch { }
            try { _database.DropTable<AR_Customer>(); } catch { }
            try { _database.DropTable<AR_CustomerContact>(); } catch { }
            try { _database.DropTable<JT_ServiceEquipmentParts>(); } catch { }
            //try { _database.DropTable<JT_DailyTimeEntry>(); } catch { }
            try { _database.DropTable<JT_EquipmentAsset>(); } catch { }
            try { _database.DropTable<JT_ServiceAgreementHeader>(); } catch { }
            try { _database.DropTable<JT_ServiceAgreementDetail>(); } catch { }
            try { _database.DropTable<JT_ServiceAgreementPMDetail>(); } catch { }
            try { _database.DropTable<JT_WorkTicketClass>(); } catch { }
            try { _database.DropTable<JT_Transaction>(); } catch { }
            try { _database.DropTable<JT_TransactionHistory>(); } catch { }
            try { _database.DropTable<JT_LaborText>(); } catch { }

            // dch rkl 01/13/2017 add CI_ExtendedDescription
            try { _database.DropTable<CI_ExtendedDescription>(); } catch { }

            if (!TableExists<JT_TransactionImportDetail>())
            {
                try { _database.DropTable<JT_TransactionImportDetail>(); } catch { }
                _database.CreateTable<JT_TransactionImportDetail>();
            }
            else
            {
                // dch rkl 11/30/2016
                // When data is pulled back from JobOps, remove any parts records in JT_TransactionImportDetail where
                // quantity used is zero.  These are the default parts, and they will get reloaded when the parts
                // screen is accessed for a ticket.  This will allow any changes in the Sales Order Detail table
                // for the default parts to get refreshed.
                List<JT_TransactionImportDetail> lsTrans = _database.Table<JT_TransactionImportDetail>().Where(
                    wt => (wt.RecordType == "P" && wt.QuantityUsed == 0)).ToList<JT_TransactionImportDetail>();
                foreach (JT_TransactionImportDetail tranDtl in lsTrans)
                {
                    DeleteExportRow(tranDtl);
                }
            }

            if (!TableExists<JT_DailyTimeEntry>())
            {
                try { _database.DropTable<JT_DailyTimeEntry>(); } catch { }
                _database.CreateTable<JT_DailyTimeEntry>();
            }

            //for later versions, check to see if these tables exist
            if (!TableExists<IM_ItemCost>())
            {
                try { _database.DropTable<IM_ItemCost>(); } catch { }
                _database.CreateTable<IM_ItemCost>();
            }

            // Next, create the tables       
            _database.CreateTable<JT_TechnicianScheduleDetail>();
            _database.CreateTable<SO_SalesOrderHeader>();
            _database.CreateTable<SO_SalesOrderDetail>();
            _database.CreateTable<SO_ShipToAddress>();
            _database.CreateTable<JT_WorkTicket>();
            _database.CreateTable<JT_WorkTicketText>();
            _database.CreateTable<JT_WorkTicketHistory>();
            _database.CreateTable<JT_WorkTicketClass>();
            _database.CreateTable<AR_Customer>();
            _database.CreateTable<AR_CustomerContact>();
            _database.CreateTable<JT_ServiceEquipmentParts>();
            //_database.CreateTable<JT_DailyTimeEntry>();
            _database.CreateTable<JT_EquipmentAsset>();
            _database.CreateTable<JT_ServiceAgreementHeader>();
            _database.CreateTable<JT_ServiceAgreementDetail>();
            _database.CreateTable<JT_ServiceAgreementPMDetail>();
            _database.CreateTable<JT_WorkTicketClass>();
            _database.CreateTable<JT_TransactionHistory>();
            _database.CreateTable<JT_Transaction>();
            _database.CreateTable<JT_LaborText>();

            // dch rkl 01/13/2017 add CI_ExtendedDescription
            _database.CreateTable<CI_ExtendedDescription>();

            // Fill the tables with data.
            FillTechnicianScheduleDetailTable(technician.TechnicianNo);            
           
            FillWorkTicketTable();

            // dch rkl 01/20/2017 This is done as part of work ticket load
            //FillWorkTicketTextTable();

            FillSalesOrderHeaderTable();
            FillShipToAddressTable();
            FillSalesOrderDetails();
            FillItemCostTable();
            FillCustomerTable();
            FillCustomerContactTable();
            FillServiceEquipmentPartsTable();
            FillDailyTimeEntryTable();
            //FillExpensesTableFromSdata(); TODO... not implemented yet.
            FillEquipmentAssetTable();
            FillServiceAgreementHeaderTable();
            FillWorkTicketClassTable();
            FillTransactionTable();
            FillTransactionHistoryTable();
            FillLaborTextTable();

            // dch rkl 01/13/2017 add CI_ExtendedDescription
            FillExtendedDescriptionTable();
        }


        #region WorkTicket

        public void FillWorkTicketTable()
        {
            StringBuilder sb = new StringBuilder();

            List<JT_TechnicianScheduleDetail> scheduledTickets = GetTechnicianScheduleDetailFromDB();
            for(int i  = 0; i < scheduledTickets.Count; i++)
            {
                if(i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(SalesOrderNo eq '");
                sb.Append(scheduledTickets[i].SalesOrderNo);
                sb.Append("' and WTNumber eq '");
                sb.Append(scheduledTickets[i].WTNumber);
                //sb.Append("' and WTStep eq '");
                //sb.Append(scheduledTickets[i].WTStep);
                sb.Append("')");
            }

            //FillLocalTable<JT_WorkTicket>("where", sb.ToString()); 
            //FillLocalTable<JT_WorkTicketHistory>("where", sb.ToString());
            FillLocalTable<JT_WorkTicket>();
            FillLocalTable<JT_WorkTicketHistory>();

            // dch rkl 01/20/2017
            FillLocalTable<JT_WorkTicketText>("where", sb.ToString());
        }



        public JT_WorkTicket GetWorkTicket(JT_TechnicianScheduleDetail scheduleDetail)
        {
            JT_WorkTicket workTicket = null;

            lock (_locker)
            {
                if (scheduleDetail != null)
                {
                    workTicket =
                        _database.Table<JT_WorkTicket>().Where(
                            wt => (wt.SalesOrderNo == scheduleDetail.SalesOrderNo) &&
                                  (wt.WTNumber == scheduleDetail.WTNumber) &&
                                  (wt.WTStep == scheduleDetail.WTStep)
                        ).FirstOrDefault();
                }
            }

            return workTicket;
        }

        //public JT_WorkTicket GetWorkTicket(string formattedWorkTicketNumber)
        //{
        //    // TODO... obsolete?
        //    JT_WorkTicket workTicket = null;

        //    lock (_locker)
        //    {
        //        workTicket =
        //            _database.Table<JT_WorkTicket>().Where(wt => wt.WTNumber == workTicketNumber).FirstOrDefault();
        //    }

        //    return workTicket;
        //}

        public JT_WorkTicket GetWorkTicket(string formattedWorkTicketNumber)
        {
            //JT_WorkTicket workTicket = null;
            JT_TechnicianScheduleDetail scheduleDetail = new JT_TechnicianScheduleDetail();
            string[] workTicketNumberParts = JT_WorkTicket.BreakFormattedTicketNumber(formattedWorkTicketNumber);
            scheduleDetail.SalesOrderNo = workTicketNumberParts[0];
            scheduleDetail.WTNumber = workTicketNumberParts[1];
            scheduleDetail.WTStep = workTicketNumberParts[2];

            return GetWorkTicket(scheduleDetail);

            //lock (_locker)
            //{
            //    workTicket = 
            //        _database.Table<JT_WorkTicket>().Where(
            //            wt => 
            //                (wt.SalesOrderNo == workTicketNumberParts[0]) &&
            //                (wt.WTNumber == workTicketNumberParts[1]) //&&
            //                //(wt.WTStep == workTicketNumberParts[2])
            //        ).FirstOrDefault();
            //}

            //return workTicket;
        }

        //public App_WorkTicket GetAppWorkTicket(string formattedWorkTicketNumber)
        //{
        //    //JT_WorkTicket workTicket = null;
        //    JT_TechnicianScheduleDetail scheduleDetail = new JT_TechnicianScheduleDetail();
        //    string[] workTicketNumberParts = JT_WorkTicket.BreakFormattedTicketNumber(formattedWorkTicketNumber);
        //    scheduleDetail.SalesOrderNo = workTicketNumberParts[0];
        //    scheduleDetail.WTNumber = workTicketNumberParts[1];
        //    scheduleDetail.WTStep = workTicketNumberParts[2];

        //    return GetWorkTicket(scheduleDetail);

        //    //lock (_locker)
        //    //{
        //    //    workTicket = 
        //    //        _database.Table<JT_WorkTicket>().Where(
        //    //            wt => 
        //    //                (wt.SalesOrderNo == workTicketNumberParts[0]) &&
        //    //                (wt.WTNumber == workTicketNumberParts[1]) //&&
        //    //                //(wt.WTStep == workTicketNumberParts[2])
        //    //        ).FirstOrDefault();
        //    //}

        //    //return workTicket;
        //}

        // TODO
        public List<JT_WorkTicket> GetWorkTicketsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_WorkTicket>().OrderBy(wt => wt.WTNumber).ToList();
            }
        }

        //public void SaveWorkTicketAsCurrent(JT_WorkTicket workTicket)
        //{
        //    //int rows = 0;

        //    JT_TechnicianScheduleDetail scheduleDetail = 
        //        _database.Table<JT_TechnicianScheduleDetail>().Where(
        //            sd => (sd.WTNumber == workTicket.WTNumber) &&
        //                  (sd.WTStep == workTicket.WTStep) &&
        //                  (sd.SalesOrderNo == workTicket.SalesOrderNo)                    
        //            ).FirstOrDefault();

        //    //App_CurrentSelectionData currentData = _database.Table<App_CurrentSelectionData>().FirstOrDefault();

        //    //currentData.SalesOrderNo = workTicket.SalesOrderNo;
        //    //currentData.WTNumber = workTicket.WTNumber;
        //    //currentData.WTStep = workTicket.WTStep;

        //    SaveScheduleDetailAsCurrent(scheduleDetail);
        //}

        

        public JT_WorkTicket RetrieveCurrentWorkTicket()
        {
            JT_WorkTicket currentTicket = null;

            lock (_locker)
            {
                //App_CurrentSelectionData currentData = _database.Table<App_CurrentSelectionData>().FirstOrDefault();

                //return _database.Table<JT_WorkTicket>().Where(
                //        wt => (wt.SalesOrderNo == currentData.SalesOrderNo) &&
                //              (wt.WTNumber == currentData.WTNumber) &&
                //              (wt.WTStep == currentData.WTStep)
                //    ).FirstOrDefault();
                JT_TechnicianScheduleDetail scheduleDetail = RetrieveCurrentScheduleDetail();

                if (scheduleDetail != null)
                {
                    currentTicket =
                        _database.Table<JT_WorkTicket>().Where(
                            wt => (wt.SalesOrderNo == scheduleDetail.SalesOrderNo) &&
                                  (wt.WTNumber == scheduleDetail.WTNumber) &&
                                  (wt.WTStep == scheduleDetail.WTStep)
                        ).FirstOrDefault();
                }
            }

            return currentTicket;
        }

        //public App_CurrentWorkTicket RetrieveCurrentAppWorkTicket()
        //{
        //    lock (_locker)
        //    {
        //        return new App_CurrentWorkTicket(); // TODO... db stuff handled in class.  Should we remove and put here?
        //    }
        //}

        #endregion

        #region Work Ticket Text

        // dch rkl 01/20/2017 This is done as part of work ticket load
        //public void FillWorkTicketTextTable()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    List<JT_TechnicianScheduleDetail> scheduledTickets = GetTechnicianScheduleDetailFromDB();
        //    for (int i = 0; i < scheduledTickets.Count; i++)
        //    {
        //        if (i > 0)
        //        {
        //            sb.Append(" or ");
        //        }
        //        sb.Append("(SalesOrderNo eq '");
        //        sb.Append(scheduledTickets[i].SalesOrderNo);
        //        sb.Append("' and WTNumber eq '");
        //        sb.Append(scheduledTickets[i].WTNumber);
        //        sb.Append("' and WTStep eq '");
        //        sb.Append(scheduledTickets[i].WTStep);
        //        sb.Append("')");
        //    }

        //    FillLocalTable<JT_WorkTicketText>("where", sb.ToString()); 
        //}

        public App_WorkTicketText RetrieveTextFromWorkTicket(App_WorkTicket workTicket)
        {
            JT_WorkTicketText note = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    note =
                        _database.Table<JT_WorkTicketText>().Where(
                            wtt => (wtt.SalesOrderNo == workTicket.SalesOrderNo) &&
                                   (wtt.WTNumber == workTicket.WTNumber) &&
                                   (wtt.WTStep == workTicket.WTStep)
                        ).FirstOrDefault();
                }
            }

            if (note == null)
            {
                return null;
            }
            else
            {
                return new App_WorkTicketText(note);
            }
        }

        public JT_WorkTicketText RetrieveTextFromWorkTicket(JT_WorkTicket workTicket)
        {
            JT_WorkTicketText note = null;

            lock (_locker)
            {
                if (workTicket != null)
                {
                    note =
                        _database.Table<JT_WorkTicketText>().Where(
                            wtt => (wtt.SalesOrderNo == workTicket.SalesOrderNo) &&
                                   (wtt.WTNumber == workTicket.WTNumber) &&
                                   (wtt.WTStep == workTicket.WTStep)
                        ).FirstOrDefault();
                }
            }

            return note;
        }

        

        public JT_WorkTicketText RetrieveTextFromCurrentWorkTicket()
        {
            JT_WorkTicketText note = null;

            lock (_locker)
            {
                JT_TechnicianScheduleDetail scheduleDetail = RetrieveCurrentScheduleDetail();

                if (scheduleDetail != null)
                {
                    note = 
                        _database.Table<JT_WorkTicketText>().Where(
                            wtt => (wtt.SalesOrderNo == scheduleDetail.SalesOrderNo) &&
                                   (wtt.WTNumber == scheduleDetail.WTNumber) &&
                                   (wtt.WTStep == scheduleDetail.WTStep)
                        ).FirstOrDefault();
                }
            }

            return note;
        }

        public void SaveWorkTicketText(App_WorkTicketText workTicketText)
        {
            int rows = 0;

            lock (_locker)
            {
				JT_WorkTicketText workTicketToSave = new JT_WorkTicketText() {
					WTNumber = workTicketText.WTNumber,
					WTStep = workTicketText.WTStep,
					Text = workTicketText.Text,
					SequenceNo = workTicketText.SequenceNo,
					SalesOrderNo = workTicketText.SalesOrderNo,
					IsModified = true,
					ID = workTicketText.ID
				};

                JT_TransactionImportDetail transactionDetail = new JT_TransactionImportDetail()
                {
                    RecordType = "W",
                    SalesOrderNo = workTicketText.SalesOrderNo,
                    WTNumber = workTicketText.WTNumber,
                    WTStep = workTicketText.WTStep,
                    TransactionDate = DateTime.Now.ToString("yyyyMMdd"),        // dch rkl 11/03/2016 include transaction date for notes
                StepText = workTicketText.Text
                };

                _database.Insert(transactionDetail);

				SaveWorkTicketText(workTicketToSave);
                /*workTicketText.IsModified = true;
                // TODO
                //JT_WorkTicketText itemToSave;
                

				if (workTicketText.WTNumber != null && workTicketText.WTStep != null)
                {
                    rows = _database.Update(workTicketText);
                }
                else
                {
                    rows = _database.Insert(workTicketText);
                }

                // TODO... need call back?*/
            }
        }

        public void SaveWorkTicketText(JT_WorkTicketText workTicketText)
        {
            int rows = 0;

            lock (_locker)
            {
                workTicketText.IsModified = true;

                if (workTicketText.ID != 0)
                {
                    rows = _database.Update(workTicketText);
                }
                else
                {
                    rows = _database.Insert(workTicketText);
                }

                // TODO... need call back?
            }
        }

        #endregion

        #region CI_Options

        public void FillCIOptions()
        {
            FillLocalTable<CI_Options>();
        }

        #endregion

        #region Default Sales Order Parts

        public void FillSalesOrderDetails()
        {
            StringBuilder sb = new StringBuilder();
            List<JT_WorkTicket> workTickets = GetWorkTicketsFromDB().ToList();

            for (int i = 0; i < workTickets.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(");
                sb.Append("SalesOrderNo eq '" + workTickets[i].SalesOrderNo + "' AND ");
                sb.Append("JT158_WTNumber eq '" + workTickets[i].WTNumber + "' AND ");
                sb.Append("JT158_WTStep eq '" + workTickets[i].WTStep + "' AND ");
                sb.Append("JT158_WTPart eq 'Y')"); //AND ");
                //sb.Append("JT158_WTBillFlag ne 'B' AND ");
                //sb.Append("JT158_WTPurchase eq 'Y')");
            }
            //FillLocalTable<SO_SalesOrderDetail>("where", sb.ToString());
            FillLocalTable<SO_SalesOrderDetail>();
        }

        #endregion

        #region Service Equipment Parts

        public void FillServiceEquipmentPartsTable()
        {
            StringBuilder sb = new StringBuilder();

            List<JT_WorkTicket> workTickets = GetWorkTicketsFromDB().ToList();

            for (int i = 0; i < workTickets.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" or ");
                }
                sb.Append("(ItemCode eq '");
                sb.Append(workTickets[i].DtlRepairItemCode);
                sb.Append("')");
            }

            FillLocalTable<JT_ServiceEquipmentParts>("where", sb.ToString()); 
        }

        

        public List<JT_ServiceEquipmentParts> RetrievePartsListFromWorkTicket(JT_WorkTicket workTicket)
        {
            lock (_locker)
            {
                //App_CurrentSelectionData currentData = _database.Table<App_CurrentSelectionData>().FirstOrDefault();
                List<JT_ServiceEquipmentParts> partsList = null;
                CI_Item repairItem = RetrieveRepairItemFromWorkTicket(workTicket);

                if (repairItem != null)
                {
                    partsList = _database.Table<JT_ServiceEquipmentParts>().Where(
                             part => repairItem.ItemCode.Contains(part.ItemCode)
                         ).ToList();
                }

                return partsList;
            }
        }

        public List<JT_ServiceEquipmentParts> RetrievePartsListFromCurrentWorkTicket()
        {
            lock (_locker)
            {
                List<JT_ServiceEquipmentParts> partsList = null;
                CI_Item repairItem = RetrieveRepairItemFromCurrentWorkTicket();
                if (repairItem != null) 
                {
                    partsList = _database.Table<JT_ServiceEquipmentParts>().Where(
                             part => repairItem.ItemCode.Contains(part.ItemCode)
                         ).ToList();
                }

                return partsList;
            }
        }

        public bool IsPartOnPartsList(JT_WorkTicket workTicket, CI_Item partToFind)
        {
            bool returnValue = false;

            lock (_locker)
            {
                try
                {
                    returnValue = (RetrievePartsListFromWorkTicket(workTicket).Find(part => part.PartItemCode == partToFind.ItemCode) != null);
                }
                catch
                {
                    returnValue = false;
                }                
            }

            return returnValue;
        }

        public void AddItemToPartsList(App_WorkTicket workTicket, CI_Item partToAdd)
        {
            int rows = 0;

            lock(_locker)
            {
                JT_ServiceEquipmentParts newPart = new JT_ServiceEquipmentParts();                
                CI_Item repairItem = RetrieveRepairItemFromWorkTicket(workTicket);

                newPart.ItemCode = repairItem.ItemCode;
                newPart.PartItemCode = partToAdd.ItemCode;
                newPart.ProblemCode = "Problems";
                newPart.IsModified = true;

                rows = _database.Insert(newPart);
            }
        }

        public void UpdatePartOnPartsList(CI_Item partToUpdate)
        {
            // TODO
        }

        #endregion

        public void UploadDataToSage()
        {
            //TODO
            throw new NotImplementedException();
        }

        #region Classification Code

        public void FillClassificationCodeTable()
        {
            FillLocalTable<JT_ClassificationCode>();  
        }

        #endregion

        #region Work Ticket Class

        public void FillWorkTicketClassTable()
        {
            FillLocalTable<JT_WorkTicketClass>(); 
        }

        #endregion

		#region Transaction and Transaction history

		public void FillTransactionTable()
		{
			FillLocalTable<JT_Transaction>();
		}

		public void FillTransactionHistoryTable()
		{
			FillLocalTable<JT_TransactionHistory>();
		}

		public void FillLaborTextTable()
		{
			FillLocalTable<JT_LaborText>();
		}

        #endregion

        public List<JT_TransactionImportDetail> GetCurrentExport()
        {
            // dch rkl 10/14/2016 exclude parts with a quantity of zero.  These are the default parts.
            List<JT_TransactionImportDetail> lsTrans = _database.Table<JT_TransactionImportDetail>().Where(
            wt => (wt.RecordType != "P" ||
                  (wt.RecordType == "P" && wt.QuantityUsed != 0))).ToList<JT_TransactionImportDetail>();
            return lsTrans;
           // return _database.Table<JT_TransactionImportDetail>().ToList<JT_TransactionImportDetail>();
        }
        
        public void DeleteExportRow(JT_TransactionImportDetail transaction)
        {
            _database.Delete<JT_TransactionImportDetail>(transaction.ID); 
        }
    }
}
