using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_AssessmentSheetLCL
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

    }

    public class WFLD_AssessmentSheetLCLDtl
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PayeeName { get; set; }
        public string PayeeCode { get; set; }
        public string ImporterName { get; set; }
        public string BOENo { get; set; }
        public decimal BOEValueDuty { get; set; }
        public decimal Area { get; set; }
        public decimal GrossWt { get; set; }
        public int Week { get; set; }
        public string CargoType { get; set; }
        public decimal ENT { get; set; }
        public decimal HND { get; set; }
        public decimal STO { get; set; }
        public decimal INS { get; set; }
        public decimal OTI { get; set; }
        public decimal HAZ { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
    }
}