using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kol_MiscInvoice
    {
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
        public decimal Round_up { get; set; }         
        public decimal InvoiceAmt { get; set; }
        public string Naration { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public IList<kol_MiscInvDtl> lstBTTCargoEntryDtl { get; set; } = new List<kol_MiscInvDtl>();
        public string MiscInvModelJson { get; set; }
        public string BTTCargoEntryDtlJS { get; set; }
    }

    public class kol_MiscInvDtl
    {
        public string Purpose { get; set; }
        public string PurposeCode { get; set; }        
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public string SACCode { get; set; }
        public decimal TotalAmt { get; set; } = 0M;

    }

}