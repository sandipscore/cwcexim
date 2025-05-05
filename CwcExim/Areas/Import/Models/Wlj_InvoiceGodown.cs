using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class Wlj_InvoiceGodown: Wlj_InvoiceBase
    {

        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<WLJPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WLJPreInvoiceContainer>();
        public List<WLJPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WLJPostPaymentContainer>();
        public List<WLJPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WLJPostPaymentChrg>();
        public IList<WLJContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WLJContainerWiseAmount>();

        public List<WLJOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WLJOperationCFSCodeWiseAmount>();
        public List<Wlj_DeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<Wlj_DeliPostPaymentChargebreakupdate>();

        public List<Wlj_InvoiceCargo> lstInvoiceCargo { get; set; } = new List<Wlj_InvoiceCargo>();
        //--------------------------------------------------------------------------------------------------------------------

        public string OldLocationIds { get; set; }

        public string PaymentMode { get; set; }
    }
    public class Wlj_InvoiceCargo : PreInvoiceCargo
    {

    }


    public class Wlj_PostPaymentContainerGodown : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class Wlj_PostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class Wlj_ContainerWiseAmountGodown : ContainerWiseAmount
    {

    }

    public class Wlj_PreInvoiceContainerGodown : PreInvoiceContainer
    {

    }

    public class Wlj_tentativeinvoice
    {

        public static Wlj_InvoiceGodown InvoiceObj;
    }
    public class Wlj_OperationCFSCodeWiseAmountGodown
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


    public class Wlj_DeliPostPaymentChargebreakupdate
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
