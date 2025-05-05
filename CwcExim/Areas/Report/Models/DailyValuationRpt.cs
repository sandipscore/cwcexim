using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DailyValuationRpt
    {
        public string ContainerNo { get; set; }
        public int Size { get; set; }
        public string Party { get; set; }
        public string CommodityName { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Duty { get; set; }
        public decimal CIFValue { get; set; }
    }
}