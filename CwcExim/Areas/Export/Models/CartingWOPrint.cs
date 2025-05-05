using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class CartingWOPrint
    {
        public string CartingNo { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string CHAName { get; set; }
        public string ExpName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public string GodownName { get; set; }
        public string CompanyAddress { get; set; }
    }
}