using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
namespace CwcExim.Areas.Bond.Models
{
    public class DndBondPostPaymentSheet: BondPostPaymentSheet
    {
        public string Module { get; set; }
        public string ExportUnder { get; set; } = string.Empty;
    }

    public class DndListOfBondInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
    }
}