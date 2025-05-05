using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class MonthlyPerformaceReport
    {
        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public int DescriptionId { get; set; }
        public bool IsHeader { get; set; } 
        public string Particulars { get; set; } 
        public string MonthUnderReport { get; set; }
        public string PrevMonth { get; set; }
        public string CorresMonthPrevYear { get; set; }
    }

}