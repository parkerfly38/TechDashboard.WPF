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
                return _database.Table<JT_MiscellaneousCodes>().Where(code => (code.RecordType == "M") && (code.CodeType == codeType)).OrderBy(code => code.MiscellaneousCode).ToList();
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
    }
}
