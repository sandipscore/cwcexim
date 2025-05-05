using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ContainerStuffingDtlV2
    {
        public int ContainerStuffingDtlId { get; set; }
        public int ContainerStuffingId { get; set; }
        public int StuffingReqDtlId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Consignee { get; set; }
        public string MarksNo { get; set; }
        public int? Insured { get; set; }
        public int? Refer { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public string Exporter { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public int StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public decimal Fob { get; set; }
        public string Size { get; set; }
        public string StuffingType { get; set; }
        public string CustomSeal { get; set; }
        public string ShippingSeal { get; set; }
        public string RequestDate { get; set; }
        public string CommodityName { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoType { get; set; }
        //  public List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl> { get; set;}
        public List<ContainerStuffingDtlV2> LstStuffing { get; set; } = new List<ContainerStuffingDtlV2>();
        public List<ContainerStuffingV2SCMTR> LstSCMTR { get; set; } = new List<ContainerStuffingV2SCMTR>();


        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }

        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string MCINPCIN { get; set; }
        public int SEZ { get; set; }
        public int PacketsFrom { get; set; }
        public int PacketsTo { get; set; }
    }


    public class ContainerStuffingV2SCMTR
    {
        public int Id { get; set; }
        public int ContainerStuffingId { get; set; }
        public string ReportingpartyCode { get; set; }
        public string Equipmenttype { get; set; }
        public string ContainerID { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipmentLoadStatus { get; set; }
        public string FinalDestinationLocation { get; set; }
        public string EventDate { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentSealNo { get; set; }
        public string EquipmentStatus { get; set; }

        public Int32 CargoSequenceNo { get; set; }
        public string DocumentType { get; set; }
        public string ShipmentLoadStatus { get; set; }
        public string PackageType { get; set; }

        public Int32 EquipmentSerialNo { get; set; }
        public Int32 DocumentSerialNo { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentReferenceNo { get; set; }
        
    }
}