using System;
using System.Collections.Generic;
using System.Text;

using Sage.SData.Client;
using SQLite;
using System.Linq;
using TechDashboard.Models;

/**************************************************************************************************
 * Page Name    TechDashboardDB_ExtendedDescription
 * Description: Extended Description Database Functions
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 01/13/2017   DCH     Created
 **************************************************************************************************/
namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region ExtendedDescription

        protected void FillExtendedDescriptionTable()
        {
            StringBuilder sb = new StringBuilder();

            List<CI_Item> items = GetItemsFromDB();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ExtendedDescriptionKey > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" or ");
                    }
                    string edky = items[i].ExtendedDescriptionKey.ToString();
                    while (edky.Length < 10) { edky = "0" + edky; }
                    sb.Append("(ExtendedDescriptionKey eq '");
                    sb.Append(edky);
                    sb.Append("')");
                }
            }

            FillLocalTable<CI_ExtendedDescription>("where", sb.ToString());
        }

        /// <summary>
        /// Retreives a specific extended description from the local database's CI_ExtendedDescription
        /// table using a given ExtendedDescriptionKey
        /// </summary>
        /// <param name="ExtendedDescriptionKey">The extended description key</param>
        /// <returns>A CI_ExtendedDescription object for the given extended description key.</returns>
        public CI_ExtendedDescription GetExtendedDescription(int ExtendedDescriptionKey)
        {
            lock (_locker)
            {
                return _database.Table<CI_ExtendedDescription>().Where(descr => descr.ExtendedDescriptionKey == ExtendedDescriptionKey).FirstOrDefault();
            }
        }

        #endregion
    }
}
