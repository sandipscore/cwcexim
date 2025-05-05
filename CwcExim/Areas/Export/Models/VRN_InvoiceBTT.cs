using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_InvoiceBTT:VRN_InvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<VRN_PreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<VRN_PreInvoiceContainerBTT>();
        public List<VRN_PostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<VRN_PostPaymentContainerBTT>();
        public List<VRN_PostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<VRN_PostPaymentChrgBTT>();
        public IList<VRN_ContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<VRN_ContainerWiseAmountBTT>();
        public List<VRN_OperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<VRN_OperationCFSCodeWiseAmountBTT>();
        public List<VRN_PreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<VRN_PreInvoiceCargoBTT>();
        public string ExportUnder { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class VRN_PostPaymentContainerBTT : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VRN_PostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VRN_ContainerWiseAmountBTT : ContainerWiseAmount
    {
        public decimal CargoHandlingCharge { get; set; } = 0M;
    }

    public class VRN_PreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class VRN_OperationCFSCodeWiseAmountBTT
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
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Clause { get; set; }

    }


    public class VRN_PreInvoiceCargoBTT : PreInvoiceCargo
    {
    }
}
