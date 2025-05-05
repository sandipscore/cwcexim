using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSRPartyWiseLedger
    {

        [Required(ErrorMessage ="Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        public int PartyId { get; set; }
    }

    public class DSRPartyWiseLedgerList
    {
        public string InvoiceNo { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptNo { get; set; }
        public decimal Amount { get; set; }
    }
    public class DSRPartyLedgerList
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }
}