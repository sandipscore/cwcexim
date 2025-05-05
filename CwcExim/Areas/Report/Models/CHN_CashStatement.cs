using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_CashStatement
    {
        public string CompanyAddress { get; set; }
        public List<CHN_CashStatementDetail> LstCash { get; set; } = new List<CHN_CashStatementDetail>();

        public List<CHN_ChequeStatemnt> LstChq { get; set; } = new List<CHN_ChequeStatemnt>();

    }
    public class CHN_CashStatementDetail
    {
        public string ReceiptDate { get; set; }

        public string Shift { get; set; }

        public int TotalCR { get; set; }
        public int UserId { get; set; }
        public string StartCR { get; set; }
        public string EndCR { get; set; }
        public string UserName { get; set; }
        public decimal CashAmt { get; set; }
        public decimal ChequeAmt { get; set; }
        public decimal NEFTAmt { get; set; }


    }


    public class CHN_ChequeStatemnt
    {
        public int UserId { get; set; }

        public string ReceiptDate { get; set; }
        public string ChequeDate { get; set; }
        public string ReceiptNo { get; set; }
        public decimal ChequeAmt { get; set; }
        public string ChequeNo { get; set; }
    }

    public class Chn_UserDetails
    {
        public int UId { get; set; }
        public string UserName { get; set; }
    }
}