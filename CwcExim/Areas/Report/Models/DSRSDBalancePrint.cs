using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRSDBalancePrint
    {
        public decimal SDBalance { get; set; }
        public string PortName { get; set; }
        public string EmptyFrom { get; set; }
        public string EmptyTo { get; set; }
        public string LoadFrom { get; set; }
        public string LoadTo { get; set; }

        public string MovementType { get; set; }

    }
    public class DSRDaysWeeks
    {
        public int Days { get; set; }
        public int FreeDays { get; set; }
        //public int Weeks { get; set; }
    }
}