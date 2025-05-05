using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRInvoiceFCLToLCLConversion : DSRInvoiceBase
    {
        public List<DSRInvoiceContainerFCLToLCLConversion> LstContainersFCLToLCLConversion { get; set; } = new List<DSRInvoiceContainerFCLToLCLConversion>();
        public List<DSRInvoiceChargeFCLToLCLConversion> LstChargesFCLToLCLConversion { get; set; } = new List<DSRInvoiceChargeFCLToLCLConversion>();
    }

    public class DSRInvoiceContainerFCLToLCLConversion : DSRInvoiceContainerBase
    {
    }
    public class DSRInvoiceChargeFCLToLCLConversion : DSRInvoiceChargeBase
    {   
         
    }
}