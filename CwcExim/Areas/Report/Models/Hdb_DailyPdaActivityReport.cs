using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_DailyPdaActivityReport
    {
       // public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string ReceiptNo { get; set; }
        public string Party { get; set; }
       // public string GSTNo { get; set; }

       
        public string Deposit { get; set; }
        public string Withdraw { get; set; }
      

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
       


    }
}