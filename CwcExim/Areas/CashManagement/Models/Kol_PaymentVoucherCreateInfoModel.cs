using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Kol_PaymentVoucherCreateInfoModel
    {
        public string VoucherId { get; set; }
        public string UserGST { get; set; }
        public IList<Expenses> Expenses { get; set; } = new List<Expenses>();
        public IList<ExpHSN> ExpHSN { get; set; } = new List<ExpHSN>();
        public IList<HSN> HSN { get; set; } = new List<HSN>();
        public IList<Party> Party { get; set; } = new List<Party>();
    }

    public class Expenses
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

    public class ExpHSN
    {
        public string HSNCode { get; set; }
        public string ExpCode { get; set; }
    }

    public class HSN
    {
        public int HSNId { get; set; }        
        public string HSNCode { get; set; }
        public decimal GST { get; set; }
    }

    public class Party
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }

    public class KolVoucherHead
    {
        public int ReceiptId { get; set; }
        public string VoucherNo { get; set; }
        public int ExpenseId { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}