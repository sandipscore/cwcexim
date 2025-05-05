using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_InvoiceGodown: VRN_InvoiceBase
    {

        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<VRN_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VRN_PreInvoiceContainer>();
        public List<VRN_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VRN_PostPaymentContainer>();
        public List<VRN_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VRN_PostPaymentChrg>();
        public IList<VRN_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VRN_ContainerWiseAmount>();

        public List<VRN_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VRN_OperationCFSCodeWiseAmount>();
        public List<VRN_DeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VRN_DeliPostPaymentChargebreakupdate>();

        public List<VRN_InvoiceCargo> lstInvoiceCargo { get; set; } = new List<VRN_InvoiceCargo>();
        //--------------------------------------------------------------------------------------------------------------------

        public string OldLocationIds { get; set; }

        public string PaymentMode { get; set; }
    }
    public class VRN_InvoiceCargo : PreInvoiceCargo
    {

    }

    public class VRN_PostPaymentContainerGodown : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VRN_PostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VRN_ContainerWiseAmountGodown : ContainerWiseAmount
    {

    }

    public class VRN_PreInvoiceContainerGodown : PreInvoiceContainer
    {

    }

    public class VRN_tentativeinvoice
    {

        public static VRN_InvoiceGodown InvoiceObj;
    }
    public class VRN_OperationCFSCodeWiseAmountGodown
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


    public class VRN_DeliPostPaymentChargebreakupdate
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;

        public int OperationId { get; set; }
        public string CFSCode { get; set; }
        public string Startdate { get; set; }
        public string EndDate { get; set; }
    }
}
