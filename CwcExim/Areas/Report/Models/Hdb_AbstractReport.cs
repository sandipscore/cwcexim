using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_AbstractReport
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public string GivenMnthFirstDate { get; set; }
        public string CurrMonthlastDate { get; set; }
        public int PrevTotalUnpaidInvoice { get; set; }
        public decimal PrevTotalUnpaidAmt { get; set; }
        public int CurrMnthTotalInvoice { get; set; }
        public decimal CurrMnthTotalInvoiceAmt { get; set; }

        public int TotalOutstandingInv { get; set; }
        public decimal TotalOutstandingAmt { get; set; }

        public int TotalPaidPreInv { get; set; }
        public decimal TotalPrevinvoicePaidAmt { get; set; }

        public int TotalPaidCurrMnthInv { get; set; }
        public decimal TotalPaidCurrMnthInvAmt { get; set; }

        public int TotalPaidInv { get; set; }
        public decimal TotalIncome { get; set; }

        public int PrevPendingInv { get; set; }
        public decimal PrevPendingInvAmt { get; set; }

        public int CurrMnthPendingInv { get; set; }
        public decimal CurrMnthPendingAmt { get; set; }

        public int TotalPendingInv { get; set; }
        public decimal TotalPendingAmt { get; set; }

        public int CurrMnthTotalCrNote { get; set; }
        public decimal CurrMnthTotalCrAmt { get; set; }

        public int TotalBalancedInv { get; set; }
        public decimal TotalBalancedAmt { get; set; }




    }
}