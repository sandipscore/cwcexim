using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLDBankDeposit
    {
        public int Id { get; set; }
        public int Idd { get; set; }
        public string DepositDate { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal NEFT { get; set; }
        public decimal Draft { get; set; }
        public int BankId { get; set; }        
        public string DepSlipNo { get; set; }
        public string ReceivedDate { get; set; }
        public List<WFLDBankDetails> BankAccountList { get; set; } = new List<WFLDBankDetails>();

        public List<WFLDExpenses> ExpensesDetails { get; set; } = new List<WFLDExpenses>();
        //public List<WFLDBankDetails> BankAccountList { get; set; }

    }

    public class WFLDBankDetails
    {
        public int BankId { get; set; }
        public string AccountNo { get; set; }
    }
}