using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PpgBankDeposit
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
        public List<PpgBankDetails> BankAccountList { get; set; } = new List<PpgBankDetails>();

        public List<PpgExpenses> ExpensesDetails { get; set; } = new List<PpgExpenses>();
        public int Idd { get; set; }
    }

    public class PpgBankDetails
    {
        public int BankId { get; set; }
        public string AccountNo { get; set; }
    }
    public class PpgExpenses
    {
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }

        public decimal BalanceinHand { get; set; }

        public decimal? RefundAmount { get; set; }
        public int? ReceiptId { get; set; }
        public string VoucherNo { get; set; }
        public int? id { get; set; }
    }
}