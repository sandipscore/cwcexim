using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wlj_DailyCashBook
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
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal Igst { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCheque { get; set; }
        public decimal Tds { get; set; }
        public decimal CrTds { get; set; }
        public decimal Area { get; set; }
        public decimal Roundoff { get; set; } = 0;

    }

    public class Wlj_MonthlyCashBook
    {
       public String StartRNo { get; set; }
        public String LastRNo { get; set; }
        public int TotalRNo { get; set; }
        public decimal OpCash { get; set; }
        public decimal OpChq { get; set; }
        public decimal TotalOp { get; set; }
        public decimal ReceiptCash { get; set; }
        public decimal ReceiptChq { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal CashBank { get; set; }
        public decimal ChqBank { get; set; }
        public decimal TotalBank { get; set; }
        public decimal ClosingCash { get; set; }
        public decimal ClosingChq { get; set; }

        public decimal TotalClose { get; set; }
        public IList<Wlj_DailyCashBook> lstCashReceipt { get; set; } = new List<Wlj_DailyCashBook>();
        
    }


   
}