using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class KOL_CWCChargeModel
    {
        //Clause, ChargeType, ChargeName, Quantity, Rate, Amount, Discount, Taxable, SACCode
        public string Clause { get; set; }        
        public string SACCode { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxable { get; set; }
        public decimal Gst { get; set; }
    }
}