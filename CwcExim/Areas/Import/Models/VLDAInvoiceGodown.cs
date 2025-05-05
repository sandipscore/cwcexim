using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class VLDAInvoiceGodown : VLDAInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<VLDAPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VLDAPreInvoiceContainer>();
        public List<VLDAPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VLDAPostPaymentContainer>();
        public List<VLDAPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VLDAPostPaymentChrg>();
        public IList<VLDAContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VLDAContainerWiseAmount>();

        public List<VLDAOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VLDAOperationCFSCodeWiseAmount>();
        public List<VLDADeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VLDADeliPostPaymentChargebreakupdate>();

        public List<VLDAInvoiceCargo> lstInvoiceCargo { get; set; } = new List<VLDAInvoiceCargo>();

        //--------------------------------------------------------------------------------------------------------------------
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string OldLocationIds { get; set; }
        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
    }
    public class VLDAInvoiceCargo : PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
        public string MultiGodownId { get; set; } = string.Empty;
    }


    public class VLDAPostPaymentContainerGodown : VLDAPostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VLDAPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VLDAContainerWiseAmountGodown : VLDAContainerWiseAmount
    {

    }

    public class VLDAPreInvoiceContainerGodown : VLDAPreInvoiceContainer
    {

    }

    public class VLDAtentativeinvoice
    {

        public static VLDAInvoiceGodown InvoiceObj;
    }
    public class VLDAOperationCFSCodeWiseAmountGodown
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


    public class VLDADeliPostPaymentChargebreakupdate
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