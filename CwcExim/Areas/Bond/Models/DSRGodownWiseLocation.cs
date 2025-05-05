using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRGodownWiseLocation
    {
        public string LocationId { get; set; }
        public int GodownId { get; set; }
        public string LocationName { get; set; }
        public decimal UnloadedUnit { get; set; }
        public decimal BalancedUnit { get; set; }
        public decimal UnloadedWeight { get; set; }
        public decimal BalancedWeight { get; set; }
        public decimal SpaceUnreserved { get; set; }
        

    }
}