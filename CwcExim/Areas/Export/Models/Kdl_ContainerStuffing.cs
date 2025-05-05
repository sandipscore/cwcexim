using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kdl_ContainerStuffing: ContainerStuffing
    {
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string AmendmentDate { get; set; }
    }
}