using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class DNDInvoiceGodown : DNDInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<DNDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DNDPreInvoiceContainer>();
        public List<DNDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DNDPostPaymentContainer>();
        public List<DNDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DNDPostPaymentChrg>();
        public IList<DNDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DNDContainerWiseAmount>();

        public List<DNDOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DNDOperationCFSCodeWiseAmount>();
        public List<DNDDeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DNDDeliPostPaymentChargebreakupdate>();

        public List<DNDInvoiceCargo> lstInvoiceCargo { get; set; } = new List<DNDInvoiceCargo>();
        public List<string> ActualApplicable { get; set; } = new List<string>();
        public bool IsPartyStateInCompState { get; set; }
        //--------------------------------------------------------------------------------------------------------------------

        public string OldLocationIds { get; set; }

        public string PaymentMode { get; set; }
        public decimal SDBalance { get; set; } = 0M;

    }
    public class DNDInvoiceCargo:PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
    }


    public class DNDPostPaymentContainerGodown : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DNDPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DNDContainerWiseAmountGodown : ContainerWiseAmount
    {

    }

    public class DNDPreInvoiceContainerGodown : PreInvoiceContainer
    {

    }

    public class DNDtentativeinvoice
    {

        public static DNDInvoiceGodown InvoiceObj;
    }
    public class DNDOperationCFSCodeWiseAmountGodown
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


    public class DNDDeliPostPaymentChargebreakupdate
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