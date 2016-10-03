using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace TechDashboard.Models
{
    public class JT_WorkTicket
    {
        /// <summary>
        /// Sales Order Number - varchar(7)
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// Work Ticket Number - varchar(3)
        /// </summary>
        public string WTNumber { get; set; }

        /// <summary>
        /// Work Ticket Step - varcar(3)
        /// </summary>
        public string WTStep { get; set; }

        /// <summary>
        /// Detail Manufacturer Serial Number - varchar(20)
        /// </summary>
        public string DtlMfgSerialNo { get; set; }

        /// <summary>
        /// Work Ticket Description - varchar(30)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Contact Code - varchar(10)
        /// </summary>
        public string HdrContactCode { get; set; }

        /// <summary>
        /// Repair Item Code - varchar(30)
        /// </summary>
        public string DtlRepairItemCode { get; set; }

        /// <summary>
        /// Coverage Exception Code - varchar(10)
        /// </summary>
        public string DtlCoverageExceptionCode { get; set; }

        /// <summary>
        /// Serial Number - varchar(12)
        /// </summary>
        public string DtlInternalSerialNo { get; set; }

        /// <summary>
        /// Service Contract Code - varchar(10)
        /// </summary>
        public string HdrServiceContractCode { get; set; }

        /// <summary>
        /// Status Code - varchar(3)
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Warranty Repair Y/N - varchar(1)
        /// </summary>
        public string DtlWarrantyRepair { get; set; }

        /// <summary>
        /// Covered on Contract Y/N - varchar(1)
        /// </summary>
        public string DtlCoveredOnContract { get; set; }

        /// <summary>
        /// Preventative Maintenance Y/N - varchar(1)
        /// </summary>
        public string DtlPreventitiveMaintenance { get; set; }

        /// <summary>
        /// Problem Code - varchar(10)
        /// </summary>
        public string DtlProblemCode { get; set; }

        /// <summary>
        /// Activity Code - varchar(4)
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// Header Template Number - varchar(30)
        /// </summary>
        public string HdrTemplateNo { get; set; }

        /// <summary>
        /// Header Work Ticket Class - varchar(3)
        /// </summary>
        public string HdrWtClass { get; set; }


        public JT_WorkTicket()
        {
            // empty
        }

        public JT_WorkTicket(JT_WorkTicket workTicket) 
        {
            this.SalesOrderNo = workTicket.SalesOrderNo;
            this.WTNumber = workTicket.WTNumber;
            this.WTStep = workTicket.WTStep;
            this.Description = workTicket.Description;
            this.HdrContactCode = workTicket.HdrContactCode;
            this.DtlRepairItemCode = workTicket.DtlRepairItemCode;
            this.DtlInternalSerialNo = workTicket.DtlInternalSerialNo;
            this.HdrServiceContractCode = workTicket.HdrServiceContractCode;
            this.StatusCode = workTicket.StatusCode;
            this.DtlWarrantyRepair = workTicket.DtlWarrantyRepair;
            this.DtlMfgSerialNo = workTicket.DtlMfgSerialNo;
            this.DtlCoveredOnContract = workTicket.DtlCoveredOnContract;
            this.DtlPreventitiveMaintenance = workTicket.DtlPreventitiveMaintenance;
            this.ActivityCode = workTicket.ActivityCode;
            this.DtlCoverageExceptionCode = workTicket.DtlCoverageExceptionCode;
        }

        [Ignore]
        public string FormattedTicketNo
        {
            get
            {
                return SalesOrderNo + "-" + WTNumber + "-" + WTStep;
            }
        }

        /// <summary>
        /// Breaks apart (or unformats) the formatted ticket number provided.
        /// </summary>
        /// <param name="formattedTicketNumber">Formatted work ticket number</param>
        /// <returns>String array with the following indexes:
        /// 0 = Sales Order Number,
        /// 1 = Work Ticket Number
        /// 2 = Work Ticket Step</returns>
        public static string[] BreakFormattedTicketNumber(string formattedTicketNumber)
        {
            return formattedTicketNumber.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
