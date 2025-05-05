using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_EscalationReport
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Party { get; set; }
        public decimal TaxableAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal TotalAmt { get; set; }

    }
}