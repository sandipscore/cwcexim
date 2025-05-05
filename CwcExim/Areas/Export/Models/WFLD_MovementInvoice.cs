using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
namespace CwcExim.Areas.Export.Models
{
    public class WFLD_MovementInvoice : WFLD_InvoiceBase
    {
         public int hasSDAvailableBalance { get; set; }
            //--------------------------------------------------------------------------------------------------------------------
            public List<WFLDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainer>();
            public List<WFLDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainer>();
            public List<WFLDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrg>();
            public IList<WFLDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmount>();
        public List<WFLDCMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDCMPostPaymentChargebreakupdate>();
        public List<WFLDOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmount>();
            //--------------------------------------------------------------------------------------------------------------------
        }


        public class WFLDPostPaymentContainerGodown : PostPaymentContainer 
        {
            public string DeliveryDate { get; set; }
        }
        public class WFLDPostPaymentChrgGodown : PostPaymentCharge
        {
            public int OperationId { get; set; }
        }

        public class WFLDContainerWiseAmountGodown : ContainerWiseAmount
        {

        }

        public class WFLDPreInvoiceContainerGodown : PreInvoiceContainer
        {
       
    }
    public class WFLDCMPostPaymentChargebreakupdate
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
    public class WFLDTentativeInvoice
    {
        public static WFLD_MovementInvoice InvoiceObjW;
        public static WFLD_MovementInvoice InvoiceObjGR;
        public static WFLD_MovementInvoice InvoiceObjFMC;
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
    }