using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DeliveryApplication
    {
        public int DeliveryId { get; set; }

        [Display(Name = "Delivery No")]
        public string DeliveryNo { get; set; }

        [Display(Name = "Delivery Date")]
       
        public string DeliveryDate { get; set; }
        public int DestuffingId { get; set; }

        [Display(Name = "Destuffing Entry No")]
        public string DestuffingEntryNo { get; set; }
        
        public int CHAId { get; set; }
        public string DeliveryAppDtlXml { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        
        public List<DeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<DeliveryApplicationDtl>();
    }
    public class DeliveryApplicationDtl
    {
        public int DeliveryDtlId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }

        public string BEODate { get; set; }
        public string CargoDescription { get; set; }
        public string Commodity { get; set; }
        public int CommodityId { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWt { get; set; }
        public decimal SQM { get; set; }
        public decimal CUM { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
        public decimal DelNoOfPackages { get; set; }
        public decimal DelGrossWt { get; set; }
        public decimal DelSQM { get; set; }
        public decimal DelCUM { get; set; }
        public decimal DelCIF { get; set; }
        public decimal DelDuty { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public int ImporterId { get; set; }

    }

    public class CIFFromOOC
        {
        public Decimal CIF { get; set; }
        public Decimal Duty { get; set; }
        public String BOE_DATE { get; set; }
        

    }
    public class DeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DeliveryAppDate { get; set; }
    }
    public class DestuffingEntryNoList
    {
        public int DestuffingId { get; set; }
        public string DestuffingEntryNo { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string ContainerNo { get; set; }

    }

    public class BOELineNoList
    {
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }
    }

}