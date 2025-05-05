using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_PartyLedCons:PdSummary
    {
        public string CreditAmt { get; set; }
        public string DebitAmt { get; set; }
        public string SDAmount { get; set; }
        public string PartyCode { get; set; }

    }
}