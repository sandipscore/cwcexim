using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class CHNInvoiceGodown
    {
        public string CompanyGstNo { get; set; }
        public string PartyCode { get; set; }
        public string PartyGstNo { get; set; }

        public decimal TotalTax { get; set; }
        
        public string ApproveOn { get; set; } = string.Empty;
        public string ROAddress { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public int RequestId { get; set; } = 0;
        public string RequestNo { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAddress { get; set; } = string.Empty;
        public string PartyState { get; set; } = string.Empty;
        public string PartyStateCode { get; set; } = string.Empty;
        public string PartyGST { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal TotalAmt { get; set; } = 0M;
        public decimal TotalDiscount { get; set; } = 0M;
        public decimal TotalTaxable { get; set; } = 0M;
        public decimal TotalCGST { get; set; } = 0M;
        public decimal TotalSGST { get; set; } = 0M;
        public decimal TotalIGST { get; set; } = 0M;
        public decimal CWCAmtTotal { get; set; } = 10M;
        public decimal CWCTotal { get; set; } = 0M;

        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal { get; set; } = 0M;
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS { get; set; } = 0M;
        public decimal HTTDS { get; set; } = 0M;
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal InvoiceAmt { get; set; } = 0M;
        public string ShippingLineName { get; set; } = string.Empty;
        public string CHAName { get; set; } = string.Empty;
        public string ImporterExporter { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public string BOEDate { get; set; } = string.Empty;
        public string CFSCode { get; set; } = string.Empty;
        public string DestuffingDate { get; set; } = string.Empty;
        public string StuffingDate { get; set; } = string.Empty;
        public string CartingDate { get; set; } = string.Empty;
        public string ArrivalDate { get; set; } = string.Empty;
        public int TotalNoOfPackages { get; set; } = 0;
        public decimal TotalGrossWt { get; set; } = 0M;
        public decimal TotalWtPerUnit { get; set; } = 0M;
        public decimal TotalSpaceOccupied { get; set; } = 0M;
        public string TotalSpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal TotalValueOfCargo { get; set; } = 0M;
        public decimal PdaAdjustedAmount { get; set; } = 0M;
        public string CompGST { get; set; } = string.Empty;
        public string CompPAN { get; set; } = string.Empty;
        public string CompStateCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        //subir for edir receipt
        public string CashierRemarks { get; set; } = string.Empty;

        public string PDAadjustedCashReceiptEdit { get; set; } = string.Empty;
        // end 
        public int DeliveryType { get; set; } = 1;
        public string BillType { get; set; } = string.Empty;
        public string StuffingDestuffDateType { get; set; } = string.Empty;
        public string StuffingDestuffingDate { get; set; } = string.Empty;
        public string ImporterExporterType { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string UptoDate { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0M;
        public int OperationType { get; set; }
        public int FromGodownId { get; set; }
        public string OldGodownName { get; set; }
        public int ToGodownId { get; set; }
        public string NewGodownName { get; set; }
        //public int OldLocationIds { get; set; }
        public string OldLctnNames { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public int MovementId { get; set; } = 0;
        public decimal OTHours { get; set; } = 0;
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Chn_ImpPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Chn_ImpPreInvoiceContainer>();
        public List<Chn_ImpPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Chn_ImpPostPaymentContainer>();
        public List<Chn_ImpPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Chn_ImpPostPaymentChrg>();
        public IList<Chn_ImpContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Chn_ImpContainerWiseAmount>();

        public List<Chn_ImpOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Chn_ImpOperationCFSCodeWiseAmount>();
        public List<Chn_ImpPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<Chn_ImpPostPaymentChargebreakupdate>();
        public IList<Chn_ADDCWCImpPostPaymentCharge> lstADDPostPaymentChrg { get; set; } = new List<Chn_ADDCWCImpPostPaymentCharge>();
        public List<CHNInvoiceCargo> lstInvoiceCargo { get; set; } = new List<CHNInvoiceCargo>();

        //--------------------------------------------------------------------------------------------------------------------
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string OldLocationIds { get; set; }
        public string PaymentMode { get; set; }
        public bool IsPartyStateInCompState { get; set; }
    }
    public class CHNInvoiceCargo : PreInvoiceCargo
    {
        public string LineNo { get; set; } = string.Empty;
    }
    public class Chntentativeinvoice
    {
        public static CHNInvoiceGodown InvoiceObj;
    }
    public class CHNDeliveryApplicationDtl
    {
        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public int DeliveryDtlId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }

        public string OOC_BOENo { get; set; }
        public string OOC_BOEDATE { get; set; }
        public string CargoDescription { get; set; }
        public string Commodity { get; set; }
        public int CommodityId { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWt { get; set; }
        public decimal SQM { get; set; }
        public decimal CUM { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
        public int DelNoOfPackages { get; set; }
        public decimal DelGrossWt { get; set; }
        public decimal DelSQM { get; set; }
        public decimal DelCUM { get; set; }
        public decimal DelCIF { get; set; }
        public decimal DelDuty { get; set; }
        public string ShippingLine { get; set; }
        public int OblFreeFlag { get; set; }
        public string TSA { get; set; }
    }

    public class CHNMergeDeliveryIssueViewModel
    {
        public CHN_DeliverApplication DeliApp { get; set; }
        public CHN_Issueslip IssueSlip { get; set; }
        public int Uid { get; set; }
        public int OldPartyId { get; set; }
        public int OldPayeeId { get; set; }
        public String  SEZ { get; set; }
       public bool IsSupplyICD { get; set; }
        public bool? IsSupply { get; set; }
        [RegularExpression("^[A-Za-z0-9\\-,./\r\n:'&?() ]*$", ErrorMessage = "Invalid Character.")]
        [MaxLength(500)]
        public string Remarks { get; set; } = "";
        public string Remark { get; set; } = "";

        public string ExaminationType { get; set; }
        public string Weighment { get; set; }
        public string Scanned { get; set; }

    }
    public class CHN_DeliverApplication
    {
        public int DeliveryId { get; set; }

        [Display(Name = "Delivery No")]
        public string DeliveryNo { get; set; }
        public int DestuffingId { get; set; }

        [Display(Name = "Destuffing Entry No")]
        public string DestuffingEntryNo { get; set; }
        public int ImporterId { get; set; }
        public int CHAId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public int OTHr { get; set; }
        public string InvoiceType { get; set; }
        public string DeliveryAppDtlXml { get; set; }

        public string DeliveryOrdDtlXml { get; set; }
        public string DeliveryGodownDtlXml { get; set; }


        public string lstPrePaymentContXML { get; set; }
        public string lstPostPaymentContXML { get; set; }
        public string lstPostPaymentChrgXML { get; set; }
        public string lstContWiseAmountXML { get; set; }
        public string lstOperationCFSCodeWiseAmountXML { get; set; }
        public string lstPostPaymentChrgBreakupXML { get; set; }
        public string lstInvoiceCargoXML { get; set; }



        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public List<CHNDeliveryGodownDtl> LstDeliveryGodownDtl { get; set; } = new List<CHNDeliveryGodownDtl>();
        public List<CHNDeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<CHNDeliveryApplicationDtl>();
        public List<CHNDeliveryOrdDtl> LstDeliveryordDtl { get; set; } = new List<CHNDeliveryOrdDtl>();

        public List<Chn_ImpPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Chn_ImpPreInvoiceContainer>();
        public List<Chn_ImpPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Chn_ImpPostPaymentContainer>();
        public List<Chn_ImpPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Chn_ImpPostPaymentChrg>();
        public IList<Chn_ImpContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Chn_ImpContainerWiseAmount>();

        public List<Chn_ImpOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Chn_ImpOperationCFSCodeWiseAmount>();
        public List<Chn_ImpPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<Chn_ImpPostPaymentChargebreakupdate>();

        public List<CHNInvoiceCargo> lstInvoiceCargo { get; set; } = new List<CHNInvoiceCargo>();

        public IList<Chn_ADDCWCImpPostPaymentCharge> lstADDPostPaymentChrg { get; set; } = new List<Chn_ADDCWCImpPostPaymentCharge>();

        public DateTime DeliveryDate { get; set; }

        public DateTime InvoiceDate { get; set; }
        public int ObleFreeFlag { get; set; }

        //New added on 07/06/2019
        [Display(Name = "Vehicle Nos")]
        public int VehicleNos { get; set; } = 0;
        public int VehicleNoUn { get; set; } = 0;

        [Display(Name = "Is Insured")]
        public bool IsInsured { get; set; }

        [Display(Name = "Bill to party")]
        public bool BillToParty { get; set; }

        [Display(Name = "Own movement")]
        public bool IsTransporter { get; set; }
        public bool IsSealCharge { get; set; }
        public String SEZ { get; set; }
        public bool IsSupplyICD { get; set; }
        public int ParkingHours { get; set; }
        public int LockingHours { get; set; }

        public int Weighment { get; set; }
        //public int GodownId { get; set; }
        //public string  GodownName { get; set; }
    }
    public class CHNmergedet
    {
        public int Id { get; set; }
        public string AppNo { get; set; }

    }

    public class CHNDeliveryOrdDtl
    {
        public int OrderId { get; set; }
        public string DeliveryNo { get; set; }
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }
    public class CHNdeliverydet
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string GSTNo { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public string LineNo { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int CargoType { get; set; }
        public string DeliveryAppDate { get; set; }
    }
    public class CHNCHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }

        public bool BillToParty { get; set; }
        public bool IsInsured { get; set; }
        public string InsuredFrmdate { get; set; }
        public string InsuredTodate { get; set; }
        public bool IsTransporter { get; set; }
    }
    public class CHNDeliveryGodownDtl
    {
        public string BOELineNoGodown { get; set; }

        public string OBL { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public int DelQty { get; set; }
        public decimal DelWeight { get; set; }
        public decimal DelSQM { get; set; }
        public decimal DelCBM { get; set; }
        public decimal DelCIF { get; set; }
        public decimal DelDuty { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
    }
    public class CHN_Issueslip
    {
        public int IssueSlipId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string IssueSlipNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string IssueSlipDate { get; set; }

        [StringLength(1000, ErrorMessage = "Cargo Description Cannot Be More Than 1000 Characters")]
        public string CargoDescription { get; set; }
        public int Uid { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceNo { get; set; }

        public List<CHN_IssueSlipReport> LstIssueSlipRpt = new List<CHN_IssueSlipReport>();

        public List<CHN_IssueSlipContainer> LstContainer = new List<CHN_IssueSlipContainer>();

        public List<CHN_IssueSlipCargo> LstCargo = new List<CHN_IssueSlipCargo>();
        public decimal TotalCWCDues { get; set; }
        public string CRNoDate { get; set; }
    }
    public class CHN_IssueSlipContainer
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public decimal Total { get; set; }
        public string BOEDate { get; set; }
        public string BOENo { get; set; }
    }

    public class CHN_IssueSlipCargo
    {
        public string OBLNo { get; set; }
        public string CargoDescription { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }
        public string StackNo { get; set; }
        public string Area { get; set; }
        public decimal NetWeight { get; set; }
    }

    public class CHN_IssueSlipReport
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Vessel { get; set; }
        public string BOEDate { get; set; }
        public string BOENo { get; set; }
        public string CHA { get; set; }
        public string ShippingLine { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public string MarksNo { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Weight { get; set; }
        public string ArrivalDate { get; set; }
        public string DestuffingDate { get; set; }
        public string Location { get; set; } 

        public string DeliveryDate { get; set; }

    }
}