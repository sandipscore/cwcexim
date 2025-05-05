using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class HDBLoadContReq
    {
        public int LoadContReqId { get; set; }
        [Display(Name = "Load Container Req No.")]
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public int CHAId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public string Remarks { get; set; }

        public bool IsODC { get; set; }
        public List<HDBLoadContReqDtl> lstContDtl { get; set; } = new List<HDBLoadContReqDtl>();
        public string StringifyData { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]

        public string Transportation { get; set; }

        public string IsPrinting { get; set; }
        [Display(Name = "Package Type")]
        public string PackageType { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
        public decimal? Distance { get; set; }
      

        public string OtherPkgType { get; set; }
        public int NoOfPrint { get; set; }

        public string IsConfirm { get; set; }
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
        public string CustomExaminationType { get; set; }

    }
    public class HDBLoadContReqDtl
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
        public string CargoDescription { get; set; }
        public decimal GrossWt { get; set; }
        public int NoOfUnits { get; set; }
        public decimal FobValue { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CommodityName { get; set; }
        public string PackageType { get; set; }
        public string OtherPkgType { get; set; }

        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string SCMTRPackageType { get; set; }
        public string CustomSeal { get; set; }
        public string ContLoadType { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
        public bool IsSEZ { get; set; }

        public int PacketsTo { get; set; }
        public int PacketsFrom { get; set; }
    }
    public class HDBListLoadContReq
    {
        public int LoadContReqId { get; set; }
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public string CHAName { get; set; }
        public string groupContainer { get; set; }
        public string groupSize { get; set; }
        public string groupShipBill { get; set; }
        public string PortName { get; set; }
        public string ShippingBillDate { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }

    }
}