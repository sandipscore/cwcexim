using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_DestuffingDailyReport
    {
       [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string CFSCode { get; set; }
        public string DestuffingDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string PortName { get; set; }
        public string Weight { get; set; }

    }
}