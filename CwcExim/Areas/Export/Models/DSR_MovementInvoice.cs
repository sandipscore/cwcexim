using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
namespace CwcExim.Areas.Export.Models
{
    public class DSR_MovementInvoice : DSR_InvoiceBase
    {
         public int hasSDAvailableBalance { get; set; }
            //--------------------------------------------------------------------------------------------------------------------
            public List<DSRPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainer>();
            public List<DSRPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainer>();
            public List<DSRPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrg>();
            public IList<DSRContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmount>();
        public List<DSRCMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRCMPostPaymentChargebreakupdate>();
        public List<DSROperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmount>();
            //--------------------------------------------------------------------------------------------------------------------
        }


        public class DSRPostPaymentContainerGodown : PostPaymentContainer 
        {
            public string DeliveryDate { get; set; }
        }

        public class DSRPostPaymentChrgGodown : PostPaymentCharge
        {
            public int OperationId { get; set; }
        }

        public class DSRContainerWiseAmountGodown : ContainerWiseAmount
        {

        }

        public class DSRPreInvoiceContainerGodown : PreInvoiceContainer
        {
       
    }
    public class DSRCMPostPaymentChargebreakupdate
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
    public class DSRTentativeInvoice
    {
        public static DSR_MovementInvoice InvoiceObjW;
        public static DSR_MovementInvoice InvoiceObjGR;
        public static DSR_MovementInvoice InvoiceObjFMC;
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
    }