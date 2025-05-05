using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class CreditNote
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
        public string CreditNoteType { get; set; }
        //   #Added two column PayeeId and PayeeName for Task Id CWU-538 Developed by Kalyan Saha as on 30/05/2020,1430

        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string SupplyType { get; set; }
    }
    public class ListOfInvoiceNo
    {
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }

        public string InvoiceType { get; set; }
    }
    public class InvoiceDetails
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
        public IList<InvoiceCarges> lstInvoiceCarges { get; set; } = new List<InvoiceCarges>();

        //   #Added two column PayeeId and PayeeName for Task Id CWU-538 Developed by Kalyan Saha as on 30/05/2020,1430

        public int PayeeId { get; set; }
        public string PayeeName { get; set; }

        public string SupplyType { get; set; }
    }
    public class InvoiceCarges
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
    public class ListOfCRNote
    {
        public int CRNoteId { get; set; }
        public string CRNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo{ get; set; }
        public string InvoiceDate{ get; set; }
        public string PartyName{ get; set; }
    }
    public class PrintModelOfCr
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
        public IList<ChargesModel> lstCharges { get; set; } = new List<ChargesModel>();

        //   #Added one column PayeeName (Whatsup group) for Print Developed by Dilip Samanta as on 03/06/2020,1430
        public string PayeeName { get; set; }
        public string irn { get; set; }
        public string SignedQRCode { get; set; }
        public string SupplyType { get; set; }


        public string ver { get; set; }
        public int orgId { get; set; }
        public int mode { get; set; }
        public string tid { get; set; }

        public string tr { get; set; }
        public string tn { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public decimal am { get; set; }
        public decimal mam { get; set; }
        public string mid { get; set; }
        public string msid { get; set; }
        public string mtid { get; set; }
        public string gstBrkUp { get; set; }
        public int qrMedium { get; set; }

        public string invoiceNo { get; set; }

        public string QRInvoiceDate { get; set; }
        public string InvoiceName { get; set; }
        public string QRexpire { get; set; }

        public int pinCode { get; set; }
        public string tier { get; set; }



        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal GSTIncentive { get; set; }
        public decimal GSTPCT { get; set; }
        public int CrNoteId { get; set; }

        public string gstIn { get; set; }
    }
    public class PrintModelOfBulkCrCompany
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompStateName { get; set; }
        public string CompStateCode { get; set; }
        public string CompCityName { get; set; }
        public string CompGstIn { get; set; }
        public string CompPan { get; set; }
        public IList<ChargesModel> lstCharges { get; set; } = new List<ChargesModel>();
        public IList<PrintModelOfBulkCrParty> lstCrParty { get; set; } = new List<PrintModelOfBulkCrParty>();
    }
    public class PrintModelOfBulkCrParty { 
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
        public string irn { get; set; }
        public string SignedQRCode { get; set; }
        public string SupplyType { get; set; }
        public string PayeeName { get; set; }

    }
    public class ChargesModel
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