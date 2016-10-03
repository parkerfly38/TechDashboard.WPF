using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_RepairPart
    {
        private App_WorkTicket _workTicket;
        private int _id;

        private string _parentItemCode;
        private string _partItemCode; 
        private string _partItemCodeDescription; 
        private string _warehouse;  
        private string _problemCode;  
        private double _quantity; 
        private double _unitCost; 
        private double _unitPrice; 
        private string _unitOfMeasure; 
        private string _comment; 
        private bool _isChargeable; 
        private bool _isPrintable; 
        private bool _isPurchased;  
        private bool _isOverhead;

        #region Public Properties

        /// <summary>
        /// Local Database ID
        /// </summary>
        public int ID
        {
            get { return _id; }
        }
        
        /// <summary>
        /// Parent Item Code
        /// </summary>
        public string ParentItemCode
        {
            get { return _parentItemCode; }
        }

        /// <summary>
        /// Part Item Code 
        /// </summary>
        public string PartItemCode
        {
            get { return _partItemCode; }
        }

        /// <summary>
        /// Part Item Description
        /// </summary>
        public string PartItemCodeDescription
        {
            get { return _partItemCodeDescription; }
        }

        /// <summary>
        /// Warehouse 
        /// </summary>
        public string Warehouse
        {
            get { return _warehouse; }
            set { _warehouse = value; }
        }

        /// <summary>
        /// Problem Code 
        /// </summary>
        public string ProblemCode
        {
           get { return _problemCode; }
        }

        /// <summary>
        /// Quantity 
        /// </summary>
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// Unit Cost
        /// </summary>
        public double UnitCost
        {
            get { return _unitCost; }
        }

        /// <summary>
        /// Unit Price
        /// </summary>
        public double UnitPrice
        {
            get { return _unitPrice; }
        }

        /// <summary>
        /// Extended Unit Price
        /// </summary>
        public double ExtdPrice
        {
            get { return Math.Round(_unitPrice * _quantity,2,MidpointRounding.AwayFromZero); }

        }
        /// <summary>
        /// Unit Of Measure 
        /// </summary>
        public string UnitOfMeasure
        {
            get { return _unitOfMeasure; }
            set { _unitOfMeasure = value; }
        }
        
        /// <summary>
        /// Comment 
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        /// Is Chargeable 
        /// </summary>
        public bool IsChargeable
        {
            get { return _isChargeable; }
            set
            {
                _isChargeable = value;

                // if part is chargeable, then MUST print
                if(_isChargeable)
                {
                    _isPrintable = true;
                }
            }
        }

        /// <summary>
        /// Is Printable 
        /// </summary>
        public bool IsPrintable
        {
            get { return _isPrintable; }
            set
            {
                // if part is chargeable, then MUST print
                if (_isChargeable)
                {
                    _isPrintable = true;
                }
                else
                {
                    _isPrintable = value;
                }
            }
        }

        /// <summary>
        /// Is Purchased
        /// </summary>
        public bool IsPurchased
        {
            get { return _isPurchased; }
            set { _isPurchased = value; }
        }

        /// <summary>
        /// Is Chargeable 
        /// </summary>
        public bool IsOverhead
        {
            // puke -- default initially from Work ticket class.  PartsCalculateOverheadDefault
            get { return _isOverhead; }
            set { _isOverhead = value; }
        }

        #endregion

        public App_RepairPart(JT_ServiceEquipmentParts part, App_RepairItem puke)
        {
            // puke...stub to stop errors
        }

        /// <summary>
        /// Contstructor for repair part objects using the transaction import detail records.
        /// </summary>
        /// <param name="importDetail">The JT_TransactionImportDetail record to use.</param>
        /// <param name="workTicket">The App_WorkTicket object associated with this part.</param>
        public App_RepairPart(JT_TransactionImportDetail importDetail, App_WorkTicket workTicket)
        {
            _workTicket = workTicket;

            _id = importDetail.ID;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = importDetail.ItemCode;
            _partItemCodeDescription = importDetail.ItemCodeDesc;
            _warehouse = importDetail.WarehouseCode;
            _problemCode = importDetail.ProblemCode;
            _quantity = importDetail.QuantityUsed;
            _unitCost = importDetail.UnitCost;
            _unitPrice = importDetail.UnitPrice;
            _unitOfMeasure = importDetail.UnitOfMeasure;
            _comment = importDetail.CommentText;
            _isChargeable = (((importDetail.ChargePart != null) && (importDetail.ChargePart.Trim().ToUpper() == "Y")) ? true : false);
            _isPrintable = (((importDetail.PrintPart != null) && (importDetail.PrintPart.Trim().ToUpper() == "Y")) ? true : false);
            _isPurchased = (((importDetail.PurchasePart != null) && (importDetail.PurchasePart.Trim().ToUpper() == "Y")) ? true : false);
            _isOverhead = (((importDetail.Overhead != null) && (importDetail.Overhead.Trim().ToUpper() == "Y")) ? true : false);
        }

        public App_RepairPart(App_Item item, App_WorkTicket workTicket)
        {
            _workTicket = workTicket;

            _id = 0;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = item.ItemCode;
            _partItemCodeDescription = item.ItemCodeDesc;
            _warehouse = item.WarehouseCode;
            _problemCode = _workTicket.DtlProblemCode;
            _quantity = 0.0;
            _unitCost = Convert.ToDouble(item.StandardUnitCost);
            _unitPrice = Convert.ToDouble(item.StandardUnitPrice);
            _unitOfMeasure = item.StandardUnitOfMeasure;
            _comment = string.Empty;
            _isChargeable = SetIsChargeable();
            _isPrintable = (_isChargeable ? true : false);
            _isPurchased = false;
            _isOverhead = _workTicket.PartsCalculateOverheadDefault;
        }

        public App_RepairPart(CI_Item item, App_WorkTicket workTicket)
        {
            _workTicket = workTicket;

            _id = 0;            
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = item.ItemCode;
            _partItemCodeDescription = item.ItemCodeDesc;
            _warehouse = item.DefaultWarehouseCode;
            _problemCode = _workTicket.DtlProblemCode;
            _quantity = 0.0;
            _unitCost = Convert.ToDouble(item.StandardUnitCost);
            _unitPrice = Convert.ToDouble(item.StandardUnitPrice);
            _unitOfMeasure = item.StandardUnitOfMeasure;
            _comment = string.Empty;
            _isChargeable = SetIsChargeable();
            _isPrintable = (_isChargeable ? true : false);
            _isPurchased = false;
            _isOverhead = _workTicket.PartsCalculateOverheadDefault; 
        }
       
        /// <summary>
        /// Determines the initial/default value of the Chargeable flag
        /// </summary>
        /// <returns>True if logic determines part is chargeable; False otherwise.</returns>
        private bool SetIsChargeable()
        {
            if (!_workTicket.ServiceAgreement.ArePartsCovered)
            {
                return true;
            }
            else if ((_workTicket.ServiceAgreement.ArePartsCovered) &&
                     (_workTicket.DefaultPartChargeFlag))
            {
                return true;
            }
            else if ((_workTicket.ArePartsCoveredOnWarranty) &&
                     (!_workTicket.DefaultPartChargeFlag))
            {
                return false;
            }
            else if ((!_workTicket.ArePartsCoveredOnWarranty) &&
                     (!_workTicket.ArePartsCoveredOnSerivceAgreement) &&
                     (!_workTicket.DefaultPartChargeFlag))
            {
                return false;
            }
            else if ((!_workTicket.ArePartsCoveredOnWarranty) &&
                     (_workTicket.ArePartsCoveredOnSerivceAgreement) &&
                     (_workTicket.DefaultPartChargeFlag))
            {
                return true;
            }
            else if (_workTicket.ServiceAgreement.BillingType == "T")
            {
                return true;
            }
            else if (((_workTicket.ServiceAgreement.BillingType == "P") || 
                      (_workTicket.ServiceAgreement.BillingType == "F")) &&
                     (!_workTicket.ArePartsCoveredOnSerivceAgreement) &&
                     (!_workTicket.DefaultPartChargeFlag))
            {
                return true;

            }
            else
            {
                return _workTicket.DefaultPartChargeFlag;
            }
        }
    }
}
