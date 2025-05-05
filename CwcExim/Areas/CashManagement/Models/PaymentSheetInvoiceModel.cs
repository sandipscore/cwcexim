using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PaymentSheetInvoiceModel
    {
        public bool InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string StuffingNo { get; set; }
        public string Date { get; set; }
        public string Party { get; set; }
        public string Payee { get; set; }
        public string GSTNo { get; set; }
    }
}