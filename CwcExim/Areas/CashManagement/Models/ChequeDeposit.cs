using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class ChequeDeposit
    {
        public int ChequeDepositId { get; set; }
        public string ChequeDepositNo { get; set; }
        public string EntryDate { get; set; }
        public List<ChequeDepositDetail> ChequeDetails { get; set; } = new List<ChequeDepositDetail>();
    }

    public class ChequeDepositDetail
    {
        public int ChequeDepositDtlId { get; set; }
        public string ChequeDate { get; set; }
        public int BankId { get; set; } = 0;
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string ChequeNo { get; set; }
        public string Mode { get; set; }
        public string ModeText { get; set; }
        public decimal Amount { get; set; } = 0;
    }

    public class ChequeDepositList
    {
        public int ChequeDepositId { get; set; }
        public string ChequeDepositNo { get; set; }
        public string EntryDate { get; set; }
        public string ChequeNos { get; set; }
    }
}