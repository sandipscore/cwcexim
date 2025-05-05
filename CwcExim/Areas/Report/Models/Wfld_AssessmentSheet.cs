using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Wfld_AssessmentSheet
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AssessmentType { get; set; }
    }

    public class Wfld_AssessmentSheetDtl
    {
        public string AsmNo { get; set; }
        public string AsmDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string OBL { get; set; }
        public string TSA { get; set; }
        public string AsmType { get; set; }
        public string BOE { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
     
    }
}