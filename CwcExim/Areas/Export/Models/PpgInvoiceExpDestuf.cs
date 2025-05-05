using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PpgInvoiceExpDestuf: PpgInvoiceBase
    {
        public string ExportDestuffingNo { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<PpgPreInvoiceContainerExpDestuf> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainerExpDestuf>();
        public List<PpgPostPaymentContainerExpDestuf> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainerExpDestuf>();
        public List<PpgPostPaymentChrgExpDestuf> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrgExpDestuf>();
        public IList<PpgContainerWiseAmountExpDestuf> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmountExpDestuf>();
        public List<PpgOperationCFSCodeWiseAmountExpDestuf> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmountExpDestuf>();
        public List<PpgPreInvoiceCargoExpDestuf> lstPreInvoiceCargo { get; set; } = new List<PpgPreInvoiceCargoExpDestuf>();

        //--------------------------------------------------------------------------------------------------------------------
    }

    public class PpgPostPaymentContainerExpDestuf : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgExpDestuf : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmountExpDestuf : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainerExpDestuf : PreInvoiceContainer
    {

    }

    public class PpgOperationCFSCodeWiseAmountExpDestuf
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

    public class PpgPreInvoiceCargoExpDestuf : PreInvoiceCargo
    {
    }
}