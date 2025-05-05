using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VIZ_OutstandingSummary
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }        
        
     
    }
    public class VIZ_RptOutstandingSummary
    {
        public int SlNo { get; set; }
        public string PartyName { get; set; }
        public string AvailableBalAmt { get; set; }
        public string OutstandingAmt { get; set; }
        public string AmtOfTDSCert { get; set; }
    }
}