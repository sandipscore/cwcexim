using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PpgInvoiceBTT:PpgInvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<PpgPreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainerBTT>();
        public List<PpgPostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainerBTT>();
        public List<PpgPostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrgBTT>();
        public IList<PpgContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmountBTT>();
        public List<PpgOperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmountBTT>();
        public List<PpgPreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<PpgPreInvoiceCargoBTT>();
        public List<ppgBTTPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgBTTPostPaymentChargebreakupdate>();
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class PpgPostPaymentContainerBTT: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmountBTT : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class PpgOperationCFSCodeWiseAmountBTT
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

    public class PpgPreInvoiceCargoBTT:PreInvoiceCargo {
    }



    public class ppgBTTPostPaymentChargebreakupdate
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