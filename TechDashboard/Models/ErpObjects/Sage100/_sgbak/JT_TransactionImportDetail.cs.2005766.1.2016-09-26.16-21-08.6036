using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_TransactionImportDetail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Record Type - varchar(1)
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Transaction Date - varchar(8)
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// DateTime representation of the TransactionDate property
        /// </summary>
        [Ignore]
        public DateTime TransactionDateAsDateTime
        {
            get { return ConvertStringToDateTime(TransactionDate); }
            set { TransactionDate = value.ToString("yyyyMMdd"); }
        }

        /// <summary>
        /// Department Number - varchar(2)
        /// </summary>
        public string EmployeeDeptNo { get; set; }

        /// <summary>
        /// Employee Number - varchar(7)
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Work Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Work Ticket Step - varchar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Service Agreement Contract Code - varchar(10)
        /// </summary>
        public string SvcAgrmContractCode { get; set; }

        /// <summary>
        /// Manufacturer Serial Number - varchar(20)
        /// </summary>
        public string MfgSerialNo { get; set; }

        /// <summary>
        /// Repair Item Code - varchar(30)
        /// </summary>
        public string RepairItemCode { get; set; }

        /// <summary>
        /// Step Text - varchar(2048)
        /// </summary>
        public string StepText { get; set; }

        /// <summary>
        /// Problem Code - varchar(10)
        /// </summary>
        public string ProblemCode { get; set; }

        /// <summary>
        /// Exception Code - varchar(10)
        /// </summary>
        public string ExceptionCode { get; set; }

        /// <summary>
        /// Resolution Code - varchar(10)
        /// </summary>
        public string ResolutionCode { get; set; }

        /// <summary>
        /// Start Time (hhmm) - varchar(4)
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// End Time (hhmm) - varchar(4)
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Overtime - varchar(1)
        /// </summary>
        public string Overtime { get; set; }

        /// <summary>
        /// Earnings Code - varchar(2)
        /// </summary>
        public string EarningsCode { get; set; }

        /// <summary>
        /// Department Worked In - varchar(2)
        /// </summary>
        public string DeptWorkedIn { get; set; }

        /// <summary>
        /// Activity Code [ =MSG("FT_ACT") ] - varchar(4)
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// Meter Reading
        /// </summary>
        public double MeterReading { get; set; }

        /// <summary>
        /// Work Ticket History - varchar(1)
        /// </summary>
        public string WorkTicketHistory { get; set; }

        /// <summary>
        /// Work Ticket Status Code - varchar(3)
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Status Comment - varchar(30)
        /// </summary>
        public string StatusComment { get; set; }

        /// <summary>
        /// Status Time (hhmm) - varchar(4)
        /// </summary>
        public string StatusTime { get; set; }

        /// <summary>
        /// Scrap Reason Code - varchar(6)
        /// </summary>
        public string ScrapReasonCode { get; set; }

        /// <summary>
        /// Scrap Comment - varchar(30)
        /// </summary>
        public string ScrapComment { get; set; }

        /// <summary>
        /// Work Performed - varchar(2048)
        /// </summary>
        public string WorkPerformed { get; set; }

        /// <summary>
        /// Item Code - varchar(30)
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Item Code Description - varchar(2048)
        /// </summary>
        public string ItemCodeDesc { get; set; }

        /// <summary>
        /// Comment Text - varchar(2048)
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Warehouse Code - varchar(3)
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Reimburse Employee - varchar(1)
        /// </summary>
        public string ReimburseEmployee { get; set; }

        /// <summary>
        /// Charge Part - varchar(1)
        /// </summary>
        public string ChargePart { get; set; }

        /// <summary>
        /// Print Part - varchar(1)
        /// </summary>
        public string PrintPart { get; set; }

        /// <summary>
        /// Purchase Part - varchar(1)
        /// </summary>
        public string PurchasePart { get; set; }

        /// <summary>
        /// Overhead on Part - varchar(1)
        /// </summary>
        public string Overhead { get; set; }

        /// <summary>
        /// Lot Serial Number - varchar(15)
        /// </summary>
        public string LotSerialNo { get; set; }

        /// <summary>
        /// Unit Of Measure - varchar(4)
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Find Number - varchar(5)
        /// </summary>
        public string FindNo { get; set; }

        /// <summary>
        /// Hours Worked
        /// </summary>
        public double HoursWorked { get; set; }

        /// <summary>
        /// Quantity Completed
        /// </summary>
        public double QuantityCompleted { get; set; }

        /// <summary>
        /// Quantity Scrapped
        /// </summary>
        public double QuantityScrapped { get; set; }

        /// <summary>
        /// Quantity Required
        /// </summary>
        public double QuantityRequired { get; set; }

        /// <summary>
        /// Quantity Used
        /// </summary>
        public double QuantityUsed { get; set; }

        /// <summary>
        /// Unit Cost
        /// </summary>
        public double UnitCost { get; set; }

        /// <summary>
        /// Unit Price
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// Unit Of Measure Conversion Factor
        /// </summary>
        public double UnitOfMeasureConvFactor { get; set; }

        /// <summary>
        /// Import Record Valid - varchar(1)
        /// </summary>
        public string ImportRecordValid { get; set; }

        /// <summary>
        /// PUKE
        /// </summary>
        public string BillingDescription { get; set; }

        protected DateTime ConvertStringToDateTime(string dateAsString)
        {
            return ConvertStringToDateTime(dateAsString, null);
        }

        protected DateTime ConvertStringToDateTime(string dateAsString, string timeAsSTring)
        {
            DateTime returnValue;

            // puke... depends on format
            try
            {
                int year = int.Parse(dateAsString.Substring(0, 4));
                int month = int.Parse(dateAsString.Substring(4, 2));
                int day = int.Parse(dateAsString.Substring(6, 2));
                int hour = 0;
                int minute = 0;
                if ((timeAsSTring != null) && (timeAsSTring.Length == 4))
                {
                    hour = int.Parse(timeAsSTring.Substring(0, 2));
                    minute = int.Parse(timeAsSTring.Substring(2, 2));
                }

                returnValue = new DateTime(year, month, day, hour, minute, 0); 
            }
            catch
            {
                returnValue = new DateTime();
            }

            return returnValue;
        }
    }
}
