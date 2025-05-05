using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PpgInvoiceSealCuttingV2: PpgInvoiceBaseV2
    {
        public List<PpgInvoiceContainerSealCuttingV2> LstContainersSealCutting { get; set; } = new List<PpgInvoiceContainerSealCuttingV2>();
        public List<PpgInvoiceChargeSealCuttingV2> LstChargesSealCutting { get; set; } = new List<PpgInvoiceChargeSealCuttingV2>();
    }
    public class PpgInvoiceContainerSealCuttingV2 : PpgInvoiceContainerBaseV2
    {
    }
    public class PpgInvoiceChargeSealCuttingV2 : PpgInvoiceChargeBaseV2
    {
    }
}