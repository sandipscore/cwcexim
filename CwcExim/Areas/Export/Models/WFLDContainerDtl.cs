using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDContainerDtl: ContainerDtl
    {
        public string ShippingBillNo { get; set; } 
        public string ShippingBillDate { get; set; }

        public decimal SQM { get; set; }
        public string spacetype { get; set; }

    }
}