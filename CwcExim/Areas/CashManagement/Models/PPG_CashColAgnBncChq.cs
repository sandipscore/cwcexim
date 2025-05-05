using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PPG_CashColAgnBncChq: CashColAgnBncChq
    {
        public string Invoice_No { get; set; }
    }
    public class ppg_cashBncChqInvoice
    {
        public int Status { get; set; }
        public string InvoiceNo { get; set; }
    }
}