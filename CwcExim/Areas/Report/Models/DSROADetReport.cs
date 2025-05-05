using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSROADetReport
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }

    public class DSRPartyForOADet
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string PartyCode { get; set; }

    }
    
    public class DSROADtlStatement
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public List<DSROADtlStatementList> LstOnAccountDtl { get; set; } = new List<DSROADtlStatementList>();
    }
    public class DSROADtlStatementList
    {
        public string ReceivedDate { get; set; }
        public string Particular { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal ClosingBalance { get; set; }
    }
    
}