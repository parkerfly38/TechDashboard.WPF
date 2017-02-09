using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    /*********************************************************************************************************
     * TechDashboardDB_Codes.cs
     * 12/01/2016 DCH Added IM_Warehouse table, JT_CustomerBillingRates table
     * 01/20/2017 DCH Added CI_UnitOfMeasure table.
     *********************************************************************************************************/
    public partial class TechDashboardDatabase
    {
        #region Miscellaneous Codes

        public void FillMiscellaneousCodesTable()
        {
            FillLocalTable<JT_MiscellaneousCodes>(string.Empty, string.Empty);
        }

        /// <summary>
        /// Retrieves a list of all JT_MiscellaneousCodes objects from the local database table.
        /// </summary>
        /// <returns>A List container of JT_MiscellaneousCodes objects.</returns>
        public List<JT_MiscellaneousCodes> GetMiscellaneousCodesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_MiscellaneousCodes>().OrderBy(code => code.MiscellaneousCode).ToList();
            }
        }

        public List<JT_MiscellaneousCodes> GetMiscellaneousCodesFromDB(string codeType)
        {
            lock (_locker)
            {
                return _database.Table<JT_MiscellaneousCodes>().Where(code => (code.RecordType == "M") && (code.MiscellaneousCode == codeType)).OrderBy(code => code.MiscellaneousCode).ToList();
            }
        }

        public List<JT_MiscellaneousCodes> GetMiscellaneousCodesFromDB(string recordType, string codeType)
        {
            if (recordType == null)
            {
                recordType = "M";
            }

            lock (_locker)
            {
                return _database.Table<JT_MiscellaneousCodes>().Where(code => (code.RecordType == recordType) && (code.CodeType == codeType)).OrderBy(code => code.MiscellaneousCode).ToList();
            }
        }

        public JT_MiscellaneousCodes GetMiscellaneousCodeFromDB(string codeType, string miscellaneousCode)
        {
            return GetMiscellaneousCodeFromDB("M", codeType, miscellaneousCode);
        }

        public JT_MiscellaneousCodes GetMiscellaneousCodeFromDB(string recordType, string codeType, string miscellaneousCode)
        {
            lock (_locker)
            {
                var codes = GetMiscellaneousCodesFromDB(recordType, codeType);

                return codes.Where(code => code.MiscellaneousCode == miscellaneousCode).FirstOrDefault();
            }
        }

        #endregion

        #region Work Ticket Status Code

        /// <summary>
        /// Retreives all work ticket statuses from the local database table.
        /// </summary>
        /// <returns>A List collection of JT_MiscellaneousCodes objects.</returns>
        public List<JT_MiscellaneousCodes> GetWorkTicketStatusesFromDB()
        {
            lock (_locker)
            {
                return GetMiscellaneousCodesFromDB("ST");
            }
        }

        /// <summary>
        /// Retrieves the work ticket status code from the local database.
        /// </summary>
        /// <param name="miscellaneousCode">Miscellaneous Code value</param>
        /// <returns>A JT_MiscellaneousCodes object representing the specified status code.</returns>
        public JT_MiscellaneousCodes GetWorkTicketStatus(string miscellaneousCode)
        {
            lock (_locker)
            {
                return GetMiscellaneousCodeFromDB("M", "ST", miscellaneousCode);
            }
        }

        #endregion

        #region Problem Codes

        public List<string> GetProblemCodes()
        {
            lock (_locker)
            {
                var problemCodes = from codes in GetMiscellaneousCodesFromDB("PC")
                                   select codes.MiscellaneousCode;

                return problemCodes.ToList();
            }
        }

        public JT_MiscellaneousCodes GetProblemCode(string miscellaneousCode)
        {
            lock (_locker)
            {
                return GetMiscellaneousCodeFromDB("M", "PC", miscellaneousCode);
            }
        }

        #endregion

        #region Earnings Codes

        public void FillEarningsCodesTable()
        {
            FillLocalTable<JT_EarningsCode>(string.Empty, string.Empty);
        }

        /// <summary>
        /// Retrieves a list of all JT_EarningsCode objects from the local database table.
        /// </summary>
        /// <returns>A List container of JT_EarningsCode objects.</returns>
        public List<JT_EarningsCode> GetEarningsCodesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_EarningsCode>().OrderBy(code => code.EarningsCode).ToList();
            }
        }

        public List<JT_EarningsCode> GetEarningsCodesFromDBforMisc()
        {
            lock (_locker)
            {
                return _database.Table<JT_EarningsCode>().Where(x => x.TypeOfEarnings != "O" && x.TypeOfEarnings != "R").OrderBy(code => code.EarningsCode).ToList();
            }
        }

        #endregion

        #region Activity Codes

        public void FillActivityCodesTable()
        {
            FillLocalTable<JT_ActivityCode>(string.Empty, string.Empty);
        }

        public List<JT_ActivityCode> GetActivityCodesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_ActivityCode>().OrderBy(ac => ac.ActivityCode).ToList();
            }
        }

        public JT_ActivityCode GetActivityCodeFromDB(string activityCode)
        {
            JT_ActivityCode codeToReturn = null;

            lock (_locker)
            {
                if (activityCode != null)
                {
                    codeToReturn =
                        _database.Table<JT_ActivityCode>().Where(
                            ac => (ac.ActivityCode == activityCode)
                        ).FirstOrDefault();
                }                
            }

            return codeToReturn;
        }

        #endregion

        // dch rkl 10/31/2016 Add JT_Options
        #region JT_Options

        public void FillJTOptionsTable()
        {
            FillLocalTable<JT_Options>(string.Empty, string.Empty);
        }

        public List<JT_Options> GetJTOptionsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_Options>().OrderBy(code => code.ModuleCode).ToList();
            }
        }

        #endregion

        // dch rkl 10/31/2016 Add PR_EarningsDeduction
        #region PR_EarningsDeduction

        public void FillPREarningsDeductionTable()
        {
            try
            {
                FillLocalTable<PR_EarningsDeduction>("where", "RecordType eq 'E'");
            }
            catch (Exception ex)
            { }
        }

        public List<PR_EarningsDeduction> GetPREarningsDeductionFromDB()
        {
            lock (_locker)
            {
                return _database.Table<PR_EarningsDeduction>().Where(x => x.RecordType != "e").OrderBy(code => code.EarningsCode).ToList();
            }
        }

        #endregion

        // dch rkl 11/03/2016 Add JT_TimeTrackerOptions
        #region JT_TimeTrackerOptions

        public void FillJTTimeTrackerOptions()
        {
            try
            {
                FillLocalTable<JT_TimeTrackerOptions>("where", "ModuleCode eq 'J/T'");
            }
            catch (Exception ex)
            { }
        }

        public List<JT_TimeTrackerOptions> GetJTTimeTrackerOptionsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_TimeTrackerOptions>().ToList();
            }
        }

        #endregion

        // dch rkl 11/16/2016 Add SO_Options
        #region SO_Options

        public void FillSOOptionsTable()
        {
            FillLocalTable<SO_Options>(string.Empty, string.Empty);
        }

        public List<SO_Options> GetSOOptionsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<SO_Options>().OrderBy(code => code.ModuleCode).ToList();
            }
        }

        #endregion

        // dch rkl 12/01/2016 Add IM_Warehouse
        #region IM_Warehouse

        public void FillIMWarehouseTable()
        {
            FillLocalTable<IM_Warehouse>(string.Empty, string.Empty);
        }

        public List<IM_Warehouse> GetIMWarehouseFromDB()
        {
            lock (_locker)
            {
                return _database.Table<IM_Warehouse>().OrderBy(code => code.WarehouseCode).ToList();
            }
        }

        #endregion

        // dch rkl 12/01/2016 Add JT_CustomerBillingRates
        #region JT_CustomerBillingRates

        public void FillJT_CustomerBillingRatesTable()
        {
            FillLocalTable<JT_CustomerBillingRates>(string.Empty, string.Empty);
        }

        public List<JT_CustomerBillingRates> GetJT_CustomerBillingRatesFromDB()
        {
            lock (_locker)
            {
                return _database.Table<JT_CustomerBillingRates>().OrderBy(code => code.CustomerNo).ToList();
            }
        }

        public JT_CustomerBillingRates GetJT_CustomerBillingRate(string divisionNo, string customerNo, string activityCode)
        {
            JT_CustomerBillingRates custBillRate = null;

            lock (_locker)
            {
                custBillRate =
                    _database.Table<JT_CustomerBillingRates>().Where(
                        ac => (ac.ARDivisionNo == divisionNo && ac.CustomerNo == customerNo && ac.ActivityCode == activityCode)
                        ).FirstOrDefault();
            }

            return custBillRate;
        }

        #endregion


        // dch rkl 01/20/2017 Add CI_UnitOfMeasure
        #region CI_UnitOfMeasure

        public void FillCI_UnitOfMeasureTable()
        {
            FillLocalTable<CI_UnitOfMeasure>(string.Empty, string.Empty);
        }

        public List<CI_UnitOfMeasure> GetCI_UnitOfMeasureFromDB()
        {
            lock (_locker)
            {
                return _database.Table<CI_UnitOfMeasure>().OrderBy(code => code.UnitOfMeasure).ToList();
            }
        }

        public CI_UnitOfMeasure GetCI_UnitOfMeasure(string umCode)
        {
            CI_UnitOfMeasure unitOfMeas = null;

            lock (_locker)
            {
                unitOfMeas =
                    _database.Table<CI_UnitOfMeasure>().Where(
                        um => (um.UnitOfMeasure == umCode)
                        ).FirstOrDefault();
            }

            return unitOfMeas;
        }

        #endregion

        // dch rkl 01/23/2017 Add AR_Options
        #region AR_Options

        public void FillAR_OptionsTable()
        {
            FillLocalTable<AR_Options>(string.Empty, string.Empty);
        }

        public List<AR_Options> GetAR_OptionsFromDB()
        {
            lock (_locker)
            {
                return _database.Table<AR_Options>().OrderBy(code => code.ModuleCode).ToList();
            }
        }

        public AR_Options GetAR_Options(string moduleCode)
        {
            AR_Options arOpt = null;

            lock (_locker)
            {
                arOpt =
                    _database.Table<AR_Options>().Where(
                        ao => (ao.ModuleCode == moduleCode)
                        ).FirstOrDefault();
            }

            return arOpt;
        }

        #endregion
    }
}
