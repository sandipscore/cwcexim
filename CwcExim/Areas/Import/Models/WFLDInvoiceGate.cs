using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLDInvoiceGate:WFLDInvoiceBase
    {
        public List<WFLDInvoiceContainerGate> LstContainersGate { get; set; } = new List<WFLDInvoiceContainerGate>();
        public List<WFLDInvoiceChargeGate> LstChargesGate { get; set; } = new List<WFLDInvoiceChargeGate>();
    }

    public class WFLDInvoiceContainerGate : WFLDInvoiceContainerBase
    {
        public int CargoType { get; set; } = 2;

    }
    public class WFLDInvoiceChargeGate : WFLDInvoiceChargeBase
    {    
    }
}