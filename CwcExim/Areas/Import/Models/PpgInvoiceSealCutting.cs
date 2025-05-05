using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PpgInvoiceSealCutting : PpgInvoiceBase
    {
        public List<PpgInvoiceContainerSealCutting> LstContainersSealCutting { get; set; } = new List<PpgInvoiceContainerSealCutting>();
        public List<PpgInvoiceChargeSealCutting> LstChargesSealCutting { get; set; } = new List<PpgInvoiceChargeSealCutting>();
    }

    public class PpgInvoiceContainerSealCutting : PpgInvoiceContainerBase
    {
    }
    public class PpgInvoiceChargeSealCutting : PpgInvoiceChargeBase
    {    
    }
}