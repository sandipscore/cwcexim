using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_DeliveryApplication
    {
        public int DeliveryId { get; set; }

        [Display(Name = "Delivery No")]
        public string DeliveryNo { get; set; }
        public int DestuffingId { get; set; }

        [Display(Name = "Destuffing Entry No")]
        public string DestuffingEntryNo { get; set; }
        public int ImporterId { get; set; }
        public int CHAId { get; set; }
        public string DeliveryAppDtlXml { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string IsPrinting { get; set; }
        public int NoOfPrint { get; set; } = 0;
        public string ArrivalDate { get; set; } = string.Empty;
        public string TransportMode { get; set; } = string.Empty;
        public List<Hdb_DeliveryApplicationDtl> LstDeliveryAppDtl { get; set; } = new List<Hdb_DeliveryApplicationDtl>();
        public List<TSAForwarder> lstTSAForwarder { get; set; } = new List<TSAForwarder>();

    }

    public class Hdb_DeliveryApplicationDtl
    {
        public int DeliveryDtlId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }

        public string OBLNo { get; set; }
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
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Importer { get; set; }
        public int ImporterId { get; set; }
        public string CHA { get; set; }
        public string BOEDate { get; set; }
        public int CHAId { get; set; }
        public int IsBndConversion { get; set; }
        public int ForwarderId { get; set; }
        public string ForwarderName { get; set; }
        public string TransportMode { get; set; } = string.Empty;

    }

    public class Hdb_CIFFromOOC
    {
        public Decimal CIF { get; set; }
        public Decimal Duty { get; set; }

    }
    public class Hdb_DeliveryApplicationList
    {
        public int DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DeliveryAppDate { get; set; }
        public string OBL { get; set; }
        public string FormOneNo { get; set; }

        public string DestuffingEntryDate { get; set; }

    }
    public class Hdb_DestuffingEntryNoList
    {
        public int DestuffingId { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string ArrivalDate { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public string HBLNo { get; set; }
    }

    public class Hdb_BOELineNoList
    {
        public int DestuffingEntryDtlId { get; set; }
        public string BOELineNo { get; set; }
    }
}