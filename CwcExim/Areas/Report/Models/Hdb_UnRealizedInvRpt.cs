using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_UnRealizedInvRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }

        public int PartyId { get; set; } = 0;

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Partyname { get; set; }
        public string InvoiceType { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }


    public class Hdb_UnRealizedInvParty
    {


        public int PartyId { get; set; }
        public string Partyname { get; set; }
    }
}