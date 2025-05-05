using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Chn_ServiceCodeWiseInvDtlsRpt
    {
        public string SAC { get; set; }
        public string InvoiceNumber { get; set; }

        public string Date { get; set; }
    
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }

        public decimal IGSTAmt { get; set; }
        public decimal TotalValue { get; set; }
    }
}