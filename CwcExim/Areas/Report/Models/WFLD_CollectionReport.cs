using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_CollectionReport
    {
        public string TotalCash { get; set; }
        public string TotalBank { get; set; }

        public string TotalPd { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Date { get; set; }

        public string Cash { get; set; }

        public string Bank { get; set; } //cheque

        public string PO { get; set; }

        public string DD { get; set; }

        public string Pd { get; set; } //neft

        public string Chln { get; set; }

        public string Total { get; set; }
    }
    public class WFLD_FinalCollectionReportTotal
    {
        public IList<WFLD_CollectionReport> listCollectionReport = new List<WFLD_CollectionReport>();
        public WFLD_CollectionReportTotal objCollectionReportTotal = new WFLD_CollectionReportTotal();
    }

    public class WFLD_CollectionReportTotal
    {
        public string TotalCash { get; set; }
        public string TotalPDA { get; set; }
        public string TotalBank { get; set; }

        public string TotalPO { get; set; }

        public string TotalCHALLAN { get; set; }

        public string TotalDraft { get; set; }
    }
}