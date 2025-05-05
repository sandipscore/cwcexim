using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class StockPositionReportModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string GodownNo { get; set; }
        public string Commodity { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public string Party { get; set; }
    }
}