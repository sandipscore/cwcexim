using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wlj_GSTMISReport
    {
        public List<Wlj_CASHPDReport> lstCash { get; set; } = new List<Wlj_CASHPDReport>();
        public List<Wlj_CASHPDReport> lstSD { get; set; } = new List<Wlj_CASHPDReport>();

    }
    public class Wlj_CASHPDReport
    {
        public string ChargeType { get; set; } 
        public string ChargeName { get; set; }
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
    }
}