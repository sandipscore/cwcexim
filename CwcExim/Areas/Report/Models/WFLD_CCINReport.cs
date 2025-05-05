using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
  
    public class WFLD_CCINReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string CCINNo { get; set; }
        public string CCINDate { get; set; }
        public decimal PKG { get; set; }
        public decimal GrWt { get; set; }
        public string InvoiceNo { get; set; }
        public string Cargo { get; set; }
        public string Exporter { get; set; }
        public string CHA { get; set; }

    }
}