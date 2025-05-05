using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
namespace CwcExim.Areas.Bond.Models
{
    public class WFLDBondPostPaymentSheet: BondPostPaymentSheet
    {
        public string Module { get; set; }
        public string ExportUnder { get; set; } = string.Empty;
        public int IsLocalGST { get; set; } = 0;
    }
}