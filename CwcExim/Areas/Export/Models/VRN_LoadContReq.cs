using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_LoadContReq
    {
        public int LoadContReqId { get; set; }
        [Display(Name = "Load Container Req No.")]
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public int CHAId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public string Remarks { get; set; }

        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Via { get; set; }
        public string Voyage { get; set; }
        public string Movement { get; set; }
        public List<VRN_LoadContReqDtl> lstContDtl { get; set; } = new List<VRN_LoadContReqDtl>();
        public string StringifyData { get; set; }
        public string ExportType { get; set; }

        public string ExaminationType { get; set; } //Full/OnWheel
        public bool IsEMGDocument { get; set; }
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
    }
    public class VRN_LoadContReqDtl
    {
        public int containerlassid { get; set; }
        public string CntainerClass { get; set; }
        public int LoadContReqDetlId { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; } = string.Empty;
        public string Size { get; set; }
        public bool IsReefer { get; set; }
        public bool IsInsured { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public int CommodityId { get; set; }
        public int CargoType { get; set; }
        public string CargoDescription { get; set; }
        public decimal GrossWt { get; set; }
        public int NoOfUnits { get; set; }
        public decimal FobValue { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CommodityName { get; set; }
        public string CommodityType { get; set; }//Perishable - Non perishable

        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }
        public string CustomSeal { get; set; }
        public string ContLoadType { get; set; }
    }
}
