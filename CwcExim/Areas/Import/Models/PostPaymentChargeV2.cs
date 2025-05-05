using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PostPaymentChargeV2
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;
        public decimal Discount { get; set; } = 0M;
        public decimal Taxable { get; set; } = 0M;
        public decimal IGSTPer { get; set; } = 0M;
        public decimal IGSTAmt { get; set; } = 0M;
        public decimal CGSTPer { get; set; } = 0M;
        public decimal CGSTAmt { get; set; } = 0M;
        public decimal SGSTPer { get; set; } = 0M;
        public decimal SGSTAmt { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public decimal ActualFullCharge { get; set; } = 0M;
        public int OperationId { get; set; }
    }
}