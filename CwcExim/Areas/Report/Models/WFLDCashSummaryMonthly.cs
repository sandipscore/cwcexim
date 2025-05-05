using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDCashSummaryMonthly
    {
        public decimal OpeningCash { get; set; }
        public decimal OpeningChq { get; set; }

        public List<CashMonthlySum> MonthDetails { get; set; } = new List<CashMonthlySum>();


    }

    public class CashMonthlySum
    {
        public string ReceiptDate { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCheque { get; set; }
        public decimal TotalPOS { get; set; }
        public decimal Bank { get; set; }
        public decimal CashDeposit { get; set; }

        public decimal ChqDeposit { get; set; }
        public decimal BankDeposit { get; set; }
        public decimal POSDeposit { get; set; }

    }
}