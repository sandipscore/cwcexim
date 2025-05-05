using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PpgInvoiceGate:PpgInvoiceBase
    {
        public List<PpgInvoiceContainerGate> LstContainersGate { get; set; } = new List<PpgInvoiceContainerGate>();
        public List<PpgInvoiceChargeGate> LstChargesGate { get; set; } = new List<PpgInvoiceChargeGate>();
    }

    public class PpgInvoiceContainerGate:PpgInvoiceContainerBase
    {
        public int CargoType { get; set; } = 2;
    }
    public class PpgInvoiceChargeGate:PpgInvoiceChargeBase
    {    
    }
}