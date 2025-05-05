using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_AuctionInvoiceViewModel
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceModule { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string InvoiceModuleName { get; set; }
        public string PartyId { get; set; }
        public string All { get; set; }
    }
}