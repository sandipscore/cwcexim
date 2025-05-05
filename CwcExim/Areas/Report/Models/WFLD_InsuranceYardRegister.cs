using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_InsuranceYardRegister
    {
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
      
        public string Date { get; set; }
        public string size { get; set; }
        public string Module { get; set; }
        //public List<WFLD_InsuranceYardRegister> LstYard { get; set; } = new List<WFLD_InsuranceYardRegister>();
    }
}