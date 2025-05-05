using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDInvoiceBTT:WFLDInvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<WFLDPreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainerBTT>();
        public List<WFLDPostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainerBTT>();
        public List<WFLDPostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrgBTT>();
        public IList<WFLDContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmountBTT>();
        public List<WFLDOperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmountBTT>();
        public List<WFLDPreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<WFLDPreInvoiceCargoBTT>();
        public List<WFLDBTTPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDBTTPostPaymentChargebreakupdate>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class WFLDPostPaymentContainerBTT: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLDPostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WFLDContainerWiseAmountBTT : ContainerWiseAmount
    {

    }

    public class WFLDPreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class WFLDOperationCFSCodeWiseAmountBTT
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

    public class WFLDPreInvoiceCargoBTT:PreInvoiceCargo {
    }



    public class WFLDBTTPostPaymentChargebreakupdate
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