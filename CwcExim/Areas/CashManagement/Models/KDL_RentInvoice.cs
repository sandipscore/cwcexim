using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class KDL_RentInvoice: WFLD_RentInvoice
    {
        public string InvoiceHtml { get; set; } = string.Empty;
        public List<KDLRentDetails> lstPrePaymentContt { get; set; } = new List<KDLRentDetails>();


    }



    public class KDLRentDetails
    {
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string GSTNo { get; set; }
        public string Address { get; set; }

        public string State { get; set; }
        public string StateCode { get; set; }
        public Decimal amount { get; set; }
        public Decimal cgst { get; set; } = 0;
        public Decimal sgst { get; set; } = 0;
        public Decimal igst { get; set; } = 0;
        public Decimal round_up { get; set; } = 0;
        public Decimal total { get; set; } = 0;
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public string Remarks { get; set; }

    }

}