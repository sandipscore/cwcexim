using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_BttWorkSlip
    {
        public int SLNO { get; set; }
        public string  Date { get; set; }
        public string ShippingBillNo { get; set; }

        public string BttDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
    }
}