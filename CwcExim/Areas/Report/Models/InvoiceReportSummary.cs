using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class InvoiceReportSummary
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }

        public string Amount { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
       

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string IssueDate { get; set; }

        public int Registered { get; set; }
        public int UnRegistered { get; set; }
        public int All { get; set; }



    }
}