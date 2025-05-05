using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class GenerateInvoiceCRPrint
    {
        public IList<PrintInvoiceHeader> lstInvoiceHeader { get; set; } = new List<PrintInvoiceHeader>();
        public IList<PrintInvoiceCharges> lstInvoiceCharges { get; set; } = new List<PrintInvoiceCharges>();
        public IList<PrintInvoiceContainers> lstInvoiceContainers { get; set; } = new List<PrintInvoiceContainers>();
        public IList<PrintInvoiceCompanyDetails> lstInvoiceCompanyDetails { get; set; } = new List<PrintInvoiceCompanyDetails>();
        public IList<PrintInvoiceCRDetails> lstInvoiceCRDetails { get; set; } = new List<PrintInvoiceCRDetails>();
    }
    public class PrintInvoiceHeader
    {
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryDate { get; set; }
        public string PartyName { get; set; }
        public string PartyGSTNo { get; set; }
        public string PartyAddress { get; set; }
        public string PartyState { get; set; }
        public string PartyStateCode { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalIGST { get; set; }
        public decimal CWCTotal { get; set; }
        public decimal HTTotal { get; set; }
        public decimal CWCTDS { get; set; }
        public decimal HTTDS { get; set; }
        public decimal TDS { get; set; }
        public decimal TDSCol { get; set; }
        public decimal AllTotal { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string ShippingLinaName { get; set; }
        public string CHAName { get; set; }
        public string ExporterImporterName { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string TotalNoOfPackages { get; set; }
        public string TotalGrossWt { get; set; }
        public string TotalWtPerUnit { get; set; }
        public string TotalSpaceOccupied { get; set; }
        public string TotalSpaceOccupiedUnit { get; set; }
        public string TotalValueOfCargo { get; set; }
        public string CompGST { get; set; }
        public string CompPAN { get; set; }
        public string CompStateCode { get; set; }
        public string CstmExaminationDate { get; set; }
        public string DestuffingDate { get; set; }
        public string StuffingDate { get; set; }
        public string CartingDate { get; set; }
        public string ArrivalDate { get; set; }
        public string CFSCode { get; set; }
        public string Remarks { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string CashierRemarks { get; set; }
        public int PdaAdjust { get; set; }
        public decimal PdaOpening { get; set; }
        public decimal PdaClosing { get; set; }
        public decimal PdaAdjustedAmount { get; set; }
    }
    public class PrintInvoiceCharges
    {
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public decimal Amount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Total { get; set; }
    }
    public class PrintInvoiceContainers
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ArrivalDateTime { get; set; }
        public int CargoType { get; set; }
    }
    public class PrintInvoiceCRDetails
    {
        public string PayMode { get; set; }
        public string InstrumentNo { get; set; }
        public string DraweeBank { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class PrintInvoiceCompanyDetails
    {
        public int? CompanyId { get; set; }
        public string ROAddress { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? StateId { get; set; }
        public string StateCode { get; set; }
        public int? CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
    }

}