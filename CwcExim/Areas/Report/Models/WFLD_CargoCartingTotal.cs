using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
  //  Sr No | Entry No | Carting Date | Sb No | Sb Date | Exp | CHA | Cargo | No Pkg | Gr Wt | Fob | Slot | G /R | Area
    public class WFLD_CargoCartingTotal
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EntryNo { get; set; }
        public string CartingDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string Exporter { get; set; }
        public string CHA { get; set; }
        public string Cargo { get; set; }
        public decimal NoOfPKG { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal FOB { get; set; }
        public string Slot { get; set; }
        public string Space { get; set; }
        public decimal Area { get; set; }
        public string InvoiceNo { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}