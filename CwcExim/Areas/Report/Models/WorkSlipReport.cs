using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WorkSlipReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string WorkSlipType { get; set; }
        public List<WorkSlipSummary> WorkSlipSummaryList { get; set; }
        public List<WorkSlipDetail> WorkSlipDetailList { get; set; }
    }

    public class WorkSlipSummary
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

    public class WorkSlipDetail
    {
        public string Clause { get; set; }
        public string ContainerSize { get; set; }
        public string Containers { get; set; }
    }

    public class WorkSlipCartingReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CCINNo { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
        public string NoOfPackage { get; set; }
        public string Weight { get; set; }
        public string Vehicle { get; set; }
        public string Amount { get; set; }

    }
}