using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_AssessmentReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ASMDate { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string ASMType { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public Decimal PKGS { get; set; }
        public Decimal Area { get; set; }
        public Decimal CBM { get; set; }
        public Decimal Value { get; set; }
        public Decimal Duty { get; set; }
        public String SLA { get; set; }
        public String IMP { get; set; }
        public String CHA { get; set; }
        public Decimal TotalAmount { get; set; }

    }
}