using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class Wlj_DeliverApplication
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

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public List<Wlj_DeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<Wlj_DeliveryApplicationDtl>();
        public List<Wlj_DeliveryOrdDtl> LstDeliveryordDtl { get; set; } = new List<Wlj_DeliveryOrdDtl>();
        public DateTime DeliveryDate { get; set; }
        public int ObleFreeFlag { get; set; }

        public string SEZ { get; set; }

    }
    public class Wlj_DeliveryApplicationDtl
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
    }

    public class Wlj_DeliveryOrdDtl
    {
        public int OrderId { get; set; }
        public string DeliveryNo { get; set; }
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }
    public class Wlj_DeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DeliveryAppDate { get; set; }
    }

    public class Wlj_MergeDeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class Wlj_DestuffingEntryNoList
    {
        public int DestuffingId { get; set; }
        public string DestuffingEntryNo { get; set; }
    }

    public class Wlj_BOELineNoList
    {
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }

        public string CHA { get; set; }

        public string CHAId { get; set; }
    }
}
