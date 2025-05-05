using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Viz_ContStufAckSearch
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string shippingbillno { get; set; }
        public string shippingbilldate { get; set; }
    }

    public class Viz_ContStufAckRes
    {
        public string shipbill { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
    }
}
