using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PpgInvoiceFCLToLCLConversion : PpgInvoiceBase
    {
        public List<PpgInvoiceContainerFCLToLCLConversion> LstContainersFCLToLCLConversion { get; set; } = new List<PpgInvoiceContainerFCLToLCLConversion>();
        public List<PpgInvoiceChargeFCLToLCLConversion> LstChargesFCLToLCLConversion { get; set; } = new List<PpgInvoiceChargeFCLToLCLConversion>();
    }

    public class PpgInvoiceContainerFCLToLCLConversion : PpgInvoiceContainerBase
    {
    }
    public class PpgInvoiceChargeFCLToLCLConversion : PpgInvoiceChargeBase
    {    
    }
}