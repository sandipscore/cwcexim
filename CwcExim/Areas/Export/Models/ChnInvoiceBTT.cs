using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ChnInvoiceBTT:ChnInvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<ChnPreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<ChnPreInvoiceContainerBTT>();
        public List<ChnPostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<ChnPostPaymentContainerBTT>();
        public List<ChnPostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<ChnPostPaymentChrgBTT>();
        public IList<ChnContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<ChnContainerWiseAmountBTT>();
        public List<ChnOperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<ChnOperationCFSCodeWiseAmountBTT>();
        public List<ChnPreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<ChnPreInvoiceCargoBTT>();

        //--------------------------------------------------------------------------------------------------------------------
    }
    public class ChnPostPaymentContainerBTT: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class ChnPostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class ChnContainerWiseAmountBTT : ContainerWiseAmount
    {
        public decimal CargoHandlingCharge { get; set; } = 0M;
    }

    public class ChnPreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class ChnOperationCFSCodeWiseAmountBTT
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
    }

    public class ChnPreInvoiceCargoBTT:PreInvoiceCargo {
    }
}