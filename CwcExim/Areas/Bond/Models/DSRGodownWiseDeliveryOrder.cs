using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRGodownWiseDeliveryOrder
    {
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        public decimal Units { get; set; }
        public decimal Weights { get; set; }
        public decimal ClosingUnits { get; set; }
        public decimal ClosingAreas { get; set; }
        public decimal ClosingReservedArea { get; set; }
        public decimal ClosingWeight { get; set; }
        public string ReservedUpto { get; set; }

    }
}