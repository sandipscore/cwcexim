using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRGodownWiseDtl
    {
        public string ReferenceNo { get; set; }
        public int ResvSpaceId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string SpaceUnreserved { get; set; }
        public string SpaceReserved { get; set; } 
        public decimal Weight { get; set; } 
        public int Units { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal UnloadedUnit { get; set; }
        public decimal BalancedUnit { get; set; }
        public decimal UnloadedWeight { get; set; }
        public decimal BalancedWeight { get; set; }
        public string ReservationTo { get; set; }

    }
}