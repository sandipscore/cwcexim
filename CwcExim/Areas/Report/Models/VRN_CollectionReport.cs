using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_CollectionReport: Ppg_CollectionReport
    {
    }


    public class VRN_FinalCollectionReportTotal
    {
        public IList<VRN_CollectionReport> listCollectionReport = new List<VRN_CollectionReport>();
        public VRN_CollectionReportTotal objCollectionReportTotal = new VRN_CollectionReportTotal();
    }

    public class VRN_CollectionReportTotal
    {
        public string TotalCash { get; set; }
        public string TotalPDA { get; set; }
        public string TotalBank { get; set; }

        public string TotalPO { get; set; }

        public string TotalCHALLAN { get; set; }

        public string TotalDraft { get; set; }
    }
}