using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class InvoiceReportDetails
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }

        public string SAC { get; set; }
        public string Values { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }

        public string IGST { get; set; }
        public string TotalValue { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string IssueDate { get; set; }

        public int Registered { get; set; }
        public int UnRegistered { get; set; }
        public int All { get; set; }



    }

    public class InvoiceReportDetailsExcel
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string SAC { get; set; }
        public decimal Values { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TotalValue { get; set; }        

    }
}