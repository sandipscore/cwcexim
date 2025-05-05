using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public IList<PPG_ShippingLineForPage> lstApplication { get; set; } = new List<PPG_ShippingLineForPage>();
    }
}