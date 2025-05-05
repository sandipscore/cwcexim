using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_WorkSlipReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string WorkSlipType { get; set; } = string.Empty;
        public List<CHN_WorkSlipSummary> WorkSlipSummaryList { get; set; }
        public List<CHN_WorkSlipDetail> WorkSlipDetailList { get; set; }
    }

    public class CHN_WorkSlipSummary
    {
        public int SrNo { get; set; }
        public string Particulars { get; set; }
        public string Clause { get; set; }
        public string SAC { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public int? Count_20 { get; set; }
        public int? Count_40 { get; set; }
    }

    public class CHN_WorkSlipDetail
    {
        public string Clause { get; set; }
        public string ContainerSize { get; set; }
        public string Containers { get; set; }
    }
}