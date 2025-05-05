using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class VRN_CreditNoteDetails
    {
        public int CreditNoteId { get; set; }
        public string CreditNoteNo { get; set; }
        public decimal Amount { get; set; }
    }
}