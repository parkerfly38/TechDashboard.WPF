using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDashboard.Models
{
    public class SO_Options
    {
        /// <summary>
        /// Module Code - varchar(3)
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// New System - char(1) - Y=Yes, N=No, S-Setup in Progress
        /// </summary>
        public string NewSystem { get; set; }

        /// <summary>
        /// Default Warehouse Code - varchar(3)
        /// </summary>
        public string DefaultWarehouseCode { get; set; }

        /// <summary>
        /// Use Alternate Whse For Out-Of - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseAlternateWhse { get; set; }

        /// <summary>
        /// Use Default Item Warehouse - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseDefaultItemWhse { get; set; }

        /// <summary>
        /// Split Commissions Between Salespersons - char(1) - Y=Yes, N=No
        /// </summary>
        public string SplitCommissions { get; set; }

        /// <summary>
        /// Customer Split Commission to Override Ship To - char(1) - Y=Yes, N=No
        /// </summary>
        public string CustomerSplitComOverrideShipTo { get; set; }

        /// <summary>
        /// Use Shipping To Calculate Freight - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseShippingCode { get; set; }

        /// <summary>
        /// Use Default Order Date When Promoting Quote - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseDfltOrderDatePromotingQuote { get; set; }

        /// <summary>
        /// Default Days Until Quote Expires = int
        /// </summary>
        public int DefaultDaysUntilQuoteExpires { get; set; }

        /// <summary>
        /// Next Sales order Number - varchar(7)
        /// </summary>
        public string NextSalesOrderNo { get; set; }

        /// <summary>
        /// Next Invoice Number - varchar(7)
        /// </summary>
        public string NextInvoiceNo { get; set; }

        /// <summary>
        /// Current SO Calendar Year - varchar(4)
        /// </summary>
        public string CurrentCalendarYr { get; set; }

        /// <summary>
        /// Current SO Fiscal Year - varchar(4)
        /// </summary>
        public string CurrentFiscalYr { get; set; }

        /// <summary>
        /// Current SO Period - varchar(2)
        /// </summary>
        public string CurrentPeriod { get; set; }

        /// <summary>
        /// Allow Discount Rate by Detail Line - char(1) - Y=Yes, N=No
        /// </summary>
        public string AllowDiscountRate { get; set; }

        /// <summary>
        /// Check For Quantity On Hand - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckQtyOnHand { get; set; }

        /// <summary>
        /// Default Special Items to Drop Ship - char(1) - Y=Yes, N=No
        /// </summary>
        public string DefaultSpecialItemsToDS { get; set; }

        /// <summary>
        /// Include Backordered Lines - char(1) - Y=Yes, N=No
        /// </summary>
        public string IncludeBackorderedLines { get; set; }

        /// <summary>
        /// Require Job Number- char(1) - Y=Yes, N=No
        /// </summary>
        public string RequireJobNo { get; set; }

        /// <summary>
        /// Allow Jobs To Be Created - char(1) - Y=Yes, N=No
        /// </summary>
        public string AllowJobsToBeCreated { get; set; }

        /// <summary>
        /// Require Cost Code - char(1) - Y=Yes, N=No
        /// </summary>
        public string RequireCostCode { get; set; }

        /// <summary>
        /// Post Invoice Costs to Job Cost - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostInvoiceCostsToJC { get; set; }

        /// <summary>
        /// Post Sales To G/L By Division - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostSalesByDivision { get; set; }

        /// <summary>
        /// Post Customer Deposits by Division - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostDepositsByDiv { get; set; }

        /// <summary>
        /// Post Deposits in Detail - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostDepositsInDetail { get; set; }

        /// <summary>
        /// Print Sales Orders - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintSalesOrders { get; set; }

        /// <summary>
        /// Print Picking Sheets Sort Order - char(1) - B=Bin location, I=Item number, L=Line number, N=None
        /// </summary>
        public string PrintPickingSheets { get; set; }

        /// <summary>
        /// Include Kit Items on Picking Sheet - char(1) - Y=Yes, N=No
        /// </summary>
        public string IncludeKitItems { get; set; }

        /// <summary>
        /// Print Shipping Labels - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintShippingLabels { get; set; }

        /// <summary>
        /// Print Daily Drop Ship Report - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintDailyDropShipRpt { get; set; }

        /// <summary>
        /// Print C.O.D. Labels - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintCODLabels { get; set; }

        /// <summary>
        /// Display Message - char(1) - Y=Yes, N=No
        /// </summary>
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Customer Deposits Account Key - char(9)
        /// </summary>
        public string CustomerDepAcctKey { get; set; }

        /// <summary>
        /// Special Item Sales Account Key - char(9)
        /// </summary>
        public string SpecialItemSalesAcctKey { get; set; }

        /// <summary>
        /// Special Item COGS Account Key - char(9)
        /// </summary>
        public string SpecialItemCOGSAcctKey { get; set; }

        /// <summary>
        /// Special Item Purchases Account Key - char(9)
        /// </summary>
        public string SpecialItemPurchasesAcctKey { get; set; }

        /// <summary>
        /// Print Sales Journal By Division - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintSalesJournalByDiv { get; set; }

        /// <summary>
        /// Print Gross Profit Journal - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintGrossProfitJournal { get; set; }

        /// <summary>
        /// Print Gross Profit Journal By Salesperson - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintGrossPrftJrnlBySlsprson { get; set; }

        /// <summary>
        /// Print Daily Backorder Report - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintDailyBackorder { get; set; }

        /// <summary>
        /// Print Daily Deposit Recap Report - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintDailyDepositRecap { get; set; }

        /// <summary>
        /// Print Tax Detail On Sales Journal - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintTaxDetailOnSalesJrnl { get; set; }

        /// <summary>
        /// Print Tax Journal In Detail - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintTaxJournalInDetail { get; set; }

        /// <summary>
        /// Print Daily Sales Recap Reports - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintDailySalesRecap { get; set; }

        /// <summary>
        /// Print Recap By Item - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByItem { get; set; }

        /// <summary>
        /// Print Whse Detail For Recap - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintWhseDetailForRecap { get; set; }

        /// <summary>
        /// Print Recap By Whse By Item - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByWhseByItem { get; set; }

        /// <summary>
        /// Print Recap By Prod Line - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByProductLine { get; set; }

        /// <summary>
        /// Print Recap By Whse By Product Line - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByWhseByProdLine { get; set; }

        /// <summary>
        /// Print Recap By Whse - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByWhse { get; set; }

        /// <summary>
        /// Print Recap By Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByCustomer { get; set; }

        /// <summary>
        /// Print Recap By Division - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintRecapByDiv { get; set; }

        /// <summary>
        /// Print Bar Code Sales Orders - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintBarCodeSalesOrders { get; set; }

        /// <summary>
        /// Print Bar Code Picking Sheets - char(1) - Y=Yes, N=No
        /// </summary>
        public string PrintBarCodePickSheets { get; set; }

        /// <summary>
        /// Redisplay All Source Documents - char(1) - Y=Yes, N=No
        /// </summary>
        public string RedisplayAllDocuments { get; set; }

        /// <summary>
        /// Redisplay Sales Orders - char(1) - Y=Yes, N=No
        /// </summary>
        public string RedisplaySalesOrders { get; set; }

        /// <summary>
        /// Redisplay Sales Orders After Picking - char(1) - Y=Yes, N=No
        /// </summary>
        public string RedisplayPickingSheets { get; set; }

        /// <summary>
        /// Redisplay Invoices - char(1) - Y=Yes, N=No
        /// </summary>
        public string RedisplayInvoices { get; set; }

        /// <summary>
        /// Picking Sheet After Quick Print - char(1) - Y=Yes, N=No
        /// </summary>
        public string PickingSheetAfterOrder { get; set; }

        /// <summary>
        /// Picking Sheet Only For Default Whse - char(1) - Y=Yes, N=No
        /// </summary>
        public string OnlyForDefaultWhse { get; set; }

        /// <summary>
        /// Shipping Labels After Pick Sheet - char(1) - Y=Yes, N=No
        /// </summary>
        public string ShippingLabelsAfterPickSheet { get; set; }

        /// <summary>
        /// COD Labels After Quick Print - char(1) - Y=Yes, N=No
        /// </summary>
        public string CODLabelsAfterInvoice { get; set; }

        /// <summary>
        /// Quick Print Without Display Print Window - char(1) - Y=Yes, N=No
        /// </summary>
        public string QuickPrintWithoutPrintWindow { get; set; }

        /// <summary>
        /// Purge S/O Recap at Period End - char(1) - Y=Yes, N=No
        /// </summary>
        public string PurgeSORecap { get; set; }

        /// <summary>
        /// Display Unit Cost During Line Entry - char(1) - Y=Yes, N=No
        /// </summary>
        public string DisplayUnitCost { get; set; }

        /// <summary>
        /// Years to Retain Sales History
        /// </summary>
        public int YearsToRetainSalesHist { get; set; }

        /// <summary>
        /// Retain Backordered Lines in Inv - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainBackorderedLines { get; set; }

        /// <summary>
        /// Retain Ship-To Detail - - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainShipToDetail { get; set; }

        /// <summary>
        /// Retain Lot/Serial Sales History - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainLotSerialHistory { get; set; }

        /// <summary>
        /// Retain Customer Last Purchase Hhistory - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainCustLastPurchHistory { get; set; }

        /// <summary>
        /// Retain Sales Order/Quote History - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainOrderQuoteHistory { get; set; }

        /// <summary>
        /// Retain Deleted Sales Order/Quote - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainDeletedSalesOrders { get; set; }

        /// <summary>
        /// Retain Deleted Lines in History - char(1) - Y=Yes, N=No
        /// </summary>
        public string RetainDeletedLines { get; set; }

        /// <summary>
        /// Display Profit Margin Percent - char(1) - Y=Yes, N=No
        /// </summary>
        public string DisplayProfitMargin { get; set; }

        /// <summary>
        /// Enable Purchase Control - char(1) - Y=Yes, N=No
        /// </summary>
        public string EnablePurchaseControl { get; set; }

        /// <summary>
        /// Select Items Based on - char(1) - C=Customer Number, S=Ship To
        /// </summary>
        public string SelectItemsBasedOn { get; set; }

        /// <summary>
        /// Control Purchases Based On - char(1) - A=Items Allowed, N=Items Not Allowed
        /// </summary>
        public string ControlPurchasesBasedOn { get; set; }

        /// <summary>
        /// Enable Price by Total Quantity - char(1) - A=All Lines, N=None, P-Product Line, 1-Item Category 1, 2=Item Category 2, 3=Item Category 3, 4=Item Category 4
        /// </summary>
        public string EnablePriceByTotalQtyBasedOn { get; set; }


        /// <summary>
        /// Unit of Measure For Total Quantity - char(1) - L=Sales (From Lines), S=Standard, N=None
        /// </summary>
        public string UnitOfMeasureForTotalQty { get; set; }


        /// <summary>
        /// Apply Pricing by Total Quantity - char(1) - E=New Entries Only, N=No, P=Prompt, Y=Yes
        /// </summary>
        public string ApplyPricingByTotalQtyAuto { get; set; }


        /// <summary>
        /// Enable Shipping - char(1) - Y=Yes, N=No
        /// </summary>
        public string EnableShipping { get; set; }


        /// <summary>
        /// Warranty Calculation Based On - char(1) - S=Ship date, I=Invoice date
        /// </summary>
        public string WarrantyCalcBasedOn { get; set; }


        /// <summary>
        /// Recalc Expiration if Ship/Invoice - char(1) - Y=Yes, N=No
        /// </summary>
        public string RecalcExpiration { get; set; }


        /// <summary>
        /// Enable Default Price Level - char(1) - Y=Yes, N=No
        /// </summary>
        public string EnableDefaultPriceLevelByCust { get; set; }


        /// <summary>
        /// Base New Price Level Records On - char(1) - P=Price Code, S=Ship To Code
        /// </summary>
        public string BaseNewPriceLevelRecordsOn { get; set; }


        /// <summary>
        /// Enable Lot/Serial Distribution - char(1) - Y=Yes, N=No
        /// </summary>
        public string EnableLotSerialDist { get; set; }


        /// <summary>
        /// Require Lines to be Fully Distributed - char(1) - Y=Yes, N=No
        /// </summary>
        public string RequireFullyDistributedLines { get; set; }

        /// <summary>
        /// Integrate to General Ledger - char(1) - Y=Yes, N=No
        /// </summary>
        public string IntegrateGeneralLedger { get; set; }

        /// <summary>
        /// Integrate to Inventory - char(1) - Y=Yes, N=No
        /// </summary>
        public string IntegrateInventory { get; set; }

        /// <summary>
        /// Integrate to Job Cost - char(1) - Y=Yes, N=No
        /// </summary>
        public string IntegrateJobCost { get; set; }

        /// <summary>
        /// Period End Option Selection - char(1)
        /// </summary>
        public string PeriodEndOptSelection { get; set; }

        /// <summary>
        /// Current Invoice Update - char(1) - B, Y,N
        /// </summary>
        public string CurrentInvoiceUpdate { get; set; }

        /// <summary>
        /// Check for duplicate Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckARInvoiceDataEntry { get; set; }

        /// <summary>
        /// Check for duplicate Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckARInvoiceHistory { get; set; }

        /// <summary>
        /// Check for duplicate Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckSalesOrderEntry { get; set; }

        /// <summary>
        /// Check for duplicate Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckSalesOrderHistory { get; set; }

        /// <summary>
        /// Check for duplicate Customer - char(1) - Y=Yes, N=No
        /// </summary>
        public string CheckSOInvoiceDataEntry { get; set; }

        /// <summary>
        /// Company Code - char(3)
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// Default Number of Shipping Labels - int
        /// </summary>
        public int DefaultNoOfLabels { get; set; }

        /// <summary>
        /// Profit Margin Percent to Display - int
        /// </summary>
        public int ProfitMarginPercent { get; set; }

        /// <summary>
        /// Post Invoice Costs to Job Estimates - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostInvoiceCostsToJobEstimates { get; set; }

        /// <summary>
        /// Post Drop Ship Costs to Job Cost - char(1) - Y=Yes, N=No
        /// </summary>
        public string PostDropShipCostsToJC { get; set; }

        /// <summary>
        /// Relieve Work In Proces - char(1) - Y=Yes, N=No
        /// </summary>
        public string RelieveWorkInProcess { get; set; }

        /// <summary>
        /// Set Job Status to Complete When Inv - char(1) - Y=Yes, N=No
        /// </summary>
        public string SetJobStatusToCompleteWhenInv { get; set; }

        /// <summary>
        /// Include Job Number and Description in GL Comment - char(1) - Y=Yes, N=No
        /// </summary>
        public string IncludeJobNoAndDescInGLComment { get; set; }

        /// <summary>
        /// Relieve Inventory - char(1) - Y=Yes, N=No
        /// </summary>
        public string RelieveInventory { get; set; }

        /// <summary>
        /// Sales Kit Update Method - char(1) - S=Standard Method, K=Kit Item w/Cost + Component zero, C=Kit Item w/out Cost + Component costs, O=Only Component Costs
        /// </summary>
        public string SalesKitUpdateMethod { get; set; }

        /// <summary>
        /// Validate Customer - char(1) - N=No validatieon, W=Write Back to Job, Y=Yes
        /// </summary>
        public string ValidateCustomer { get; set; }

        /// <summary>
        /// Validate SO Cost Types - char(12)
        /// </summary>
        public string ValidSOCostTypes { get; set; }

        /// <summary>
        /// Use SO Accts For COGS - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseSOAcctsForCOGS { get; set; }

        /// <summary>
        /// Use SO Accts for Sales - char(1) - Y=Yes, N=No
        /// </summary>
        public string UseSOAcctsForSales { get; set; }

        /// <summary>
        /// Data Version - num(5,2)
        /// </summary>
        public decimal DataVersion { get; set; }

        /// <summary>
        /// Data Sub-Level - int
        /// </summary>
        public int DataSublevel { get; set; }

        /// <summary>
        /// Date Updated - date
        /// </summary>
        public string DateUpdated { get; set; }

        /// <summary>
        /// Time Updated - time
        /// </summary>
        public string TimeUpdated { get; set; }

        /// <summary>
        /// User Updated Key - char(10)
        /// </summary>
        public string UserUpdatedKey { get; set; }
    }
}
