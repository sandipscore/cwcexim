using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_PaymentVoucherCreateInfoModel
    {       
            public string VoucherId { get; set; }
            public string UserGST { get; set; }
            public IList<DSRExpenses> Expenses { get; set; } = new List<DSRExpenses>();
            public IList<DSRExpHSN> ExpHSN { get; set; } = new List<DSRExpHSN>();
            public IList<DSRHSN> HSN { get; set; } = new List<DSRHSN>();
            public IList<DSRParty> Party { get; set; } = new List<DSRParty>();
            public IList<DSRParty> PartyDetails { get; set; } = new List<DSRParty>();
            public string GstStateCode { get; set; }
            public string StateName { get; set; }
            public string CompanyAddress { get; set; }
            public string CityName { get; set; }
            public string PanNo { get; set; }
    }

    public class DSRExpenses
    {
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
    }

    public class DSRExpHSN
    {
        public string HSNCode { get; set; }
        public string ExpCode { get; set; }
    }

    public class DSRHSN
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; }
        public decimal GST { get; set; }
    }

    public class DSRParty
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string PinCode { get; set; }
        public string StateCode { get; set; }
        public string Pan { get; set; }
        public string GSTNo { get; set; }
    }


}