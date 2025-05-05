using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRSDSummary
    {
        public int InvoiceId { get; set; }
        public string Date { get; set; }
        public string InvoiceNo { get; set; }        
        public string Module { get; set; }        
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string PaymentType { get; set; }        
        public decimal GEN { get; set; }
        public decimal STO { get; set; }
        public decimal INS { get; set; }
        public decimal GRE { get; set; }
        public decimal GRL { get; set; }
        public decimal HT { get; set; }
        public decimal OTHS { get; set; }
        public decimal WET { get; set; }
        public decimal RCTSEAL { get; set; }
        public decimal DOC { get; set; }
        public decimal MISC { get; set; }
        public decimal PCS { get; set; }                  
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }       
        public decimal Total { get; set; }        
        public string TDS { get; set; }
        public decimal CRTDS { get; set; }
        public decimal RoundUp { get; set; }
        public string Remarks { get; set; }
        public decimal BILL { get; set; }
        
    }
}