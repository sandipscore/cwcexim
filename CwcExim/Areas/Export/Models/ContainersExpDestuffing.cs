using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ContainersExpDestuffing
    {
        public int ContainerStuffingId { get; set; }
        public int ContainerStuffingDtlId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }
}