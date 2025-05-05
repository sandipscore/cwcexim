using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_PayerStatement
    {
        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }

        [Required]
        public string PartyName { get; set; }
        public int? PartyId { get; set; }

        public string PartyAddress { get; set; }

        public decimal OpeingBalance { get; set; }

    }
}