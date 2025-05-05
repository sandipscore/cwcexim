using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VIZ_InvoiceBTT:VIZ_InvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<VIZ_PreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<VIZ_PreInvoiceContainerBTT>();
        public List<VIZ_PostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<VIZ_PostPaymentContainerBTT>();
        public List<VIZ_PostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<VIZ_PostPaymentChrgBTT>();
        public IList<VIZ_ContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<VIZ_ContainerWiseAmountBTT>();
        public List<VIZ_OperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<VIZ_OperationCFSCodeWiseAmountBTT>();
        public List<VIZ_PreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<VIZ_PreInvoiceCargoBTT>();
        public List<VIZ_BTTPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VIZ_BTTPostPaymentChargebreakupdate>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class VIZ_PostPaymentContainerBTT: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VIZ_PostPaymentChrgBTT : PostPaymentCharge
    {
               
    }

    public class VIZ_ContainerWiseAmountBTT : ContainerWiseAmount
    {

    }

    public class VIZ_PreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class VIZ_OperationCFSCodeWiseAmountBTT
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
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Clause { get; set; }
    }

    public class VIZ_PreInvoiceCargoBTT:PreInvoiceCargo {
    }



    public class VIZ_BTTPostPaymentChargebreakupdate
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