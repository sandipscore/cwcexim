using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ImportStockValue
    {
        public string SVDate { get; set; }
        public decimal CIFValue { get; set; }
        public decimal GrossDuty { get; set; }
    }
}