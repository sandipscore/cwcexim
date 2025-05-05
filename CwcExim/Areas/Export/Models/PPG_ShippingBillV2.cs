using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_ShippingBillV2
    {
        public string shippingBillNo { get; set; }
        public decimal CargoWeight { get; set; }
        public int CargoType { get; set; }
    }
}