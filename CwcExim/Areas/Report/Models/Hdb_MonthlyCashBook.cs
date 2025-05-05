using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_MonthlyCashBook
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }


        //#for list
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }
        public string CwcChargeTAX { get; set; }

        public string CwcChargeNonTAX { get; set; }
        public string CustomRevenueTAX { get; set; }


        public string CustomRevenueNonTAX { get; set; }

        public string InsuranceTAX { get; set; }

        public string InsuranceNonTAX { get; set; }
        public string PortTAX { get; set; }
        public string PortNonTAX { get; set; }
        public string CWCCGST { get; set; }
        public string CWCSGST { get; set; }
        public string CWCISGT { get; set; }
        public string HtTax { get; set; }
        public string HtNonTax { get; set; }
        public string HtCGST { get; set; }
        public string HtSGST { get; set; }
        public string HtISGT { get; set; }
        public string MISC { get; set; }
        public string TDSPlus { get; set; }
        public string Exempted { get; set; }
        public string PdaPLus { get; set; }
        public string TDSMinus { get; set; }
        public string PdaMinus { get; set; }
        public string HtAdjust { get; set; }
        public string RowTotal { get; set; }
        //  public string RowCreditNote { get; set; }
        public string BankDeposit { get; set; }


        public string LWB { get; set; }
        public string RoundOff { get; set; }

        public string RoPdRefund { get; set; }
        public string Balance { get; set; }
        public string OpeningBalance { get; set; }
        public string ClosingBalance { get; set; }

        public string AddMoneySD { get; set; }

        public string withdralfromSD { get; set; }
    }
}