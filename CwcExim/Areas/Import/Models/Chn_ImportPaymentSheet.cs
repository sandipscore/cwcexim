using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Chn_ImportPaymentSheet:ChnInvoiceBase
    {//--------------------------------------------------------------------------------------------------------------------
        public List<Chn_ImpPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Chn_ImpPreInvoiceContainer>();
        public List<Chn_ImpPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Chn_ImpPostPaymentContainer>();
        public List<Chn_ImpPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Chn_ImpPostPaymentChrg>();

        public IList<Chn_ADDCWCImpPostPaymentCharge> lstADDPostPaymentChrg { get; set; } = new List<Chn_ADDCWCImpPostPaymentCharge>();
        public IList<Chn_ImpContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Chn_ImpContainerWiseAmount>();
        public List<Chn_ImpPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<Chn_ImpPostPaymentChargebreakupdate>();

      //  public IList<ChnExpADDCWCInvoiceChargeBase> lstADDPostPaymentChrg { get; set; } = new List<ChnExpADDCWCInvoiceChargeBase>();


        public List<Chn_ImpOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Chn_ImpOperationCFSCodeWiseAmount>();
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
        public string SEZ { get; set; }
        public int DeliveryType { get; set; } = 1;
    }
    public class Chn_ImpPostPaymentContainer
    {
        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }
        public DateTime? DestuffingDate { get; set; }
        public string CFSCode { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Reefer { get; set; } = 0;
        public int Insured { get; set; } = 0;
        public int RMS { get; set; } = 0;
        public int HeavyScrap { get; set; } = 0;
        public int AppraisementPerct { get; set; } = 0;
        public int CargoType { get; set; } = 0;
        public string ArrivalDate { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public DateTime? CartingDate { get; set; } = null;
        public DateTime? StuffingDate { get; set; } = null;
        //public DateTime? DestuffingDate { get; set; } = null;
        public int NoOfPackages { get; set; } = 0;
        public decimal GrossWt { get; set; } = 0M;
        public decimal WtPerUnit { get; set; } = 0M;
        public decimal SpaceOccupied { get; set; } = 0M;
        public string SpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int DeliveryType { get; set; } = 0;
        public decimal StuffCUM { get; set; } = 0M;
        public string LCLFCL { get; set; }

        public decimal NoOfPackagesDel { get; set; } = 0M;
        public int ISODC { get; set; } = 0;
        public decimal Clauseweight { get; set; } = 0M;

        public int ParkDays { get; set; } = 0;
        public int LockDays { get; set; } = 0;

    }


    public class ChnExpADDCWCInvoiceChargeBase
    {
        public int ChargeId { get; set; }
        public string OperationId { get; set; }
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxable { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal Total { get; set; }
        public int ADDCWC { get; set; }
    }
    public class Chn_ImpPostPaymentChrg : PostPaymentCharge
    {
        public int ADDCWC { get; set; }
    }

    public class Chn_ADDCWCImpPostPaymentCharge
    {
        public int ADDCWC { get; set; }
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;
        public decimal Discount { get; set; } = 0M;
        public decimal Taxable { get; set; } = 0M;
        public decimal IGSTPer { get; set; } = 0M;
        public decimal IGSTAmt { get; set; } = 0M;
        public decimal CGSTPer { get; set; } = 0M;
        public decimal CGSTAmt { get; set; } = 0M;
        public decimal SGSTPer { get; set; } = 0M;
        public decimal SGSTAmt { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public decimal ActualFullCharge { get; set; } = 0M;
        public int OperationId { get; set; }      
      
    }
    public class Chn_ImpContainerWiseAmount
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
    public class Chn_ImpPreInvoiceContainer : PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public int ParkDays { get; set; } = 0;
        public int LockDays { get; set; } = 0;

    }

    public class Chn_ImpOperationCFSCodeWiseAmount
    {
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

    public class Chn_ImpContWiseAmount
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
    public class Chn_ImpPostPaymentChargebreakupdate
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
    public class Chn_PaySheetStuffingRequest
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
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
        public string IsInsured { get; set; }
        public string InsuredFrom { get; set; }
        public string InsuredTo { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Billtoparty { get; set; }

        public string Transporter { get; set; }
    }
    public class Chn_PaymentSheetBOE
    {
        public string CFSCode { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public bool Selected { get; set; }
    }
    public class Chn_ImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyCode { get; set; }
        public bool IsInsured { get; set; }
        public bool IsTransporter { get; set; }
        public string InsuredFrmDate { get; set; }
        public string InsuredToDate { get; set; }
    }
}