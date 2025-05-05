using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_MovementInvoiceV2 : PPG_InvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<PpgPreInvoiceContainerV2> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainerV2>();
        public List<PpgPostPaymentContainerV2> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainerV2>();
        public List<PpgPostPaymentChrgV2> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrgV2>();
        public IList<PpgContainerWiseAmountV2> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmountV2>();
        public List<ppgCMPostPaymentChargebreakupdateV2> lstPostPaymentChrgBreakup { get; set; } = new List<ppgCMPostPaymentChargebreakupdateV2>();
        public List<PpgOperationCFSCodeWiseAmountV2> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmountV2>();
        public List<PpgShipbillwiseAmountV2> lstShipbillwiseAmountV2 { get; set; } = new List<PpgShipbillwiseAmountV2>();


        
        //--------------------------------------------------------------------------------------------------------------------
    }

    public class PpgPostPaymentContainerGodownV2 : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgGodownV2 : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmountGodownV2 : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainerGodownV2 : PreInvoiceContainer
    {

    }
    public class ppgCMPostPaymentChargebreakupdateV2
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
    public class PPGTentativeInvoiceV2
    {
        public static PPG_MovementInvoiceV2 InvoiceObjW;
        public static PPG_MovementInvoiceV2 InvoiceObjGR;
        public static PPG_MovementInvoiceV2 InvoiceObjFMC;
    }



    public class PpgOperationCFSCodeWiseAmountGodownV2
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

    public class PpgShipbillwiseAmountV2
    {
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string ChargeType { get; set; }
        public decimal Amount { get; set; }
        public string GateInDate { get; set; }
        public string CCINDate { get; set; }
        public int CargoType { get; set; }
    }
}