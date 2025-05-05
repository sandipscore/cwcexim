using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_CollectionReport
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
    public class Ppg_FinalCollectionReportTotal
    {
        public IList<Ppg_CollectionReport> listCollectionReport = new List<Ppg_CollectionReport>();
        public Ppg_CollectionReportTotal objCollectionReportTotal = new Ppg_CollectionReportTotal();
    }

    public class Ppg_CollectionReportTotal
    {
        public string TotalCash { get; set; }
        public string TotalPDA { get; set; }
        public string TotalBank { get; set; }

        public string TotalPO { get; set; }

        public string TotalCHALLAN { get; set; }

        public string TotalDraft { get; set; }
    }
}