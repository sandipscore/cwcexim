using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLDInvoiceFCLToLCLConversion : WFLDInvoiceBase
    {
        public List<WFLDInvoiceContainerFCLToLCLConversion> LstContainersFCLToLCLConversion { get; set; } = new List<WFLDInvoiceContainerFCLToLCLConversion>();
        public List<WFLDInvoiceChargeFCLToLCLConversion> LstChargesFCLToLCLConversion { get; set; } = new List<WFLDInvoiceChargeFCLToLCLConversion>();
    }

    public class WFLDInvoiceContainerFCLToLCLConversion : WFLDInvoiceContainerBase
    {
    }
    public class WFLDInvoiceChargeFCLToLCLConversion : WFLDInvoiceChargeBase
    {   
         
    }
}