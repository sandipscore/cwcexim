using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class VRN_ChargeNameCrDb
    {
        public int Sr { get; set; }
        public string ChargeName { get; set; }
        public string Clause { get; set; }
        public string SACCode { get; set; }
        public string ChargeType { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public int IsLocalGST { get; set; }
    }
}