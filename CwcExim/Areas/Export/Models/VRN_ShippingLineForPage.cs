using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public IList<VRN_ShippingLineForPage> lstApplication { get; set; } = new List<VRN_ShippingLineForPage>();
    }
}