using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_SDBalaceForEU
    {
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string BalAmount { get; set; }
        public string SDAmount { get; set; }
        public string CreditAmt { get; set; }
        public string DebitAmt { get; set; }  
        public string PeriodTo { get; set; }
    }
}