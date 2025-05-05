using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class PPGInvoiceGodown : PpgInvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<PpgPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainer>();
        public List<PpgPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainer>();
        public List<PpgPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrg>();
        public IList<PpgContainerWiseAmount> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmount>();

        public List<PpgOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmount>();
        public List<ppgDeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgDeliPostPaymentChargebreakupdate>();

        public List<PpgInvoiceCargo> lstInvoiceCargo { get; set; } = new List<PpgInvoiceCargo>();
        //--------------------------------------------------------------------------------------------------------------------

        public string OldLocationIds { get; set; }

        public string PaymentMode { get; set; }

        public int BidId { get; set; }

        public string AssesmentType { get; set; }



        public string HSNCode { get; set; }

        public string GSTPer { get; set; }
        public string AssesmentDate { get; set; }

        public string FreeUpto { get; set; }


    }
    public class PpgInvoiceCargo:PreInvoiceCargo
    {

    }


    public class PpgPostPaymentContainerGodown : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmountGodown : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainerGodown : PreInvoiceContainer
    {

    }

    public class tentativeinvoice
    {

        public static PPGInvoiceGodown InvoiceObj;
    }
    public class PpgOperationCFSCodeWiseAmountGodown
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


    public class ppgDeliPostPaymentChargebreakupdate
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