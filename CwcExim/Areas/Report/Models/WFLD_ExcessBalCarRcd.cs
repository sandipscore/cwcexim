using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
   // shipping bill no / Exporter / CHA/ Excess balance /Shed / Slot.For daily record this report is needed
    public class WFLD_ExcessBalCarRcd 
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string SBNo { get; set; }
        public string Exporter { get; set; }
        public string CHA { get; set; }
        public decimal PKG { get; set; }
        public decimal Excess { get; set; }
        public string Shed { get; set; }
        public string Slot { get; set; }
    }
}