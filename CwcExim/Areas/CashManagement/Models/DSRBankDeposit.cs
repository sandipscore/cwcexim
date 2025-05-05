using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSRBankDeposit
    {
        public int Id { get; set; }
        public string DepositDate { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal NEFT { get; set; }
        public decimal Draft { get; set; }
        public int BankId { get; set; }
        public string DepSlipNo { get; set; }
        public string AccountNo { get; set; }

        public List<DSRBankDetails> BankAccountList { get; set; } = new List<DSRBankDetails>();
        

    }

    public class DSRBankDetails
    {
        public int BankId { get; set; }
        public string AccountNo { get; set; }
    }
}