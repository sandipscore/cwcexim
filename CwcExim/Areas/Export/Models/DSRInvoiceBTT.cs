using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSRInvoiceBTT: DSRInvoiceBase
    {
        //--------------------------------------------------------------------------------------------------------------------
        public List<DSRPreInvoiceContainerBTT> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainerBTT>();
        public List<DSRPostPaymentContainerBTT> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainerBTT>();
        public List<DSRPostPaymentChrgBTT> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrgBTT>();
        public IList<DSRContainerWiseAmountBTT> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmountBTT>();
        public List<DSROperationCFSCodeWiseAmountBTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmountBTT>();
        public List<DSRPreInvoiceCargoBTT> lstPreInvoiceCargo { get; set; } = new List<DSRPreInvoiceCargoBTT>();
        public List<DSRBTTPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRBTTPostPaymentChargebreakupdate>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        //public IList<DSRHTClauseList> ActualApplicable { get; set; } = new List<DSRHTClauseList>();
        public bool IsPartyStateInCompState { get; set; }
        public string ExportUnder { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class DSRPostPaymentContainerBTT : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSRPostPaymentChrgBTT : PostPaymentCharge
    {
        public int OperationId { get; set; }
        public decimal HazAmt { get; set; } = 0;
        public decimal OdcAmt { get; set; } = 0;
        public decimal HazTotal { get; set; } = 0;
        public decimal OdcTotal { get; set; } = 0;
        public decimal HAZIGSTAmt { get; set; } = 0M;
        public decimal HAZCGSTAmt { get; set; } = 0M;
        public decimal HAZSGSTAmt { get; set; } = 0M;
    }

    public class DSRContainerWiseAmountBTT : ContainerWiseAmount
    {

    }

    public class DSRPreInvoiceContainerBTT : PreInvoiceContainer
    {

    }

    public class DSROperationCFSCodeWiseAmountBTT
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

    public class DSRPreInvoiceCargoBTT : PreInvoiceCargo {
    }



    public class DSRBTTPostPaymentChargebreakupdate
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