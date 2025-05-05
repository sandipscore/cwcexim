using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VIZInvoiceYard : VIZInvoiceBase
    {

        //--------------------------------------------------------------------------------------------------------------------
        public List<VIZPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VIZPreInvoiceContainer>();
        public List<VIZPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VIZPostPaymentContainer>();
        public List<VIZPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VIZPostPaymentChrg>();
        public IList<VIZContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VIZContainerWiseAmount>();
        public List<VIZPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VIZPostPaymentChargebreakupdate>();

        public List<VIZOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VIZOperationCFSCodeWiseAmount>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
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
    }


    public class VIZPostPaymentContainer : PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }

    }
    public class VIZPostPaymentChrg : PostPaymentCharge
    {
        //public int OperationId { get; set; }
    }

    public class VIZContainerWiseAmount
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

    public class VIZPreInvoiceContainer : PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }

    }
    public class VIZListOfExpInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
        public string ContainerNo { get; set; }
        public string AppNo { get; set; }

    }
    public class VIZOperationCFSCodeWiseAmount
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

    public class VIZ_ExpContWiseAmount
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
    //public class WFLD_ExpOperationContWiseCharge
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
    public class VIZPostPaymentChargebreakupdate
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
    public class VIZPaySheetStuffingRequest
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
        public int ImporterId { get; set; } = 0;
        public int ImpCount { get; set; } = 0;

    }
}