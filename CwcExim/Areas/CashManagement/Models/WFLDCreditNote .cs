using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{

    
    public class WFLDCreditNote
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

        public string SupplyType { get; set; }
    }
    public class WFLDInvoiceDetails
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
        public string SupplyType { get; set; }

        public IList<InvoiceCarges> lstInvoiceCarges { get; set; } = new List<InvoiceCarges>();
    }

    public class WFLDChargeNameCrDb
    {
        public int Sr { get; set; }
        public string ChargeName { get; set; }
        public string Clause { get; set; }
        public string SACCode { get; set; }

        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public int IsLocalGST { get; set; }
    }
}