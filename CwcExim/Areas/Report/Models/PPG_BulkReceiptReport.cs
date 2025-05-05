using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_BulkReceiptReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string ReceiptNumber { get; set; }
    }
    public class PPG_ReceiptList
    {
        public string ReceiptNumber { get; set; }
    }
}