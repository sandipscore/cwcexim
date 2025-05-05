using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Wfld_invoiceSealCutting: PpgInvoiceBase
    {
        public List<Wfld_InvoiceContainerSealCutting> LstContainersSealCutting { get; set; } = new List<Wfld_InvoiceContainerSealCutting>();
        public List<Wfld_InvoiceChargeSealCutting> LstChargesSealCutting { get; set; } = new List<Wfld_InvoiceChargeSealCutting>();
    }

    public class Wfld_InvoiceContainerSealCutting 
    {
        public string CfsCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string CargoType { get; set; }
    }
    public class Wfld_InvoiceChargeSealCutting 
    {
        public string ChargeSD { get; set; }
        public string ChargeDesc { get; set; }
        public string HsnCode { get; set; }

        public decimal Rate { get; set; }

        public decimal TaxableAmt { get; set; }

        public decimal CGSTRate { get; set; }
        public decimal CGSTAmt { get; set; }

        public decimal SGSTRate { get; set; }
        public decimal SGSTAmt { get; set; }

        public decimal IGSTRate { get; set; }
        public decimal IGSTAmt { get; set; }

        public decimal Total { get; set; }
    }
}
