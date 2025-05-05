using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_AssessmentLCL
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string InvoiceNo { get; set; }

        public string ASMDate { get; set; }

        public string OBLNo { get; set; }
        public string OBLDate { get; set; }

        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string ASMType { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public decimal PKGS { get; set; }
        public decimal GrossWt { get; set; }
        public decimal Area { get; set; }
        public decimal CBM { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public string SLA { get; set; }
        public string IMP { get; set; }
        public string CHA { get; set; }
        public decimal TotalAmount { get; set; }

    }
}