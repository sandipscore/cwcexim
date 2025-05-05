using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_DailyCashBook
    {
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }
        public string ChqNo { get; set; }
        public decimal GRE { get; set; }
        public decimal GRL { get; set; }
        public decimal Reefer { get; set; }
        public decimal INS { get; set; }
        public decimal STO { get; set; }
        public decimal EscCharge { get; set; }
        public decimal Print { get; set; }
        public decimal Royality { get; set; }
        public decimal Franchiese { get; set; }
        public decimal HT { get; set; }
        public decimal OpCash { get; set; }
        public decimal OpChq { get; set; }
        public decimal OTCharge { get; set; }
        public decimal TotalSD { get; set; }
        public decimal cloCash { get; set; }
        public decimal clochq { get; set; }
        public decimal EGM { get; set; }
        public decimal Documentation { get; set; }
        public decimal Taxable { get; set; }
        public decimal TCgst { get; set; }
        public decimal TSgst { get; set; }
        public decimal TIgst { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal Igst { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCheque { get; set; }
        public decimal TotalDay{ get; set; }
        public decimal TotalBank { get; set; }

        public decimal BankCash { get; set; }

        public decimal BankChq { get; set; }

        public decimal Tds { get; set; }
        public decimal CrTds { get; set; }
        public decimal Area { get; set; }

        public decimal Total { get; set; }
        public decimal Roundoff { get; set; } = 0;
        public decimal CstmExam { get; set; }
        public decimal Weighment { get; set; }
        public decimal CstmCl { get; set; }
        public decimal CBSC { get; set; }
        public decimal Other { get; set; }
        public decimal OT { get; set; }

        public decimal EPCCh { get; set; }
        public decimal TCGSTAmtSGSTAmt { get; set; }
        public decimal CGSTAmtSGSTAmt { get; set; }
        public decimal ReliazationagainstBilling { get; set; }
    }

    public class VRN_MonthlyCashBook
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public String StartRNo { get; set; }
        public String LastRNo { get; set; }
        public int TotalRNo { get; set; }
        public decimal OpCash { get; set; }
        public decimal OpChq { get; set; }
        public decimal TotalOp { get; set; }
        public decimal ReceiptCash { get; set; }
        public decimal ReceiptChq { get; set; }
        public decimal ReceiptBank { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal CashBank { get; set; }
        public decimal ChqBank { get; set; }
        public decimal TotalBank { get; set; }
        public decimal ClosingCash { get; set; }
        public decimal ClosingChq { get; set; }

        public decimal TotalClose { get; set; }
        public decimal ReliazationagainstBilling { get; set; }
        public IList<VRN_DailyCashBook> lstCashReceipt { get; set; } = new List<VRN_DailyCashBook>();
        
    }


   
}