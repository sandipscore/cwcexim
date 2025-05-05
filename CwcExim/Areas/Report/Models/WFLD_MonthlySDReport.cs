using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_MonthlySDReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }        
        public int SlNo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public decimal Amount { get; set; }
        public string PartyName { get; set; }
        public string Remarks { get; set; }
        
    }
}