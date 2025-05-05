using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VLDAInvoiceYard : VLDAInvoiceBase
    {

        //--------------------------------------------------------------------------------------------------------------------
        public List<VLDAPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VLDAPreInvoiceContainer>();
        public List<VLDAPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VLDAPostPaymentContainer>();
        public List<VLDAPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VLDAPostPaymentChrg>();
        public IList<VLDAContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VLDAContainerWiseAmount>();
        public List<VLDAPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VLDAPostPaymentChargebreakupdate>();

        public List<VLDAOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VLDAOperationCFSCodeWiseAmount>();
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


    public class VLDAPostPaymentContainer : PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }

    }
    public class VLDAPostPaymentChrg : PostPaymentCharge
    {
        //public int OperationId { get; set; }
    }

    public class VLDAContainerWiseAmount
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

    public class VLDAPreInvoiceContainer : PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }

    }
    public class VLDAListOfExpInvoice
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
    public class VLDAOperationCFSCodeWiseAmount
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

    public class VLDA_ExpContWiseAmount
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
    //public class VLDA_ExpOperationContWiseCharge
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
    public class VLDAPostPaymentChargebreakupdate
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
    public class VLDAPaySheetStuffingRequest
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