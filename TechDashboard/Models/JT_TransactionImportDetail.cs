using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_TransactionImportDetail : Rkl.Erp.Sage.Sage100.TableObjects.JT_TransactionImportDetail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}
