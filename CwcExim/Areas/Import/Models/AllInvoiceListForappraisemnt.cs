using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class AllInvoiceListForappraisemnt
    {
        public int invoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public string StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }
    }
}