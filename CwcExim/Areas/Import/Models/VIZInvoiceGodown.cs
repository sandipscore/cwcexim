using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class VIZInvoiceGodown : VIZInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<VIZPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VIZPreInvoiceContainer>();
        public List<VIZPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VIZPostPaymentContainer>();
        public List<VIZPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VIZPostPaymentChrg>();
        public IList<VIZContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VIZContainerWiseAmount>();

        public List<VIZOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VIZOperationCFSCodeWiseAmount>();
        public List<VIZDeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VIZDeliPostPaymentChargebreakupdate>();

        public List<VIZInvoiceCargo> lstInvoiceCargo { get; set; } = new List<VIZInvoiceCargo>();

        //--------------------------------------------------------------------------------------------------------------------
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string OldLocationIds { get; set; }
        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
    }
    public class VIZInvoiceCargo : PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
        public string MultiGodownId { get; set; } = string.Empty;
    }


    public class VIZPostPaymentContainerGodown : VIZPostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VIZPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VIZContainerWiseAmountGodown : VIZContainerWiseAmount
    {

    }

    public class VIZPreInvoiceContainerGodown : VIZPreInvoiceContainer
    {

    }

    public class VIZtentativeinvoice
    {

        public static VIZInvoiceGodown InvoiceObj;
    }
    public class VIZOperationCFSCodeWiseAmountGodown
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


    public class VIZDeliPostPaymentChargebreakupdate
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