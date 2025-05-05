using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_InsuranceRegister
    {
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string SVDate { get; set;  }
        public List<WFLD_InsuranceRegister> LstIns { get; set; } = new List<WFLD_InsuranceRegister>();
    }
}