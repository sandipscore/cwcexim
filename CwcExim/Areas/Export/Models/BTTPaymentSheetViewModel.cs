using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class BTTPaymentSheetViewModel
    {
        public string InvoiceNo { get; set; }
        public string ModuleName { get; set; }
        public string PartyName { get; set; }
        public string InvoiceDate { get; set; }
    }
}