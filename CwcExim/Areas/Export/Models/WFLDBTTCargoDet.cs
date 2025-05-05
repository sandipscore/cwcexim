using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDBTTCargoDet
    {
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
        public string exporter { get; set; }
        public string CartingDate { get; set; }
        public decimal BTTCum { get; set; }
    }
}