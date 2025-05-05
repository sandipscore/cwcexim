using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PpgExpenseDetails
    {
        public int ExpId { get; set; }
        public int HsnId { get; set; }
        public decimal Taxable { get; set; }
        public decimal Amount { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public string ExpenseHead { get; set; }
        public string Expensecode { get; set; } = string.Empty;
    }
    public class PpgVoucherHead
    {
        public int ReceiptId { get; set; }
        public string VoucherNo { get; set; }
        public int ExpenseId { get; set; }
        public decimal BalanceAmount { get; set; }

    }    
}