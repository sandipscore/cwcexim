using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDOADetReport
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }

    public class WFLDPartyForOADet
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string PartyCode { get; set; }

    }
    
    public class WFLDOADtlStatement
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public List<WFLDOADtlStatementList> LstOnAccountDtl { get; set; } = new List<WFLDOADtlStatementList>();
    }
    public class WFLDOADtlStatementList
    {
        public string ReceivedDate { get; set; }
        public string Particular { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal ClosingBalance { get; set; }
    }

    
}