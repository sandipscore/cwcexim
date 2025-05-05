using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kdl_ListOfImpInvoice
    {
        
            public int InvoiceId { get; set; }
            public string InvoiceNo { get; set; }
            public string InvoiceDate { get; set; }
            public string PartyName { get; set; }
            public string Module { get; set; }
            public string ModuleName { get; set; }
        
    }
}