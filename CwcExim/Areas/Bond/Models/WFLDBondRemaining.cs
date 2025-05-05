using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class WFLDBondRemaining
    {
        public decimal ClosingUnits { get; set; }
        public decimal ClosingWeight { get; set; }
        public decimal ClosingArea { get; set; }
        public decimal ClosingValue { get; set; }
        public decimal ClosingDuty { get; set; }
    }
}