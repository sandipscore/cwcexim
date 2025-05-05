using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class TransactionStatusEnquiry
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }
}