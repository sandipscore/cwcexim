using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class LedgerCodeDetails
    {
        public int LedgerCodeId { get; set; }

        [Required]
        public string FinancialYear { get; set; }
        [Required]
        public string LedgerCode { get; set; }

        public int Uid { get; set; }
    }
}