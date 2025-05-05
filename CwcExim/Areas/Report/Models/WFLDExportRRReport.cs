using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDExportRRReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; } 
        public string PeriodFrom { get; set; }
      public int GatePassId { get; set; }
        public string PeriodTo { get; set; }
    }
}