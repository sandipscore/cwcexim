using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class SDSummary
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string Module { get; set; }
        public string Date { get; set; }
        public string EximTraderName { get; set; }

        public string PayeeName { get; set; }
        public decimal BILL { get; set; }
        public decimal GEN { get; set; }
        public decimal STO { get; set; }
        public decimal INS { get; set; }
        public decimal GRE { get; set; }
        public decimal GRL { get; set; }
        public decimal MFCHRG { get; set; }
        public decimal MFTAX { get; set; }
        public decimal PDA { get; set; }
        public decimal ENT { get; set; }
        public decimal FUM { get; set; }
        public decimal OT { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal MISC { get; set; }
        public string TDS { get; set; }
        public decimal CRTDS { get; set; }
        public string PaymentType { get; set; }

        public string Remarks { get; set; }
    }
}