using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_ChequeSummary
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string Bank { get; set; }
        public string ChequeNumber { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Party { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReceiptNo { get; set; }
        public string Type { get; set; }
        public string PaymentType { get; set; }
        public decimal CashAmount { get; set; }
        public decimal ChequeAmount { get; set; }
        public decimal POSAmount { get; set; }


        public decimal GCashAmount { get; set; }
        public decimal GChequeAmount { get; set; }
        public decimal GPOSAmount { get; set; }

        public decimal GOthersAmount { get; set; }

        public decimal SDAmount { get; set; }
    }
}