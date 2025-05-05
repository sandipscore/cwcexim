using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class ChnInvoiceBase
    {
        public string CompanyGstNo { get; set; }
        public string PartyCode { get; set; }
        public string PartyGstNo { get; set; }
        public decimal TotalTax { get; set; }
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
        public decimal TotalAmt { get; set; } = 0M;
        public decimal TotalDiscount { get; set; } = 0M;
        public decimal TotalTaxable { get; set; } = 0M;
        public decimal TotalCGST { get; set; } = 0M;
        public decimal TotalSGST { get; set; } = 0M;
        public decimal TotalIGST { get; set; } = 0M;
        public decimal CWCAmtTotal { get; set; } = 10M;
        public decimal CWCTotal { get; set; } = 0M;
        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal { get; set; } = 0M;
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS { get; set; } = 0M;
        public decimal HTTDS { get; set; } = 0M;
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
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
        public string Module { get; set; } = string.Empty;

        public string ExaminationType { get; set; } = string.Empty;


        public int Weighment { get; set; }
        public int Scanning { get; set; }
        public int OwnMovement { get; set; }
        public int FactoryDestuffing { get; set; }
        
        //subir for edir receipt
        /*public string CashierRemarks { get; set; } = string.Empty;

        public string PDAadjustedCashReceiptEdit { get; set; } = string.Empty;
        // end 
        public int DeliveryType { get; set; } = 1;
        public string BillType { get; set; } = string.Empty;
        public string StuffingDestuffDateType { get; set; } = string.Empty;
        public string StuffingDestuffingDate { get; set; } = string.Empty;
        public string ImporterExporterType { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        
        public string UptoDate { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0M;
        public int OperationType { get; set; }
        public int FromGodownId { get; set; } 
        public string OldGodownName { get; set; }
        public int ToGodownId { get; set; }
        public string NewGodownName { get; set; }
        public int OldLocationIds { get; set; }
        public string OldLctnNames { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public int MovementId { get; set; } = 0;
        public decimal OTHours { get; set; } = 0;*/
    }

    public class ChnInvoiceContainerBase
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int CargoType { get; set; } = 0;
        public int Reefer { get; set; } = 0;
        public int Insured { get; set; } = 0;
        public int RMS { get; set; } = 0;
        public string ArrivalDate { get; set; }
        public string DestuffingDate { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public int NoOfPackages { get; set; } = 0;
        public decimal GrossWt { get; set; } = 0;
        public decimal WtPerUnit { get; set; } = 0;
        public decimal SpaceOccupied { get; set; } = 0;
        public decimal SpaceOccupiedUnit { get; set; } = 0;
        public decimal CIFValue { get; set; } = 0;
        public decimal Duty { get; set; } = 0;
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string ShippingLineName { get; set; }
        public string CHAName { get; set; }
        public string ImporterExporter { get; set; }
    }
    public class ChnInvoiceChargeBase
    {
        public int ChargeId { get; set; }
        public string OperationId { get; set; }
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxable { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal Total { get; set; }
    }
    public class CHN_ContWiseAmount
    {
        public string CFSCode { get; set; }
        public decimal EntryFee { get; set; }
        public decimal CstmRevenue { get; set; }
        public decimal GrEmpty { get; set; }
        public decimal GrLoaded { get; set; }
        public decimal ReeferCharge { get; set; }
        public decimal StorageCharge { get; set; }
        public decimal InsuranceCharge { get; set; }
        public decimal PortCharge { get; set; }
        public decimal WeighmentCharge { get; set; }
    }
    public class CHN_OperationContWiseCharge
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Clause { get; set; }
    }
    public class CHN_CargoDtl
    {
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string CargoDescription { get; set; }
        public string GodownId { get; set; }
        public string GodownName { get; set; }
        public string GdnWiseLctnIds { get; set; }
        public string GdnWiseLctnNames { get; set; }
        public string CargoType { get; set; }
        public string DestuffingDate { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWt { get; set; }
        public string WtPerUnit { get; set; }
        public string SpaceOccupied { get; set; }
        public string SpaceOccupiedUnit { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }
}