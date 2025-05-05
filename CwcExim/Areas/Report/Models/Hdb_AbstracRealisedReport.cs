using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_AbstracRealisedReport
    {

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptNo { get; set; }
       


        public List<Hdb_AbstracRealisedReport> lstInv { get; set; } = new List<Hdb_AbstracRealisedReport>();

    }
}