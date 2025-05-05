using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Dnd_PartyDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PartyCode { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }
}