using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class WFLDInvoiceGodown : WFLDInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<WFLDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainer>();
        public List<WFLDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainer>();
        public List<WFLDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrg>();
        public IList<WFLDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmount>();

        public List<WFLDOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmount>();
        public List<WFLDDeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDDeliPostPaymentChargebreakupdate>();

        public List<WFLDInvoiceCargo> lstInvoiceCargo { get; set; } = new List<WFLDInvoiceCargo>();

        //--------------------------------------------------------------------------------------------------------------------
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string OldLocationIds { get; set; }
        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
    }
    public class WFLDInvoiceCargo :PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
        public string MultiGodownId { get; set; } = string.Empty;
    }


    public class WFLDPostPaymentContainerGodown : WFLDPostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLDPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WFLDContainerWiseAmountGodown : WFLDContainerWiseAmount
    {

    }

    public class WFLDPreInvoiceContainerGodown : WFLDPreInvoiceContainer
    {

    }

    public class WFLDtentativeinvoice
    {

        public static WFLDInvoiceGodown InvoiceObj;
    }
    public class WFLDOperationCFSCodeWiseAmountGodown
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


    public class WFLDDeliPostPaymentChargebreakupdate
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