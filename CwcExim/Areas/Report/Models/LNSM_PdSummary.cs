using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_PdSummary
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }        
        public string PartyName { get; set; }
        public string Amount { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }

    }

}