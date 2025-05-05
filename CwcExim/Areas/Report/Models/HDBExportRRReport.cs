using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class HDBExportRRReport
    {
        public string InvoiceNo { get; set; }
        public string CFSCode { get; set; }
        public string PeriodFrom { get; set; }      
        public string PeriodTo { get; set; }
        public string ContainerNo { get; set; }
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
    }
}