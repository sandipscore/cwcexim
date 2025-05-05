using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Ppg_PaymentVoucherInfo
    {
        public string VoucherId { get; set; }
        public string UserGST { get; set; }
        public IList<Ppg_Expenses> Expenses { get; set; } = new List<Ppg_Expenses>();
        public IList<Ppg_ExpHSN> ExpHSN { get; set; } = new List<Ppg_ExpHSN>();
        public IList<Ppg_HSN> HSN { get; set; } = new List<Ppg_HSN>();
        public IList<Ppg_Party> Party { get; set; } = new List<Ppg_Party>();
    }

    public class Ppg_Expenses
    {
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
    }

    public class Ppg_ExpHSN
    {
        public string HSNCode { get; set; }
        public string ExpCode { get; set; }
    }

    public class Ppg_HSN
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; }
        public decimal GST { get; set; }
    }

    public class Ppg_Party
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }

}
