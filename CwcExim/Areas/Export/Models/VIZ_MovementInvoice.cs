using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
namespace CwcExim.Areas.Export.Models
{
    public class VIZ_MovementInvoice : VIZ_InvoiceBase
    {
         public int hasSDAvailableBalance { get; set; }
            //--------------------------------------------------------------------------------------------------------------------
            public List<VIZ_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VIZ_PreInvoiceContainer>();
            public List<VIZ_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VIZ_PostPaymentContainer>();
            public List<VIZ_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VIZ_PostPaymentChrg>();
            public IList<VIZ_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VIZ_ContainerWiseAmount>();
        public List<VIZ_CMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VIZ_CMPostPaymentChargebreakupdate>();
        public List<VIZ_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VIZ_OperationCFSCodeWiseAmount>();
            //--------------------------------------------------------------------------------------------------------------------
        }

    //public class VIZ_PreInvoiceContainer : PreInvoiceContainer
    //{

    //}
    public class VIZ_PostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VIZ_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VIZ_ContainerWiseAmount : ContainerWiseAmount
    {

    }
   
    public class VIZ_PostPaymentContainerGodown : PostPaymentContainer 
        {
            public string DeliveryDate { get; set; }
        }
        public class VIZ_PostPaymentChrgGodown : PostPaymentCharge
        {
            public int OperationId { get; set; }
        }

        public class VIZ_ContainerWiseAmountGodown : ContainerWiseAmount
        {

        }

        public class VIZ_PreInvoiceContainerGodown : PreInvoiceContainer
        {
       
        }
    //public class VIZ_OperationCFSCodeWiseAmount
    //{
    //    public int InvoiceId { get; set; }
    //    public string CFSCode { get; set; }
    //    public string ContainerNo { get; set; }
    //    public string Size { get; set; }
    //    public int OperationId { get; set; }
    //    public string ChargeType { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal Rate { get; set; }
    //    public decimal Amount { get; set; }
    //}
    public class VIZ_CMPostPaymentChargebreakupdate
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
    public class VIZ_TentativeInvoice
    {
        public static VIZ_MovementInvoice InvoiceObjW;
        public static VIZ_MovementInvoice InvoiceObjGR;
        public static VIZ_MovementInvoice InvoiceObjFMC;
    }



        public class VIZ_OperationCFSCodeWiseAmountGodown
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