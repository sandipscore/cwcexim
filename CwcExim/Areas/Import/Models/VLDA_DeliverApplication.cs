using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class VLDA_DeliverApplication
    {

        public int InvoiceId { get; set; } = 0;
        public string SEZValue { get; set; } = "";
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

        public string SEZ { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public List<VLDADeliveryGodownDtl> LstDeliveryGodownDtl { get; set; } = new List<VLDADeliveryGodownDtl>();
        public List<VLDADeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<VLDADeliveryApplicationDtl>();
        public List<VLDADeliveryOrdDtl> LstDeliveryordDtl { get; set; } = new List<VLDADeliveryOrdDtl>();

        public List<VLDAPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VLDAPreInvoiceContainer>();
        public List<VLDAPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VLDAPostPaymentContainer>();
        public List<VLDAPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VLDAPostPaymentChrg>();
        public IList<VLDAContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VLDAContainerWiseAmount>();

        public List<VLDAOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VLDAOperationCFSCodeWiseAmount>();
        public List<VLDADeliPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VLDADeliPostPaymentChargebreakupdate>();

        public List<VLDAInvoiceCargo> lstInvoiceCargo { get; set; } = new List<VLDAInvoiceCargo>();



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
        public int Cargo { get; set; }
        public bool IsOpenDestuffing { get; set; }
        public bool IsDestuffing { get; set; }
        //public string  GodownName { get; set; }
    }
   
    public class VLDADeliveryApplicationDtl
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
        public decimal ResvGrossWt { get; set; }
        public decimal SQM { get; set; }
        public decimal CUM { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
        public int DelNoOfPackages { get; set; }
        public decimal DelGrossWt { get; set; }
        public decimal DelRevGrossWt { get; set; }
        public decimal DelSQM { get; set; }
        public decimal DelCUM { get; set; }
        public decimal DelCIF { get; set; }
        public decimal DelDuty { get; set; }
        public string ShippingLine { get; set; }
        public int OblFreeFlag { get; set; }
        public string TSA { get; set; }
    }
    public class VLDADeliveryGodownDtl
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
    public class VLDADeliveryOrdDtl
    {
        public int OrderId { get; set; }
        public string DeliveryNo { get; set; }
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }
    public class VLDADeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DeliveryAppDate { get; set; }
    }

    public class VLDAMergeDeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class VLDADestuffingEntryNoList
    {
        public int DestuffingId { get; set; }
        public string DestuffingEntryNo { get; set; }
    }

    public class VLDABOELineNoList
    {
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }

        public string CHA { get; set; }

        public string CHAId { get; set; }

        public string Billtoparty { get; set; }
        public string IsInsured { get; set; }
        public string InsuredFrom { get; set; }
        public string InsuredTo { get; set; }
        public string Transporter { get; set; }
        public string PartyName { get; set; }
        public string PartyId { get; set; }
        public string PartyInsured { get; set; }
        public string Oblcount { get; set; }

    }

}