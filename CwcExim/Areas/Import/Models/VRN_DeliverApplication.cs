using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CwcExim.Models;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_DeliverApplication
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

        public bool SEZ { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public List<VRN_DeliveryGodownDtl> LstDeliveryGodownDtl { get; set; } = new List<VRN_DeliveryGodownDtl>();
        public List<VRN_DeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<VRN_DeliveryApplicationDtl>();
        public List<VRN_DeliveryOrdDtl> LstDeliveryordDtl { get; set; } = new List<VRN_DeliveryOrdDtl>();

        public List<VRN_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VRN_PreInvoiceContainer>();
        public List<VRN_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VRN_PostPaymentContainer>();
        public List<VRN_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VRN_PostPaymentChrg>();
        public IList<VRN_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VRN_ContainerWiseAmount>();

        public List<VRN_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VRN_OperationCFSCodeWiseAmount>();
        public List<VRN_DeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VRN_DeliPostPaymentChargebreakupdate>();

        public List<VRN_InvoiceCargo> lstInvoiceCargo { get; set; } = new List<VRN_InvoiceCargo>();



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

        public bool ImportToBondCon { get; set; }
        public string ExportUnder { get; set; }

    }

    //public class VRN_ContainerWiseAmount
    //{
    //    public int InvoiceId { get; set; }
    //    public int ContainerId { get; set; }
    //    public string CFSCode { get; set; }
    //    public string LineNo { get; set; } = "0";
    //    public decimal EntryFee { get; set; } = 0M;
    //    public decimal CstmRevenue { get; set; } = 0M;
    //    public decimal GrEmpty { get; set; } = 0M;
    //    public decimal GrLoaded { get; set; } = 0M;
    //    public decimal ReeferCharge { get; set; } = 0M;
    //    public decimal StorageCharge { get; set; } = 0M;
    //    public decimal InsuranceCharge { get; set; } = 0M;
    //    public decimal PortCharge { get; set; } = 0M;
    //    public decimal WeighmentCharge { get; set; } = 0M;

    //}
    //public class VRN_PostPaymentContainer : PostPaymentContainer
    //{

    //    public string OBLNo { get; set; }
    //    public string SealCutDate { get; set; }
    //    public string DeliveryDate { get; set; }

    //}

    //public class VRN_PreInvoiceContainer : PreInvoiceContainer
    //{

    //    public string OBLNo { get; set; }
    //    public string SealCutDate { get; set; }

    //}

    public class VRN_DeliveryGodownDtl
    {



        public string BOELineNoGodown { get; set; }

        public string OBL { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public int DelQty { get; set; }
        public decimal DelWeight { get; set; }

        public decimal DelRevWeight { get; set; }
        public decimal DelSQM { get; set; }
        public decimal DelRevSQM { get; set; }
        public decimal DelCBM { get; set; }
        public decimal DelCIF { get; set; }
        public decimal DelDuty { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
    }
    public class VRN_DeliveryApplicationDtl
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

        public string ContainerNo { get; set; }
        public string Size { get; set; }

        public string CFSCode { get; set; }

        public string BOL { get; set; }
    }

    public class VRN_DeliveryOrdDtl
    {
        public int OrderId { get; set; }
        public string DeliveryNo { get; set; }
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }
    public class VRN_DeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DeliveryAppDate { get; set; }
    }

    public class VRN_MergeDeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string DeliveryDate { get; set; }
        public int PartyID { get; set; }
        public int InvoiceId { get; set; }
        public int IssueSlipId { get; set; }
    }
    public class VRN_DestuffingEntryNoList
    {
        public int DestuffingId { get; set; }
        public string DestuffingEntryNo { get; set; }
    }

    public class VRN_BOELineNoList
    {
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }

        public string CHA { get; set; }

        public string CHAId { get; set; }

    }
}
