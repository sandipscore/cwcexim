using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
namespace CwcExim.Areas.Export.Models
{
    public class VIZ_LoadedContainerPayment: PostPaymentSheet
    {
        public string BOLNo { get; set; } = string.Empty;
        public string BOLDate { get; set; } = string.Empty;
        public int ForwarderId { get; set; } = 0;
        public string ExportUnder { get; set; } = string.Empty;
        public string Forwarder { get; set; } = string.Empty;
        public List<VIZ_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VIZ_PreInvoiceContainer>();
        public List<VIZ_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VIZ_OperationCFSCodeWiseAmount>();       
        public string LEODate { get; set; }
        public int POD { get; set; } = 0;
    }

    public class VIZ_OperationCFSCodeWiseAmount
    {
        public int InvoiceId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Clause { get; set; }
        public string BOLNo { get; set; }
    }
    public class VIZ_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public int ISODC { get; set; } = 0;
    }
}