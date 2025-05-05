using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_WorkSlipReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string WorkSlipType { get; set; }
        public List<HDB_WorkSlipSummary> WorkSlipSummaryList { get; set; }
        public List<HDB_WorkSlipDetail> WorkSlipDetailList { get; set; }
    }

    public class HDB_WorkSlipSummary
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

    public class HDB_WorkSlipDetail
    {
        public string Clause { get; set; }
        public string ContainerSize { get; set; }
        public string Containers { get; set; }
    }

    public class HDB_WorkSlipNew
    {
        public int SerialNo { get; set; }
// public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ClauseNo { get; set; }
        public string SAC { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string NoOfPackages { get; set; }
        public string PortName { get; set; }
        public string Weight { get; set; }
        public string WeightQtl { get; set; }
        public string Distance { get; set; }
        public string CFSCode { get; set; }
        public string WorkOrderNo { get; set; }
        public string Remarks { get; set; }
    }
}