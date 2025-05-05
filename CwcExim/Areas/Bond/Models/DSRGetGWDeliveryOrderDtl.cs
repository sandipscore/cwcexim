using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRGetGWDeliveryOrderDtl
    {
        public int GodownId { get; set; }
        public string DepositNo { get; set; }
        public string GodownName { get; set; }
        public decimal Units { get; set; }
        public decimal ClosingUnits { get; set; }
        public decimal Weights { get; set; }
        public decimal ClosingWeight { get; set; }
        public decimal Area { get; set; }
        public decimal ClosingArea { get; set; }
        public decimal HndClosingArea { get; set; }
        public decimal HndClosingWeight { get; set; }
        public decimal HndClosingUnits { get; set; }

        public decimal ClosingValue { get; set; }
        public decimal Value { get; set; }
        public decimal ClosingDuty { get; set; }
        public decimal HndClosingValue  { get; set; }
        public decimal HndClosingDuty { get; set; }
        public decimal Duty { get; set; }
        public decimal ReservedArea { get; set; }
        public decimal ClosingReservedArea { get; set; }
        public decimal HndClosingReservedArea { get; set; }
        public string LocationName { get; set; }
        public string LocationId { get; set; }
    }
}