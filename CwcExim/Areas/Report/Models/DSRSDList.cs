using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRSDList
    {
        public string PartyName { get; set; }
        public string SDAmount { get; set; }
        public string UnpaidAmount { get; set; }
        public string BalanceAmount { get; set; }

        public string DepositorName { get; set; }
        public decimal Amount { get; set; } = 0M;

        public decimal AdjustAmount { get; set; }
        public decimal UtilizationAmount { get; set; }
        public decimal RefundAmount { get; set; }
    }
}