using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PayeeForPage
    {
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string PartyCode { get; set; }
    }
}