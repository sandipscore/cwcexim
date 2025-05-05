using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_GSTMISReport
    {
        public List<VRN_CASHPDReport> lstCash { get; set; } = new List<VRN_CASHPDReport>();
        public List<VRN_CASHPDReport> lstSD { get; set; } = new List<VRN_CASHPDReport>();

    }
    public class VRN_CASHPDReport
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