using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PPG_RentInvoice
    {
        public string GstIn { get; set; }

        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string DeliveryDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;

        public string PartyGST { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
        public string Container { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
       public decimal Amount { get; set; } = 0;
        public decimal CGST { get; set; } = 0;
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public List<PpgRentDetails> lstPrePaymentCont { get; set; } = new List<PpgRentDetails>();
        public List<Ppg_RentInvoiceCharge> lstPpgRentInvoiceCharge { get; set; } = new List<Ppg_RentInvoiceCharge>();
        public string ChemicalXML { get; set; }
        public int addeditflg { get; set; }

        public string SEZ { get; set; }
    }

    public class PpgRentDetails
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

    }

}