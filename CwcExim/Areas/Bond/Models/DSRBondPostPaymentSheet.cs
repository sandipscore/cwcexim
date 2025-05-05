using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
namespace CwcExim.Areas.Bond.Models
{
    public class DSRBondPostPaymentSheet : BondPostPaymentSheet
    {
        public string Module { get; set; }
        public string ExportUnder { get; set; } = string.Empty;
        public int IsLocalGST { get; set; } = 0;
        public string InsUpToDate { get; set; } = string.Empty;
        public IList<DSRBONdHTClauseList> ActualApplicable { get; set; } = new List<DSRBONdHTClauseList>();
        public bool IsPartyStateInCompState { get; set; }
    }

    public class DSRBONDPostPaymentChrg : PostPaymentCharge
    {
        //public int OperationId { get; set; }
        public decimal HazAmt { get; set; } = 0M;
        public decimal OdcAmt { get; set; } = 0M;
        public decimal HazTotal { get; set; } = 0M;
        public decimal OdcTotal { get; set; } = 0M;
        public decimal HAZIGSTAmt { get; set; } = 0M;
        public decimal HAZCGSTAmt { get; set; } = 0M;
        public decimal HAZSGSTAmt { get; set; } = 0M;
    }
    public class DSRBONdHTClauseList
    {
        public string ClauseName { get; set; }
        public string ClauseId { get; set; }
    }
}