using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
  //  Sr No | Entry No | Carting Date | SB No | SB Date | Exp | CHA | Cargo | No Pkg | Gr Wt | FOB | Slot | G / R | CBM
    public class WFLD_CargoCartingShedWise
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        public string EntryNo { get; set; }
        public string CartingDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string Exporter { get; set; }
        public string CHA { get; set; }
        public string Cargo { get; set; }
        public decimal NoOfPKG { get; set; }
        public decimal GrWt { get; set; }
        public decimal FOB { get; set; }
        public string Slot { get; set; }
        public string Space { get; set; }
        public decimal CBM { get; set; }
    }
}