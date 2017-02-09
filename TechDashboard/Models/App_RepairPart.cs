using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_RepairPart.cs
     * 11/22/2016 DCH Handle null values
     * 12/02/2016 DCH Add ItemType, QuantityReqd, Valuation, QuantityShipped
     * 12/02/2016 DCH Get Item Unit Cost based on JT_Options.DefaultPartsCost
     * 01/13/2017 DCH Add Extended Description
     * 01/13/2017 DCH Part Description should come from sales order
     * 01/23/2017 DCH Capture SO Line Key
     * 02/03/2017 DCH Use the sales unit of measure instead of the standard unit of measure
     *********************************************************************************************************/
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
        private string _itemType;      // dch rkl 11/23/2016 add item type
        private decimal _quantityReqd;       // dch rkl 11/30/2016 add quantity required
        private string _valuation;         // dch rkl 12/05/2016 Add Item Valuation
        private double _quantityShipped;        // dch rkl 12/05/2016 add qty shipped
        private string _itemCodeDesc;       // dch rkl 01/13/2017 Add this for Extended Description
        private string _soLineKey;             // dch rkl 01/23/2017 Save SOLineKey

        CI_Options _ciOptions;

        string quantityFormatString;
        string umFormatString;
        string costFormatString;
        string priceFormatString;

        #region Public Properties

        /// <summary>
        /// Local Database ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
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
            set { _partItemCode = value; }
        }

        /// <summary>
        /// Part Item Description
        /// </summary>
        public string PartItemCodeDescription
        {
            get { return _partItemCodeDescription; }
            set { _partItemCodeDescription = value; }       // dch rkl 01/13/2017 allow set
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
            set { _problemCode = value; }
        }

        /// <summary>
        /// Quantity 
        /// </summary>
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string QuantityFormatted
        {
            get { return string.Format(quantityFormatString, _quantity); }
        }

        /// <summary>
        /// Unit Cost
        /// </summary>
        public double UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }  // dch rkl 11/21/2016 this is editable
        }

        public string UnitCostFormatted
        {
            get { return string.Format(costFormatString, _unitCost); }
        }

        /// <summary>
        /// Unit Price
        /// </summary>
        public double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public string UnitPriceFormatted
        {
            get { return string.Format(priceFormatString, _unitPrice); }
        }

        /// <summary>
        /// Extended Unit Price
        /// </summary>
        public double ExtdPrice
        {
            get { return Math.Round(_unitPrice * _quantity, 2, MidpointRounding.AwayFromZero); }

        }

        public string ExtdPriceFormatted
        {
            get { return string.Format(priceFormatString, ExtdPrice); }
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
        /// dch rkl 11/23/2016 Add Item Type
        /// </summary>
        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        /// <summary>
        /// dch rkl 12/05/2016 Add Item Valuation
        /// </summary>
        public string Valuation
        {
            get { return _valuation; }
            set { _valuation = value; }
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
                if (_isChargeable)
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

        protected string _lotSerialNo;
        public string LotSerialNo
        {
            get { return _lotSerialNo; }
            set { _lotSerialNo = value; }
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
            // TODO -- default initially from Work ticket class.  PartsCalculateOverheadDefault
            get { return _isOverhead; }
            set { _isOverhead = value; }
        }

        // dch rkl 11/30/2016 add quantity required
        public decimal QuantityReqd
        {
            get { return _quantityReqd; }
            set { _quantityReqd = value; }
        }

        public string QuantityReqdFormatted
        {
            get { return string.Format(quantityFormatString, _quantityReqd); }
        }

        // dch rkl 12/05/2016 add qty shipped
        public double QuantityShipped
        {
            get { return _quantityShipped; }
            set { _quantityShipped = value; }
        }

        public string QuantityShippedFormatted
        {
            get { return string.Format(quantityFormatString, _quantityShipped); }
        }

        /// <summary>
        /// dch rkl 01/13/2017 Add Extended Description 
        /// </summary>
        public string ItemCodeDesc
        {
            get { return _itemCodeDesc; }
            set { _itemCodeDesc = value; }
        }

        // dch rkl 01/23/2017 Save SOLineKey
        public string SoLineKey
        {
            get { return _soLineKey; }
            set { _soLineKey = value; }
        }

        #endregion

        public App_RepairPart(JT_ServiceEquipmentParts part, App_RepairItem TODO)
        {
            // stub to stop errors

            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");
        }

        public App_RepairPart(JT_ServiceEquipmentParts part, App_WorkTicket workTicket, CI_Item item)
        {
            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            _workTicket = workTicket;
            _id = 0;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = part.ItemCode;
            _partItemCodeDescription = item.ItemCodeDesc;
            _warehouse = item.DefaultWarehouseCode;
            _problemCode = part.ProblemCode;
            _quantity = part.Quantity;

            // dch rkl 12/02/2016 Get Item Unit Cost based on JT_Options.DefaultPartsCost
            _unitCost = GetItemUnitCost(item.LastTotalUnitCost, item.AverageUnitCost, item.StandardUnitCost);
            //_unitCost = (double)item.StandardUnitCost;

            _unitPrice = (double)item.StandardUnitPrice;

            // dch rkl 02/03/2017 Use the sales unit of measure instead of the standard unit of measure
            //_unitOfMeasure = item.StandardUnitOfMeasure;
            _unitOfMeasure = item.SalesUnitOfMeasure;

            _comment = "";
            _isChargeable = part.IsChargeable;
            _isPrintable = part.IsPrintable;
            _isPurchased = part.IsPurchased;
            _isOverhead = part.IsOverhead;

            // dch rkl 11/23/2016 Add Item Type
            _itemType = item.ItemType;

            // dch rkl 12/05/2016 Add Item Valuation
            _valuation = item.Valuation;

            // dch rkl 11/30/2016 add quantity required
            _quantityReqd = 0;

            // dch rkl 12/05/2016 add qty shipped
            _quantityShipped = 0;

            // dch rkl 01/13/2017 Add Extended Description 
            _itemCodeDesc = "";

            // dch rkl 01/23/2017 Sales Order Line Key
            _soLineKey = "";
    }


    public App_RepairPart(JT_EquipmentAsset part, App_WorkTicket workTicket, CI_Item item)
        {
            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            _workTicket = workTicket;
            _id = 0;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = part.ItemCode;
            _partItemCodeDescription = item.ItemCodeDesc;
            _warehouse = item.DefaultWarehouseCode;
            _problemCode = workTicket.DtlProblemCode;
            _quantity = 0.0;

            // dch rkl 12/02/2016 Get Item Unit Cost based on JT_Options.DefaultPartsCost
            _unitCost = GetItemUnitCost(item.LastTotalUnitCost, item.AverageUnitCost, item.StandardUnitCost);
            //_unitCost = (double)item.StandardUnitCost;

            _unitPrice = Convert.ToDouble(item.StandardUnitPrice);

            // dch rkl 02/03/2017 Use the sales unit of measure instead of the standard unit of measure
            //_unitOfMeasure = item.StandardUnitOfMeasure;
            _unitOfMeasure = item.SalesUnitOfMeasure;

            _comment = string.Empty;
            _isChargeable = SetIsChargeable();
            _isPrintable = (_isChargeable ? true : false);
            _isPurchased = false;
            _isOverhead = _workTicket.PartsCalculateOverheadDefault;

            // dch rkl 11/23/2016 Add Item Type
            _itemType = item.ItemType;

            // dch rkl 12/05/2016 Add Item Valuation
            _valuation = item.Valuation;

            // dch rkl 11/30/2016 add quantity required
            _quantityReqd = 0;

            // dch rkl 12/05/2016 add qty shipped
            _quantityShipped = 0;

            // dch rkl 01/13/2017 Add Extended Description 
            _itemCodeDesc = "";

            // dch rkl 01/23/2017 Sales Order Line Key
            _soLineKey = "";
        }

        /// <summary>
        /// Contstructor for repair part objects using the transaction import detail records.
        /// </summary>
        /// <param name="importDetail">The JT_TransactionImportDetail record to use.</param>
        /// <param name="workTicket">The App_WorkTicket object associated with this part.</param>
        public App_RepairPart(JT_TransactionImportDetail importDetail, App_WorkTicket workTicket)
        {

            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            _workTicket = workTicket;

            _id = importDetail.ID;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = importDetail.ItemCode;
            _partItemCodeDescription = importDetail.ItemCodeDesc;

            // dch rkl 11/22/2016
            CI_Item item = App.Database.GetItemFromDB(_partItemCode);
            if (_partItemCodeDescription == null)
            {
                if (item != null && item.ItemCodeDesc != null) { _partItemCodeDescription = item.ItemCodeDesc; }
                else { _partItemCodeDescription = ""; }
            }

            // dch rkl 12/05/2016 Make sure part has a valuation value

            _warehouse = importDetail.WarehouseCode;
            _problemCode = importDetail.ProblemCode;
            _quantity = importDetail.QuantityUsed;
            _unitCost = importDetail.UnitCost;
            _unitPrice = importDetail.UnitPrice;
            _unitOfMeasure = importDetail.UnitOfMeasure;
            _comment = importDetail.CommentText;
            _lotSerialNo = importDetail.LotSerialNo;
            _isChargeable = (((importDetail.ChargePart != null) && (importDetail.ChargePart.Trim().ToUpper() == "Y")) ? true : false);
            _isPrintable = (((importDetail.PrintPart != null) && (importDetail.PrintPart.Trim().ToUpper() == "Y")) ? true : false);
            _isPurchased = (((importDetail.PurchasePart != null) && (importDetail.PurchasePart.Trim().ToUpper() == "Y")) ? true : false);
            _isOverhead = (((importDetail.Overhead != null) && (importDetail.Overhead.Trim().ToUpper() == "Y")) ? true : false);

            // dch rkl 11/23/2016 Add Item Type
            if (item != null && item.ItemType != null) { _itemType = item.ItemType; }

            // dch rkl 12/05/2016 Add Item Valuation
            if (item != null && item.Valuation != null) { _valuation = item.Valuation; }

            // dch rkl 11/30/2016 add quantity required
            _quantityReqd = (decimal)importDetail.QuantityRequired;

            // dch rkl 12/05/2016 add qty shipped
            _quantityShipped = importDetail.QuantityCompleted;

            // dch rkl 01/13/2017 Add Extended Description 
            if (importDetail.ItemCodeDesc != null) { _itemCodeDesc = importDetail.ItemCodeDesc; }
            else { _itemCodeDesc = ""; }

            // dch rkl 01/23/2017 Sales Order Line Key
            _soLineKey = importDetail.SOLineKey;
        }

        public App_RepairPart(App_Item item, App_WorkTicket workTicket)
        {
            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            _workTicket = workTicket;

            _id = 0;
            _parentItemCode = workTicket.DtlRepairItemCode;
            _partItemCode = item.ItemCode;
            _partItemCodeDescription = item.ItemCodeDesc;
            _warehouse = item.WarehouseCode;
            _problemCode = _workTicket.DtlProblemCode;
            _quantity = 0.0;

            // dch rkl 12/02/2016 Get Item Unit Cost based on JT_Options.DefaultPartsCost
            _unitCost = GetItemUnitCost(item.LastTotalUnitCost, item.AverageUnitCost, item.StandardUnitCost);
            //_unitCost = (double)item.StandardUnitCost;

            _unitPrice = Convert.ToDouble(item.StandardUnitPrice);

            // dch rkl 02/03/2017 Per Jeanne, Use the Sales Unit of Measure, not the Standard Unit of Measure
            //_unitOfMeasure = item.StandardUnitOfMeasure;
            _unitOfMeasure = item.SalesUnitOfMeasure;

            _comment = string.Empty;
            _isChargeable = SetIsChargeable();
            _isPrintable = (_isChargeable ? true : false);
            _isPurchased = false;
            _isOverhead = _workTicket.PartsCalculateOverheadDefault;
            _itemType = item.ItemType;          // dch rkl 11/23/2016 add item type

            // dch rkl 11/30/2016 add quantity required
            _quantityReqd = 0;

            // dch rkl 11/23/2016 Add Item Type
            _itemType = item.ItemType;

            // dch rkl 12/05/2016 Add Item Valuation
            _valuation = item.Valuation;

            // dch rkl 12/05/2016 add qty shipped
            _quantityShipped = 0;

            // dch rkl 01/13/2017 Add Extended Description 
            _itemCodeDesc = "";

            // dch rkl 01/23/2017 Sales Order Line Key
            _soLineKey = "";
        }

        public App_RepairPart(CI_Item item, App_WorkTicket workTicket)
        {
            _ciOptions = App.Database.GetCIOptions();
            quantityFormatString = String.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInQty, "}");
            umFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInUM, "}");
            costFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInCost, "}");
            priceFormatString = string.Concat("{0:F", _ciOptions.NumberOfDecimalPlacesInPrice, "}");

            _workTicket = workTicket;

            _id = 0;
            _parentItemCode = workTicket.DtlRepairItemCode;

            // dch rkl 10/31/2016 handle nulls
            if (item != null)
            {
                _partItemCode = item.ItemCode;
                _partItemCodeDescription = item.ItemCodeDesc;
                _warehouse = item.DefaultWarehouseCode;

                // dch rkl 12/02/2016 Get Item Unit Cost based on JT_Options.DefaultPartsCost
                _unitCost = GetItemUnitCost(item.LastTotalUnitCost, item.AverageUnitCost, item.StandardUnitCost);
                //_unitCost = (double)item.StandardUnitCost;

                _unitPrice = Convert.ToDouble(item.StandardUnitPrice);

                // dch rkl 02/03/2017 Per Jeanne, Use the Sales Unit of Measure, not the Standard Unit of Measure
                //_unitOfMeasure = item.StandardUnitOfMeasure;
                _unitOfMeasure = item.SalesUnitOfMeasure;

                // dch rkl 11/23/2016 Add Item Type
                _itemType = item.ItemType;

                // dch rkl 12/05/2016 Add Item Valuation
                _valuation = item.Valuation;
            }
            else
            {
                _partItemCode = "";
                _partItemCodeDescription = "";
                _warehouse = "";
                _unitCost = 0;
                _unitPrice = 0;
                _unitOfMeasure = "";
                _itemType = "";
                _valuation = "";
            }

            _problemCode = _workTicket.DtlProblemCode;
            _quantity = 0.0;
            _comment = string.Empty;
            _isChargeable = SetIsChargeable();
            _isPrintable = (_isChargeable ? true : false);
            _isPurchased = false;
            _isOverhead = _workTicket.PartsCalculateOverheadDefault;

            // dch rkl 11/30/2016 add quantity required
            _quantityReqd = 0;

            // dch rkl 12/05/2016 add qty shipped
            _quantityShipped = 0;

            // dch rkl 01/13/2017 Add Extended Description 
            _itemCodeDesc = "";

            // dch rkl 01/23/2017 Sales Order Line Key
            _soLineKey = "";
        }

        // dch rkl 12/02/2016 Get Item Unit Cost based on JT_Options.DefaultPartsCost
        private double GetItemUnitCost(decimal dLastTotalUnitCost, decimal dAverageUnitCost, decimal dStandardUnitCost)
        {
            double itemUnitCost = 0;

            List<JT_Options> lsOptions = App.Database.GetJTOptionsFromDB();
            if (lsOptions.Count > 0)
            {
                JT_Options options = lsOptions[0];
                if (options.DefaultPartsCost == "L") { itemUnitCost = (double)dLastTotalUnitCost; }
                else if (options.DefaultPartsCost == "A") { itemUnitCost = (double)dAverageUnitCost; }
                else { itemUnitCost = (double)dStandardUnitCost; }
            }

            return itemUnitCost;
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
