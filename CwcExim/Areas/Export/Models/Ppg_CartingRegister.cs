using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class Ppg_CartingRegister : CartingRegister
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string SpaceType { get; set; }
        public int IsShortCargo { get; set; }
        public string ShippingBillNo { get; set; }
        public List<ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<ShortCargoDetails>();

    }
    public class ShortCargoDetails
    {
        public int ShortCargoDtlId { get; set; }
        public string CartingDate { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }
    }
}