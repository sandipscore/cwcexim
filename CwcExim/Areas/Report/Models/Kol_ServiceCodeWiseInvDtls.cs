using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class Kol_ServiceCodeWiseInvDtls
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        [Display(Name = "Service Accounting Number")]
        public string SAC { get; set; }

        public string SACId { get; set; }
        public string Values { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal TotalValue { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string IssueDate { get; set; }

    }
}