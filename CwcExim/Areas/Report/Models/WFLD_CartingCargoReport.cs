using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_CartingCargoReport
    {

        public string FromDate { get; set; }

        public string ToDate { get; set; }
        public string EntryNo { get; set; }

        public string SBNo { get; set; }

        public string SBDate { get; set; }
        public string ExpCod { get; set; }

        public decimal NoOfPkg { get; set; }

        public decimal GrWT { get; set; }

        public decimal FOB { get; set; }

        public string Truck { get; set; }

        public string InDate { get; set; }
    }
}