using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ShippingLine
    {
        public string ShippingLineName { get; set; }
        public int ShippingLineId { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
    }



    public class ForeignLiner
    {
        public string ForeignLinerName { get; set; }
       
    }
}