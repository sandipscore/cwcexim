using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Kol_MiscInv
    {
       
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal Round_up { get; set; }
        public string SACCode { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal InvoiceAmt { get; set; }
        public decimal TotalAmt { get; set; } = 0M;
      
    }


   
    public class KolMiscPostModel
    {
        public decimal InvoiceAmt { get; set; }
        public string SACCode { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public string ChargeName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }


        public string GstIn { get; set; }

        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string DeliveryDate { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;

        public string PartyGST { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string PurposeCode { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public decimal CGST { get; set; } = 0;
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
    }
    public class KolPaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyAlias { get; set; }
    }

    public class KolListOfMiscInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
    }
}