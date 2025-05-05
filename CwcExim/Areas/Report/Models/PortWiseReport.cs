using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PortWiseReport
    {

        public string SlaCode { get; set; }
        public string ShippingLine { get; set; }
        public int RLoni { get; set; }
        public int RTkd { get; set; }
        public int ROth { get; set; }
        public int TLoni { get; set; }
        public int TOth { get; set; }
        public int TTkd { get; set; }
        public List<PortWiseReport> PortWiseReportList { get; set; } = new List<PortWiseReport>();
        public int TotalFCL { get; set; }
        public int TotalLCL { get; set; }
        
    }
}