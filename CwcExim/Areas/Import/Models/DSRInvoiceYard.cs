using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRInvoiceYard : DSRInvoiceBase
    {        

        //--------------------------------------------------------------------------------------------------------------------
        public List<DSRPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainer>();
        public List<DSRPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainer>();
        public List<DSRPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrg>();
        public IList<DSRContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmount>();
        public List<DSRPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRPostPaymentChargebreakupdate>();

        public List<DSROperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmount>();
        public List<DSRBounceChequeDtl> lstBounceChequeDtl { get; set; } = new List<DSRBounceChequeDtl>();
        //public IList<string> ActualApplicable { get; set; } = new List<string>();
        public IList<DSRHTClauseList> ActualApplicable { get; set; } = new List<DSRHTClauseList>();
        //--------------------------------------------------------------------------------------------------------------------
        public string lstPrePaymentContXML { get; set; }
        public string lstPostPaymentContXML { get; set; }
        public string lstPostPaymentChrgXML { get; set; }
        public string lstContWiseAmountXML { get; set; }
        public string lstOperationCFSCodeWiseAmountXML { get; set; }
        public string lstPostPaymentChrgBreakupXML { get; set; }

        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
        public string NDays { get; set; }
        public string ExportUnder { get; set; } = string.Empty;
    }
    
    public class DSRHTClauseList
    {
        public string ClauseName { get; set; }
        public string ClauseId { get; set; }
    }

    public class DSRPostPaymentContainer : PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DestuffDate { get; set; }

    }
    public class DSRPostPaymentChrg : PostPaymentCharge
    {
        //public int OperationId { get; set; }
        public decimal HazAmt { get; set; } = 0M;
        public decimal OdcAmt { get; set; } = 0M;
        public decimal HazTotal { get; set; } = 0M;
        public decimal OdcTotal { get; set; } = 0M;
        public decimal HAZIGSTAmt { get; set; } = 0M;
        public decimal HAZCGSTAmt { get; set; } = 0M;
        public decimal HAZSGSTAmt { get; set; } = 0M;
    }

    public class DSRContainerWiseAmount
    {
        public int InvoiceId { get; set; }
        public int ContainerId { get; set; }
        public string CFSCode { get; set; }
        public string LineNo { get; set; } = "0";
        public decimal EntryFee { get; set; } = 0M;
        public decimal CstmRevenue { get; set; } = 0M;
        public decimal GrEmpty { get; set; } = 0M;
        public decimal GrLoaded { get; set; } = 0M;
        public decimal ReeferCharge { get; set; } = 0M;
        public decimal StorageCharge { get; set; } = 0M;
        public decimal InsuranceCharge { get; set; } = 0M;
        public decimal PortCharge { get; set; } = 0M;
        public decimal WeighmentCharge { get; set; } = 0M;

    }

    public class DSRPreInvoiceContainer : PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }

    }

    public class DSROperationCFSCodeWiseAmount
    {
        //public int InvoiceId { get; set; }
        //public string CFSCode { get; set; }
        //public string ContainerNo { get; set; }
        //public string Size { get; set; }
        //public int OperationId { get; set; }
        //public string ChargeType { get; set; }
        //public decimal Quantity { get; set; }
        //public decimal Rate { get; set; }
        //public decimal Amount { get; set; }

        public int InvoiceId { get; set; }

        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Clause { get; set; }
    }

    public class DSR_ExpContWiseAmount
    {
        public string CFSCode { get; set; }
        public decimal EntryFee { get; set; }
        public decimal CstmRevenue { get; set; }
        public decimal GrEmpty { get; set; }
        public decimal GrLoaded { get; set; }
        public decimal ReeferCharge { get; set; }
        public decimal StorageCharge { get; set; }
        public decimal InsuranceCharge { get; set; }
        public decimal PortCharge { get; set; }
        public decimal WeighmentCharge { get; set; }
    }
    //public class DSR_ExpOperationContWiseCharge
    //{
    //    public string CFSCode { get; set; }
    //    public string ContainerNo { get; set; }
    //    public string Size { get; set; }
    //    public string DocumentType { get; set; }
    //    public string DocumentNo { get; set; }
    //    public string DocumentDate { get; set; }
    //    public string OperationId { get; set; }
    //    public string ChargeType { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal Rate { get; set; }
    //    public decimal Amount { get; set; }
    //    public string Clause { get; set; }
    //}

    public class DSRPostPaymentChargebreakupdate
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
    public class DSRPaySheetStuffingRequest
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string CHAGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int StuffingReqId { get; set; }
        public int DeliveryType { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }

        public string DestuffingEntryDate { get; set; } = "";

        public string Importer { get; set; } = "";

        public int ForwarderId { get; set; } = 0;
        public string Forwarder { get; set; } = "";
        public string HBLNo { get; set; }
        public string Billtoparty { get; set; }
        public string IsInsured { get; set; }
        public string InsuredFrom { get; set; }
        public string InsuredTo { get; set; }
        public string Transporter { get; set; }

        public string ContainerList { get; set; }
        public string OBLList { get; set; }
        public int RMS { get; set; }
        public int rows { get; set; }
    }

    public class DSRBounceChequeDtl
    {
        public int CashCollectionId { get; set; }
        public string BncChequeNo { get; set; }
        public string BncChequeDate { get; set; }
        public decimal BncChequeAmt { get; set; }
        public string BounceDate { get; set; }
        public string ChequeDDNo { get; set; }
        public string ChequeDate { get; set; }
        public decimal ChequeAmt { get; set; }
        public string DepositDate { get; set; }
    }
    public class DSRListOfImpInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
    }
    public class DSRYardOBLAmendment
    {
        public int IsAmendment { get; set; }
    }
}