using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_PartyWiseLedger: PartyWiseLedger
    {
        public string GSTNo { get; set; }
    }
    public class CHN_PartyLedgerList
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
    }
}