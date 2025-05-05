using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_SDReportForOnAccount
    {
        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }

        public string PartyName { get; set; }
        public int? PartyId { get; set; }

        public string PartyAddress { get; set; }

        public decimal OpeingBalance { get; set; }

      

    }

   


}