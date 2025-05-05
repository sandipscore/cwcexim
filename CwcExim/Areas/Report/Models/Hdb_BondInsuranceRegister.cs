using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_BondInsuranceRegister
    {
        public string Date { get; set; }
        public decimal OCIFValue { get; set; }
        public decimal OGrossDuty { get; set; }
        public decimal RCIFValue { get; set; }
        public decimal RGrossDuty { get; set; }
        public decimal DCIFValue { get; set; }
        public decimal DGrossDuty { get; set; }
        public decimal CCIFValue { get; set; }
        public decimal CGrossDuty { get; set; }
        public decimal CTotal { get; set; }

    }
    public class Hdb_BondInsuranceType
    {
        public string ValueType { get; set; }
        public string ValueName { get; set; }       
    }
}