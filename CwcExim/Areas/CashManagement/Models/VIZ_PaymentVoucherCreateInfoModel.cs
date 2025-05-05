using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class VIZ_PaymentVoucherCreateInfoModel
    {
       
            public string VoucherId { get; set; }
            public string UserGST { get; set; }
            public IList<VIZ_Expenses> Expenses { get; set; } = new List<VIZ_Expenses>();
            public IList<VIZ_ExpHSN> ExpHSN { get; set; } = new List<VIZ_ExpHSN>();
            public IList<VIZ_HSN> HSN { get; set; } = new List<VIZ_HSN>();
            public IList<VIZ_Party> Party { get; set; } = new List<VIZ_Party>();
     }

    public class VIZ_Expenses
    {
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }

        public decimal BalanceinHand { get; set; }

        public decimal? RefundAmount { get; set; }
        public int ReceiptId { get; set; }
        public string VoucherNo { get; set; }
        public int id { get; set; }
    }

    public class VIZ_ExpHSN
    {
        public string HSNCode { get; set; }
        public string ExpCode { get; set; }
    }

    public class VIZ_HSN
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; }
        public decimal GST { get; set; }
    }

    public class VIZ_Party
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string GSTNo { get; set; }
    }

    public class VIZ_VoucherHead
    {
        public int ReceiptId { get; set; }
        public string VoucherNo { get; set; }
        public int ExpenseId { get; set; }
        public decimal BalanceAmount { get; set; }
    }


}