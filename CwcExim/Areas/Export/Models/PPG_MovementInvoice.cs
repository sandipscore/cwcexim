using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
namespace CwcExim.Areas.Export.Models
{
    public class PPG_MovementInvoice : PPG_InvoiceBase
    {
         public int hasSDAvailableBalance { get; set; }
            //--------------------------------------------------------------------------------------------------------------------
            public List<PpgPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainer>();
            public List<PpgPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainer>();
            public List<PpgPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrg>();
            public IList<PpgContainerWiseAmount> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmount>();
        public List<ppgCMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgCMPostPaymentChargebreakupdate>();
        public List<PpgOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmount>();
        //--------------------------------------------------------------------------------------------------------------------
        public List<ContainerStuffingDtlV2> LstStuffingDtl = new List<ContainerStuffingDtlV2>();
        public string PaymentMode { get; set; }
        public string ContainerNo { get; set; }
        public int ShippingId { get; set; }
        public int MoveToId { get; set; }
        public string MoveTo { get; set; }
        public string TransportMode { get; set; }
        public decimal TareWeight { get; set; }
        public decimal CargoWeight { get; set; }
        public int ContainerStuffingId { get; set; }
        public int GatewayPortId { get; set; }
        public int CargoType { get; set; }
        
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
    public class ppgCMPostPaymentChargebreakupdate
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
    public class PPGTentativeInvoice
    {
        public static PPG_MovementInvoice InvoiceObjW;
        public static PPG_MovementInvoice InvoiceObjGR;
        public static PPG_MovementInvoice InvoiceObjFMC;
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
    }