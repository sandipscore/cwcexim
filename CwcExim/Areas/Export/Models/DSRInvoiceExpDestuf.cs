using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSRInvoiceExpDestuf : DSRInvoiceBase
    {
        public string ExportDestuffingNo { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<DSRPreInvoiceContainerExpDestuf> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainerExpDestuf>();
        public List<DSRPostPaymentContainerExpDestuf> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainerExpDestuf>();
        public List<DSRPostPaymentChrgExpDestuf> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrgExpDestuf>();
        public IList<DSRContainerWiseAmountExpDestuf> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmountExpDestuf>();
        public List<DSROperationCFSCodeWiseAmountExpDestuf> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmountExpDestuf>();
        public List<DSRPreInvoiceCargoExpDestuf> lstPreInvoiceCargo { get; set; } = new List<DSRPreInvoiceCargoExpDestuf>();

        //--------------------------------------------------------------------------------------------------------------------
    }

    public class DSRPostPaymentContainerExpDestuf : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSRPostPaymentChrgExpDestuf : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DSRContainerWiseAmountExpDestuf : ContainerWiseAmount
    {

    }

    public class DSRPreInvoiceContainerExpDestuf : PreInvoiceContainer
    {

    }

    public class DSROperationCFSCodeWiseAmountExpDestuf
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

    public class DSRPreInvoiceCargoExpDestuf : PreInvoiceCargo
    {
    }
}