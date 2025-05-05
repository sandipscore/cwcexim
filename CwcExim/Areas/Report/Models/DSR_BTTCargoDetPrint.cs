using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_BTTCargoDetPrint
    {
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
        public string exporter { get; set; }
        public string CartingDate { get; set; }
        public string DeliveryDate { get; set; }
        public int NoOfDays { get; set; }
        public int NoOfWeeks { get; set; }
        public string CargoType { get; set; }
        public string ODCType { get; set; }
        public decimal Area { get; set; }
    }
}