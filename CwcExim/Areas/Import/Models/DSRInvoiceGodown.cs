using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class DSRInvoiceGodown : DSRInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<DSRPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainer>();
        public List<DSRPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainer>();
        public List<DSRPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrg>();
        public IList<DSRContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmount>();

        public List<DSROperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmount>();
        public List<DSRDeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRDeliPostPaymentChargebreakupdate>();

        public List<DSRInvoiceCargo> lstInvoiceCargo { get; set; } = new List<DSRInvoiceCargo>();

        //--------------------------------------------------------------------------------------------------------------------
       // public IList<string> ActualApplicable { get; set; } = new List<string>();

        public IList<DSRHTClauseList> ActualApplicable { get; set; } = new List<DSRHTClauseList>();


        public string OldLocationIds { get; set; }
        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
        public string SEZ { get; set; }
    }
    public class DSRInvoiceCargo : PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
    }


    public class DSRPostPaymentContainerGodown : DSRPostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSRPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DSRContainerWiseAmountGodown : DSRContainerWiseAmount
    {

    }

    public class DSRPreInvoiceContainerGodown : DSRPreInvoiceContainer
    {

    }

    public class DSRtentativeinvoice
    {

        public static DSRInvoiceGodown InvoiceObj;
    }
    public class DSROperationCFSCodeWiseAmountGodown
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


    public class DSRDeliPostPaymentChargebreakupdate
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