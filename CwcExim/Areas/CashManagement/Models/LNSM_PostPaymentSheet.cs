using CwcExim.Areas.CashManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{ 
    public class LNSM_PostPaymentSheet
    {
        private decimal _CWCTotal = 0M;
        private decimal _HTTotal = 0M;
        private decimal _CWCTDS = 0M;
        private decimal _HTTDS = 0M;
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string ApproveOn { get; set; } = string.Empty;
        public string ROAddress { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public int RequestId { get; set; } = 0;
        public string RequestNo { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAddress { get; set; } = string.Empty;
        public string PartyState { get; set; } = string.Empty;
        public string PartyStateCode { get; set; } = string.Empty;
        public string PartyGST { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public int PartyIdSec { get; set; } = 0;
        public string PartyNameSec { get; set; } = string.Empty;
        public string PartyAddressSec { get; set; } = string.Empty;
        public string PartyStateSec { get; set; } = string.Empty;
        public string PartyStateCodeSec { get; set; } = string.Empty;
        public string PartyGSTSec { get; set; } = string.Empty;
        public int PayeeIdSec { get; set; } = 0;
        public string PayeeNameSec { get; set; } = string.Empty;
        public int SecondInv { get; set; }
        public decimal TotalAmt { get; set; } = 0M;
        public decimal TotalDiscount { get; set; } = 0M;
        public decimal TotalTaxable { get; set; } = 0M;
        public decimal TotalCGST { get; set; } = 0M;
        public decimal TotalSGST { get; set; } = 0M;
        public decimal TotalIGST { get; set; } = 0M;
        public decimal CWCAmtTotal { get; set; } = 10M;
        public decimal CWCTotal
        {
            get { return _CWCTotal; }
            set { _CWCTotal = value; }
        }
        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal
        {
            get { return _HTTotal; }
            set { _HTTotal = value; }
        }
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS
        {
            get { return _CWCTDS; }
            set { _CWCTDS = value; }
        }
        public decimal HTTDS
        {
            get { return _HTTDS; }
            set { _HTTDS = value; }
        }
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal TCS { get; set; } = 0M;
        public decimal InvoiceAmt { get; set; } = 0M;
        public string ShippingLineName { get; set; } = string.Empty;
        public string CHAName { get; set; } = string.Empty;
        public string ImporterExporter { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public string BOEDate { get; set; } = string.Empty;
        public string CFSCode { get; set; } = string.Empty;
        public string DestuffingDate { get; set; } = string.Empty;
        public string StuffingDate { get; set; } = string.Empty;
        public string CartingDate { get; set; } = string.Empty;
        public string ArrivalDate { get; set; } = string.Empty;
        public int TotalNoOfPackages { get; set; } = 0;
        public decimal TotalGrossWt { get; set; } = 0M;
        public decimal TotalWtPerUnit { get; set; } = 0M;
        public decimal TotalSpaceOccupied { get; set; } = 0M;
        public string TotalSpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal TotalValueOfCargo { get; set; } = 0M;
        public decimal PdaAdjustedAmount { get; set; } = 0M;
        public string CompGST { get; set; } = string.Empty;
        public string CompPAN { get; set; } = string.Empty;
        public string CompStateCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;        
        public string CashierRemarks { get; set; } = string.Empty;
        public string PDAadjustedCashReceiptEdit { get; set; } = string.Empty;      
        public int DeliveryType { get; set; } = 1;
        public string BillType { get; set; } = string.Empty;
        public string StuffingDestuffDateType { get; set; } = string.Empty;
        public string StuffingDestuffingDate { get; set; } = string.Empty;
        public string ImporterExporterType { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public string InvoiceHtmlSec { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string UptoDate { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0M;
        public int OperationType { get; set; }
        public IList<LNSM_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<LNSM_PostPaymentContainer>();
        public IList<LNSM_PostPaymentContainer> lstStorPostPaymentCont { get; set; } = new List<LNSM_PostPaymentContainer>();
        public IList<LNSM_PostPaymentCharge> lstPostPaymentChrg { get; set; } = new List<LNSM_PostPaymentCharge>();
        public IList<LNSM_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<LNSM_ContainerWiseAmount>();
        public IList<LNSM_PreContainerWiseAmount> lstPreContWiseAmount { get; set; } = new List<LNSM_PreContainerWiseAmount>();
        public IList<LNSM_PreInvoiceCargo> lstPreInvoiceCargo { get; set; } = new List<LNSM_PreInvoiceCargo>();
        public List<LNSM_CashReceipt> CashReceiptDetails { get; set; } = new List<LNSM_CashReceipt>();
              
        public List<LNSM_CfsCodewiseRateHT> lstCfsCodewiseRateHT = new List<LNSM_CfsCodewiseRateHT>();
       

    }
    
    public class LNSM_PostPaymentContainer
    {
        public string CFSCode { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Reefer { get; set; } = 0;
        public int Insured { get; set; } = 0;
        public int RMS { get; set; } = 0;
        public int HeavyScrap { get; set; } = 0;
        public int AppraisementPerct { get; set; } = 0;
        public int CargoType { get; set; } = 0;
        public string ArrivalDate { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public DateTime? CartingDate { get; set; } = null;
        public DateTime? StuffingDate { get; set; } = null;
        public DateTime? DestuffingDate { get; set; } = null;
        public int NoOfPackages { get; set; } = 0;
        public decimal GrossWt { get; set; } = 0M;
        public decimal WtPerUnit { get; set; } = 0M;
        public decimal SpaceOccupied { get; set; } = 0M;
        public string SpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int DeliveryType { get; set; } = 0;
        public decimal StuffCUM { get; set; } = 0M;
        public string LCLFCL { get; set; }

        public decimal NoOfPackagesDel { get; set; } = 0M;
        public int ISODC { get; set; } = 0;
        public decimal Clauseweight { get; set; } = 0M;
        public string StorageType { get; set; } = string.Empty;
       
    }
    public class LNSM_PostPaymentCharge
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;
        public decimal Discount { get; set; } = 0M;
        public decimal Taxable { get; set; } = 0M;
        public decimal IGSTPer { get; set; } = 0M;
        public decimal IGSTAmt { get; set; } = 0M;
        public decimal CGSTPer { get; set; } = 0M;
        public decimal CGSTAmt { get; set; } = 0M;
        public decimal SGSTPer { get; set; } = 0M;
        public decimal SGSTAmt { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public decimal ActualFullCharge { get; set; } = 0M; 
        public int OperationId { get; set; }
    }
    public class LNSM_ContainerWiseAmount
    {
        public int InvoiceId { get; set; }
        public int ContainerId { get; set; }
        public string CFSCode { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public decimal EntryFee { get; set; } = 0M;
        public decimal CstmRevenue { get; set; } = 0M;
        public decimal GrEmpty { get; set; } = 0M;
        public decimal GrLoaded { get; set; } = 0M;
        public decimal ReeferCharge { get; set; } = 0M;
        public decimal StorageCharge { get; set; } = 0M;
        public decimal InsuranceCharge { get; set; } = 0M;
        public decimal PortCharge { get; set; } = 0M;
        public decimal WeighmentCharge { get; set; } = 0M;
    }   
    public class LNSM_ClauseTypeForHT
    {
        public string OperationCode { get; set; }
        public string ClauseOrder { get; set; }
        public string CommodityType { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }
        public string OperationDesc { get; set; }
        public string SacCode { get; set; }
        public string RateCWC { get; set; }
        public string ChargeType  { get; set; }

       public string Quantity { get; set; }
    }
    public class LNSM_CfsCodewiseRateHT
    {
        public string CFSCode { get; set; } = string.Empty;
        public string Clause { get; set; } = string.Empty;      
        public string Size { get; set; } = string.Empty;
        public string CommodityType { get; set; } = string.Empty;
        public decimal Rate { get; set; } = 0;
    }

}