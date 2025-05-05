using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSR_LoadContReq
    {
        public int LoadContReqId { get; set; }
        [Display(Name = "Load Container Req No.")]
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public int CHAId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        [StringLength(500, ErrorMessage = "Remarks Cannot Be More Than 500 Characters In Length")]
        public string Remarks { get; set; }

        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Via { get; set; }
        public string Voyage { get; set; }
        [StringLength(500, ErrorMessage = "Factory Address Cannot Be More Than 500 Characters In Length")]
        public string FactoryAddress { get; set; }
        public string PikUpFrom { get; set; }

        public string MoveTo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Distance { get; set; }

        public List<DSRLoadContReqDtl> lstContDtl { get; set; } = new List<DSRLoadContReqDtl>();
        public string StringifyData { get; set; }
        public string Movement { get; set; }
        public List<DSRLoadCont> lstContainerInfo { get; set; } = new List<DSRLoadCont>();
        public bool IsMoveToRCT { get; set; }

      
        public int? PODID { get; set; }

       
        public string POD { get; set; }
        public string StuffingDate { get; set; }
        public string ValidDate { get; set; }
        public string CargoWeight { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLine { get; set; }

        public string PickupValidity { get; set; }

        public bool Loadmovement { get; set; }        
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
        public string CustomExaminationType { get; set; }

    }

    public class DSRLoadContReqDtl
    {
        public int LoadContReqDetlId { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public bool IsReefer { get; set; }
        public bool IsInsured { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public int CommodityId { get; set; }
        public int CargoType { get; set; }
        [StringLength(500, ErrorMessage = "Cargo Description Cannot Be More Than 500 Characters In Length")]
        public string CargoDescription { get; set; }

        public string StuffingDate { get; set; }
        public decimal GrossWt { get; set; }
        public int NoOfUnits { get; set; }
        public decimal FobValue { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CommodityName { get; set; }
        public string EntryNo { get; set; }
        public bool IsWashing { get; set; }
        public bool IsWeighment { get; set; }
        public bool IsReworking { get; set; }
        public bool IsInternalShifting { get; set; }
        public bool IsLiftOnEmpty { get; set; }
        public bool IsLiftOffEmpty { get; set; }

        public int RMSValue { get; set; }
        public int ReeferHrs { get; set; }
        public string EmptyDate { get; set; }
        public bool BothWayEmpty { get; set; }
        public string ShippingSeal { get; set; }
        public string CustomSeal { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
        public bool IsSEZ { get; set; }
        public int PacketsTo { get; set; }
        public int PacketsFrom { get; set; }
    }
    public class DSRListLoadContReq
    {
        public int LoadContReqId { get; set; }
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public string CHAName { get; set; }
    }
    public class DSRLoadCont
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string EntryNo { get; set; }
        public string CargoType { get; set; }
    }
}