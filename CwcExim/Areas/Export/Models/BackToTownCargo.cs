using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Export.Models
{
    public class BackToTownCargo
    {
        public int BttCargoEntryId { get; set; }
        public string BTTNo { get; set; }
        public string BTTDate { get; set; }
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public int CartingRegisterId { get; set; }
        public string CartingDate { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string Remarks { get; set; }
        public string XMLText { get; set; }
    }
    public class BackToTownCargoDtl
    {
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal BTTQty { get; set; }
        public decimal BTTWt { get; set; }
    }
}