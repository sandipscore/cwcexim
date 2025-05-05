using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PPGSDList:SDList
    {
        public decimal AdjustAmount { get; set; }
        public decimal UtilizationAmount { get; set; }
        public decimal RefundAmount { get; set; }
    }
}