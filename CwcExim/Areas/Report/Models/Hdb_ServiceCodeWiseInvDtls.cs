using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_ServiceCodeWiseInvDtls
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        [Display(Name = "Service Accounting Number")]
        public string SAC { get; set; }

        public string SACId { get; set; }
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


    }

    public class Hdb_SACList
    {
        public string SAC { get; set; }
    }
}