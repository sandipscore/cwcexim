using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRInvoiceGate : DSRInvoiceBase
    {
        public List<DSRInvoiceContainerGate> LstContainersGate { get; set; } = new List<DSRInvoiceContainerGate>();
        public List<DSRInvoiceChargeGate> LstChargesGate { get; set; } = new List<DSRInvoiceChargeGate>();
    }

    public class DSRInvoiceContainerGate : DSRInvoiceContainerBase
    {
        public int CargoType { get; set; } = 2;

    }
    public class DSRInvoiceChargeGate : DSRInvoiceChargeBase
    {    
    }
}