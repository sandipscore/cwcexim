using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Kol_Boewisedstfprnt
    {
        public int DestuffingEntryId { get; set; }
        public string BOENo { get; set; }
    }
    public class ListOfboe
    {
        public int DestuffingEntryDtlId { get; set; }
        public string boeno { get; set; }
    }

    public class boedestuffing
    {

        public int DestuffingEntryId { get; set; }
        public int DestuffingDtlId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int DeStuffingWODtlId { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DestuffingEntryDate { get; set; }

        
        public string DODate { get; set; }

   
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string Rotation { get; set; }
        public string SealNo { get; set; }
        public string CustomSealNo { get; set; }
        public string Remarks { get; set; }
        public string LCLFCL { get; set; }
        public string DestuffingEntryXML { get; set; }
        public int Uid { get; set; }

        // public int DestuffingEntryId { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }

      
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string MarksNo { get; set; }
        public int? CHAId { get; set; }
        public string CHA { get; set; }
        public int? ImporterId { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public int? CargoType { get; set; }
        public int? CommodityId { get; set; }
        public string Commodity { get; set; }
        public int? NoOfPackages { get; set; }

        public decimal? CUM { get; set; }

     
        public decimal? SQM { get; set; }
        public int? GodownId { get; set; }
        public string GodownName { get; set; }
        public decimal? GrossWeight { get; set; }

       
        public decimal? DestuffingWeight { get; set; }
        public decimal? CIFValue { get; set; }
        public decimal? Duty { get; set; }
        public int AppraisementStatus { get; set; }
        public string GodownWiseLocationIds { get; set; }
        public string GodownWiseLctnNames { get; set; }
        public string LocationDetails { get; set; }
        public string ClearLocation { get; set; }
        public string Cargo { get; set; }

        public List<boedestuffing> Lstbode = new List<boedestuffing>();
        public int RemNoOfPackages { get; set; }
        public decimal RemDestuffingWeight { get; set; }
        public decimal RemCIFValue { get; set; }
        public decimal RemDuty { get; set; }
    }
}