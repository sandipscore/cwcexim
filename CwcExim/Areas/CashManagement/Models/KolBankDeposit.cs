using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class KolBankDeposit
    {
        public int Id { get; set; }
        public string DepositDate { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal NEFT { get; set; }
        public decimal Draft { get; set; }
        public int BankId { get; set; }
        public string DepSlipNo { get; set; }
        public string ReceivedDate { get; set; }
        public List<KolBankDetails> BankAccountList { get; set; } = new List<KolBankDetails>();

        public List<Expenses> ExpensesDetails { get; set; } = new List<Expenses>();
        public int Idd { get; set; }
    }

    public class KolBankDetails
    {
        public int BankId { get; set; }
        public string AccountNo { get; set; }
    }
}