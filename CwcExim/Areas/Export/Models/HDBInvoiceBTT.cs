using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class HDBInvoiceBTT : Hdb_InvoiceBase
    {

        public string ExportUnder { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<HDBPreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<HDBPreInvoiceContainerBTT>();
        public List<HDBPostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<HDBPostPaymentContainerBTT>();
        public List<HDBPostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<HDBPostPaymentChrgBTT>();
        public IList<HDBContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<HDBContainerWiseAmountBTT>();
        public List<HDBOperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<HDBOperationCFSCodeWiseAmountBTT>();
        public List<HDBPreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<HDBPreInvoiceCargoBTT>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();

        //--------------------------------------------------------------------------------------------------------------------
    }
    public class HDBPostPaymentContainerBTT: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class HDBPostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class HDBContainerWiseAmountBTT : ContainerWiseAmount
    {

    }

    public class HDBPreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class HDBOperationCFSCodeWiseAmountBTT
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
        
    }

    public class HDBPreInvoiceCargoBTT : PreInvoiceCargo {
    }
}