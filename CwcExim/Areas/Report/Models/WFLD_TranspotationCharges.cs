using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_TranspotationCharges
    {
        public int SLNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ContainerNo { get; set; }
        public string PartyName { get; set; }
        public string Amount { get; set; }
    }
}