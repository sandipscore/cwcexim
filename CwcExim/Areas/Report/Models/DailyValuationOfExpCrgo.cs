using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DailyValuationOfExpCrgo
    {
        public string ShippingBillNo { get; set; }
        public string CommodityName { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal FobValue { get; set; }
        public string CHAName { get; set; }
        public string InsuredBy { get; set; }
    }
}