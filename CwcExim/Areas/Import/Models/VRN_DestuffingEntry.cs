using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_DestuffingEntry
    {
        public string StartDate { get; set; }
        public int DestuffingEntryId { get; set; }
        public string DestuffingEntryNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DestuffingEntryDate { get; set; }
        public int TallySheetId { get; set; }
        public int ContainerId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }
        public int? CHAId { get; set; }
        public string CHA { get; set; }
       
        public string Rotation { get; set; }
       
        public int DeliveryType { get; set; }
     
        public int GodownId { get; set; }


        public int Id { get; set; }

        public string DestuffingEntryXML { get; set; }
        public IList<VRN_DestuffingEntryDtl> lstDtl { get; set; } = new List<VRN_DestuffingEntryDtl>();
        public IList<GodownLocation> lstLocation { get; set; } = new List<GodownLocation>();
        public string TallySheetDate { get; set; }
    }
    public class VRN_DestuffingEntryDtl
    {
        public int IsEditable { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int TallySheetDtlId { get; set; }
        [StringLength(45, ErrorMessage = "BOL No Cannot Be More Than 45 Characters")]
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string LineNo { get; set; }
        public int CommodityId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Commodity { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfPackages { get; set; }
        public int ReceivedPackages { get; set; }
        public string UOM { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal DestuffingWeight { get; set; }
        public decimal TallySheetArea { get; set; }
        public decimal Area { get; set; }
        public string GodownWiseLocationIds { get; set; }
        public string GodownWiseLocationNames { get; set; }
        public string Remarks { get; set; }
        public int GodownId { get; set; }
        public int ContainerId { get; set; }
        [Range(0, 99999999.99, ErrorMessage = "CIF Value should be greater than 0 and less than  equal to 99999999.99 ")]
        public decimal CIFValue { get; set; }
        [Range(0, 99999999.99, ErrorMessage = "Gross Duty should be greater than 0 and less than  equal to 99999999.99 ")]
        public decimal GrossDuty { get; set; }
        [StringLength(30, ErrorMessage = "BOE No Cannot Be More Than 45 Characters")]
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string OBLWiseDestuffingDate { get; set; }
        public int CargoType { get; set; }
        public string Vessel { get;set; }
        public string Voyage { get; set; }
        public int StorageType { get; set; } = 1;
    }
    public class VRN_DestuffingList
    {
        public int DestuffingEntryId { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string ContainerNo { get; set; }
        public string Rotation { get; set; }
        public string ShippingLine { get; set; }
        public string CHA { get; set; }
    }
   
    public class VRN_DestuffingSheet
    {
        public string StartDate { get; set; }
        public string DestuffingEntryNo { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string DestuffingEntryDateTime { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string GateInDate { get; set; }
        public string CustomSealNo { get; set; }
        public string SlaSealNo { get; set; }
        public string CFSCode { get; set; }
        public string IGMNo { get; set; }
        public string MovementType { get; set; }
        public string ShippingLine { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string GodownName { get; set; }
        public IList<VRN_DestuffingSheetDtl> lstDtl { get; set; } = new List<VRN_DestuffingSheetDtl>();
       
    }
    public class VRN_DestuffingSheetDtl
    {
        public string SMTPNo { get; set; }
        public string OblNo { get; set; }
        public string Importer { get; set; }
        public string Cargo { get; set; }
        public string Type { get; set; }
        public int NoOfPkg { get; set; }
        public int PkgRec { get; set; }
        public decimal Weight { get; set; }
        public decimal Area { get; set; }
        public string GodownWiseLctnNames { get; set; }
        public string Remarks { get; set; }

    }
}
