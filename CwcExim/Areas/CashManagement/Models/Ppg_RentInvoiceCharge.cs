using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Ppg_RentInvoiceCharge
    {
        public string ChargeHead { get; set; }

        public string ChargeName { get; set; }
        public string Date { get; set; }
        public string amount { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        public string total { get; set; }
        public string round_up { get; set; }
        public string GSTNo { get; set; }

    }
}