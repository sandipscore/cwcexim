using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class DSR_DailyInvReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
     
    }
    public class DSR_DailyReport
    {
        public string InvoiceNo { get; set; }
        public string Party { get; set; }
        public decimal TotalAmt { get; set; }
        public string ReceiptNo { get; set; }
        public decimal ReceiptAmt { get; set; }

    }
}