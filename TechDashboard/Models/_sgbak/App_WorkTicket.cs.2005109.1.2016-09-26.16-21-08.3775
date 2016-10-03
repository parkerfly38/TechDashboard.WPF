using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_WorkTicket
    {
        private JT_WorkTicket _workTicket;
        private JT_WorkTicket _workTicketStepZero;
        private JT_WorkTicketClass _workTicketClass;
        private App_RepairItem _repairItem;
        private App_ServiceAgreement _serviceAgreement;

        private JT_ClassificationCode _problemCode;
        private JT_ClassificationCode _coverageExceptionCode;


        private string _hdrServiceContractCode;
        private string _statusCode;
        private string _statusDescription;
        private string _dtlWarrantyRepair;
        private string _dtlCoveredOnContract;
        private string _dtlMfgSerialNo;



        private string _activityCode;





        /// <summary>
        /// Work Ticket Number formatted for display
        /// </summary>
        public string FormattedTicketNumber
        {
            get { return FormatWorkTicketNumber(SalesOrderNo, WTNumber, WTStep); }
        }

        /// <summary>
        /// Sales Order Number 
        /// </summary>
        public string SalesOrderNo
        {
            get { return _workTicket.SalesOrderNo; }
        }

        /// <summary>
        /// Work Ticket Number
        /// </summary>
        public string WTNumber
        {
            get { return _workTicket.WTNumber; }
        }

        /// <summary>
        /// Work Ticket Step
        /// </summary>
        public string WTStep
        {
            get { return _workTicket.WTStep; }
        }

        /// <summary>
        /// Work Ticket Description 
        /// </summary>
        public string WtStepDescription
        {
            get { return _workTicket.Description; }
        }

        /// <summary>
        /// Work Ticket Description 
        /// </summary>
        public string Description
        {
            get { return _workTicketStepZero.Description; }
        }

        // puke... work ticket status descriptioin

        /// <summary>
        /// Service agreement for this work ticket (should come from Step '000')
        /// </summary>
        public App_ServiceAgreement ServiceAgreement
        {
            get { return _serviceAgreement; }
        }

        /// <summary>
        /// The repair item for this work ticket
        /// </summary>
        public App_RepairItem RepairItem
        {
            get { return _repairItem; }
        }

        /// <summary>
        /// Manufacturer Serial Number
        /// </summary>
        public string ManufacturerSerialNumber
        {
            // puke... shouldn't this come from repair item?
            get { return _workTicket.DtlMfgSerialNo; }
        }
        /// <summary>
        /// Internal Serial Number
        /// </summary>
        public string InternalSerialNumber
        {
            // puke... shouldn't this come from repair item?
            get { return _workTicket.DtlInternalSerialNo; }
        }

        /// <summary>
        /// Flag denoting if this is a warranty repair.
        /// Returns true if JT_WorkTicket.DtlWarrantyRepair == "Y"
        /// </summary>
        public bool IsWarrantyRepair
        {
            get
            {
                return ((_workTicket.DtlWarrantyRepair != null) &&
                    (_workTicket.DtlWarrantyRepair.Trim().ToUpper() == "Y"));
            }
        }

        /// <summary>
        /// Flag denoting if the parts are covered under warranty
        /// </summary>
        public bool ArePartsCoveredOnWarranty
        {
            // puke... add logic for dates
            get
            {
                return ((_workTicket.DtlWarrantyRepair != null) &&
                    (_workTicket.DtlWarrantyRepair.Trim().ToUpper() == "Y"));
            }
        }

        /// <summary>
        /// Flag denoting if this ticket is a service agreement repair.
        /// Returns true if JT_WorkTicket.DtlCoveredOnContract == "Y"
        /// </summary>
        public bool IsServiceAgreementRepair
        {
            get
            {
                return ((_workTicket.DtlCoveredOnContract != null) &&
                    (_workTicket.DtlCoveredOnContract.Trim().ToUpper() == "Y"));
            }
        }

        public bool IsCoveredOnContract
        {
            get
            {
                return ((_workTicket.DtlCoveredOnContract != null) &&
                    (_workTicket.DtlCoveredOnContract.Trim().ToUpper() == "Y"));
            }
        }

        public bool ArePartsCoveredOnSerivceAgreement
        {
            get
            {
                if (IsPreventativeMaintenance)
                {
                    return ServiceAgreement.ArePartsCovered;
                }

                // if we get here, this is not prev. maint.
                if (IsCoveredOnContract)
                {
                    return ServiceAgreement.ArePartsCovered;
                }

                // if we get here, nothing matched
                return false;
            }
        }

        /// <summary>
        /// Default setting to charge parts.
        /// Returns True if JT_WorkTicketClass.DefaultPartChrgFlag == "Y"
        /// </summary>
        public bool DefaultPartChargeFlag
        {
            get
            {
                return ((_workTicketClass != null) &&
                    (_workTicketClass.DefaultPartChrgFlag != null) &&
                    (_workTicketClass.DefaultPartChrgFlag.Trim().ToUpper() == "Y"));
            }
        }

        /// <summary>
        /// Default value for parts calculating overhead.
        /// Returns True if JT_WorkTicketClass.PartsCalculateOverheadDefault == "Y"
        /// </summary>
        public bool PartsCalculateOverheadDefault
        {
            get
            {
                return ((_workTicketClass != null) &&
                    (_workTicketClass.PartsCalculateOverheadDefault != null) &&
                    (_workTicketClass.PartsCalculateOverheadDefault.Trim().ToUpper() == "Y"));
            }
        }


        public bool HasServiceAgreement
        {
            get { return (_serviceAgreement != null); }
        }

        /// <summary>
        /// Flag to denote if this is preventative maintenance.
        /// Returns True if JT_WorkTicket.DtlPreventitiveMaintenance == "Y"
        /// </summary>
        public bool IsPreventativeMaintenance
        {
            get
            {
                return ((_workTicket.DtlPreventitiveMaintenance != null) &&
                    (_workTicket.DtlPreventitiveMaintenance.Trim().ToUpper() == "Y"));
            }
        }

        /// <summary>
        /// Header template number for this work ticket
        /// </summary>
        public string HeaderTemplateNumber
        {
            get { return _workTicket.HdrTemplateNo; }
        }







        /// <summary>
        /// Contact Code
        /// </summary>
        public string HdrContactCode
        {
            get { return _workTicketStepZero.HdrContactCode; }
        }

        /// <summary>
        /// Repair Item Code
        /// </summary>
        public string DtlRepairItemCode
        {
            get { return _workTicket.DtlRepairItemCode; }
        }

        /// <summary>
        /// Service Contract Code
        /// </summary>
        public string HdrServiceContractCode
        {
            get { return _hdrServiceContractCode; }
        }

        /// <summary>
        /// Status Code
        /// </summary>
        public string StatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// Status Description
        /// </summary>
        public string StatusDescription
        {
            get { return _statusDescription; }
        }

        /// <summary>
        /// Warranty Repair Y/N 
        /// </summary>
        public string DtlWarrantyRepair
        {
            get { return _dtlWarrantyRepair; }
        }

        /// <summary>
        /// Covered on Contract Y/N
        /// </summary>
        public string DtlCoveredOnContract
        {
            get { return _dtlCoveredOnContract; }
        }



        /// <summary>
        /// Problem Code
        /// </summary>
        public string DtlProblemCode
        {
            get
            {
                if (_problemCode == null)
                {
                    return null;
                }

                return _problemCode.ClassificationCode;
            }
        }

        /// <summary>
        /// Problem Code Description
        /// </summary>
        public string DtlProblemCodeDescription
        {
            get
            {
                if (_problemCode == null)
                {
                    return null;
                }

                return _problemCode.Description;
            }
        }

        /// <summary>
        /// Coverage Exception Code
        /// </summary>
        public string DtlCoverageExceptionCode
        {
            get
            {
                if (_coverageExceptionCode == null)
                {
                    return null;
                }

                return _coverageExceptionCode.ClassificationCode;
            }
        }

        /// <summary>
        /// Coverage Exception Code Description
        /// </summary>
        public string DtlaCoverageExceptionCodeDescription
        {
            get
            {
                if (_coverageExceptionCode == null)
                {
                    return null;
                }

                return _coverageExceptionCode.Description;
            }
        }

        /// <summary>
        /// Activity Code
        /// </summary>
        public string ActivityCode
        {
            get { return _activityCode; }
        }



        public string DtlMfgSerialNo
        {
            get
            {
                return _dtlMfgSerialNo;
            }

            set
            {
                _dtlMfgSerialNo = value;
            }
        }



        public App_WorkTicket(JT_WorkTicket workTicket, JT_WorkTicket workTicketStepZero, JT_WorkTicketClass workTicketClass,
            App_RepairItem repairItem, App_ServiceAgreement serviceAgreement, JT_ClassificationCode problemCode,
            JT_ClassificationCode coverageExceptionCode)
        {
            _workTicket = workTicket;
            _workTicketStepZero = workTicketStepZero;
            _workTicketClass = workTicketClass;
            _repairItem = repairItem;
            _serviceAgreement = serviceAgreement;
            _problemCode = problemCode;
            _coverageExceptionCode = coverageExceptionCode;


            // puke... need service agreement (hdr, dtl, pmdtl)
            // puke... need sales order header for address info


            _hdrServiceContractCode = workTicket.HdrServiceContractCode;
            _statusCode = workTicket.StatusCode;
            _statusDescription = "PUKE"; // comes from JT_Status.Description -- need to add
            _dtlWarrantyRepair = workTicket.DtlWarrantyRepair;
            _dtlCoveredOnContract = workTicket.DtlCoveredOnContract;
            _activityCode = workTicket.ActivityCode;
            _dtlMfgSerialNo = workTicket.DtlMfgSerialNo;
        }

        /// <summary>
        /// Formats a standardized work ticket number
        /// </summary>
        /// <param name="salesOrderNumber">Sales Order Number</param>
        /// <param name="workTicketNumber">Work Ticket Number</param>
        /// <param name="workTicketStep">Work Ticket Step</param>
        /// <returns>String representation of the formatted work ticket number.</returns>
        public static string FormatWorkTicketNumber(string salesOrderNumber, string workTicketNumber, string workTicketStep)
        {
            return salesOrderNumber + "-" + workTicketNumber + "-" + workTicketStep;
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
