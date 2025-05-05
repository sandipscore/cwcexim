using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{

    public class VRN_CreditNote
    {
        public int CRNoteId { get; set; }
        public string CRNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyGSTNo { get; set; }
        public string PartyAddress { get; set; }
        public string PartyState { get; set; }
        public string PartyStateCode { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal RoundUp { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public string Module { get; set; }
        public string CRNoteHtml { get; set; }
        public string ChargesJson { get; set; }

        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
    }
    public class VRN_InvoiceDetails
    {
        public string Module { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyGSTNo { get; set; }
        public string PartyAddress { get; set; }
        public string PartyState { get; set; }
        public string PartyStateCode { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public IList<VRN_InvoiceCarges> lstInvoiceCarges { get; set; } = new List<VRN_InvoiceCarges>();
        public string SupplyType { get; set; }
    }
    public class VRN_InvoiceCarges
    {
        public int ChargesTypeId { get; set; }
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Taxable { get; set; }
        public decimal RetValue { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal Total { get; set; }
    }

    public class VRN_ListOfCRNote
    {
        public int CRNoteId { get; set; }
        public string CRNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }

        public string irn { get; set; }
        public string SignedQRCode { get; set; }
        public string SupplyType { get; set; }
    }
    public class VRN_PrintModelOfCr
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompStateName { get; set; }
        public string CompStateCode { get; set; }
        public string CompCityName { get; set; }
        public string CompGstIn { get; set; }
        public string CompPan { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PartyCityName { get; set; }
        public string PartyStateName { get; set; }
        public string PartyStateCode { get; set; }
        public string PartyGSTIN { get; set; }
        public string CRNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal RoundUp { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public IList<VRN_ChargesModel> lstCharges { get; set; } = new List<VRN_ChargesModel>();
        public string PayeeName { get; set; }
        public string irn { get; set; }
        public string SignedQRCode { get; set; }
        public string SupplyType { get; set; }
    }
    public class VRN_PrintModelOfBulkCrCompany
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompStateName { get; set; }
        public string CompStateCode { get; set; }
        public string CompCityName { get; set; }
        public string CompGstIn { get; set; }
        public string CompPan { get; set; }
        public IList<VRN_ChargesModel> lstCharges { get; set; } = new List<VRN_ChargesModel>();
        public IList<VRN_PrintModelOfBulkCrParty> lstCrParty { get; set; } = new List<VRN_PrintModelOfBulkCrParty>();
    }
    public class VRN_PrintModelOfBulkCrParty
    {
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PartyCityName { get; set; }
        public string PartyStateName { get; set; }
        public string PartyStateCode { get; set; }
        public string PartyGSTIN { get; set; }
        public string CRNoteNo { get; set; }
        public int CRNoteId { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal RoundUp { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }

    }
    public class VRN_ChargesModel
    {
        public string ChargeName { get; set; }
        public decimal Taxable { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Total { get; set; }
        public string SACCode { get; set; }
        public int CRNoteId { get; set; }
    }
}