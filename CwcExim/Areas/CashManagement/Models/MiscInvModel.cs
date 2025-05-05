using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class MiscInvModel
    {

     
        public string GstIn { get; set; }
      
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string DeliveryDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
      
        public string PartyGST { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal Amount { get; set; } =0;
        public decimal CGST { get; set; } = 0;
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;

        public string SEZ { get; set; }
    }

    public class PurposeListForInvc
    {
       public string Purpose { get; set; }

        public IEnumerable<PurposeListForInvc> PurposeList { get; set; }
    }
    public class Hdb_MiscInvModel
    {
        public string GstIn { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string PartyGST { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public decimal CGST { get; set; } = 0;
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public decimal NetAmt { get; set; } = 0;
        public string ExportUnder { get; set; }

        public string HSNCode { get; set; }
        public string InvoiceHtml { get; set; }
    }
}