using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDSDBalancePrint
    {
        public decimal SDBalance { get; set; }
        public int TotalSbNo { get; set; }

        public String TransporterName { get; set; }
        public String CHAName { get; set; }
        public String CustomSeal { get; set; }
        public String LinerSeal { get; set; }
        public String Country { get; set; }

    }
    public class WFLDDaysWeeks
    {
        public int Days { get; set; }
        //public int Weeks { get; set; }
    }
}