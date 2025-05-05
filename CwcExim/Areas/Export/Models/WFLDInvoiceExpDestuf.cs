using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDInvoiceExpDestuf: WFLDInvoiceBase
    {
        public string ExportDestuffingNo { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<WFLDPreInvoiceContainerExpDestuf> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainerExpDestuf>();
        public List<WFLDPostPaymentContainerExpDestuf> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainerExpDestuf>();
        public List<WFLDPostPaymentChrgExpDestuf> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrgExpDestuf>();
        public IList<WFLDContainerWiseAmountExpDestuf> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmountExpDestuf>();
        public List<WFLDOperationCFSCodeWiseAmountExpDestuf> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmountExpDestuf>();
        public List<WFLDPreInvoiceCargoExpDestuf> lstPreInvoiceCargo { get; set; } = new List<WFLDPreInvoiceCargoExpDestuf>();

        //--------------------------------------------------------------------------------------------------------------------
    }

    public class WFLDPostPaymentContainerExpDestuf : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLDPostPaymentChrgExpDestuf : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WFLDContainerWiseAmountExpDestuf : ContainerWiseAmount
    {

    }

    public class WFLDPreInvoiceContainerExpDestuf : PreInvoiceContainer
    {

    }

    public class WFLDOperationCFSCodeWiseAmountExpDestuf
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

    public class WFLDPreInvoiceCargoExpDestuf : PreInvoiceCargo
    {
    }
}