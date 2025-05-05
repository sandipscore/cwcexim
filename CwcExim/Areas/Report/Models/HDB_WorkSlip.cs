using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_WorkSlip
    {
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string WorkSlipType { get; set; }
        public List<HDB_WorkSlipDetails> WorkSlipDetailList { get; set; }
    }
    public class HDB_WorkSlipDetails
    {
        public int SerialNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ClauseNo { get; set; }
        public string SAC { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string NoOfPackage { get; set; }
        public string PortName { get; set; }
        public string Weight { get; set; }
        public string Distance { get; set; }
        public string CFSCode { get; set; }
        public string WorkOrderNo { get; set; }
        public string Remarks { get; set; }

    }
}