using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_LoadContReq 
    {
        public int LoadContReqId { get; set; }
        [Display(Name = "Load Container Req No.")]
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public int CHAId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public string Remarks { get; set; }

        public int ForeignLinerId { get; set; }
        public string ForeignLiner { get; set; }

   
        public string Vessel { get; set; }

        public string Via { get; set; }


 
        public string Voyage { get; set; }


        public string Forwarder { get; set; }

        public int? ForwarderId { get; set; }

     
        public string MainLine { get; set; }

        public List<WFLDLoadContReqDtl> lstContDtl { get; set; } = new List<WFLDLoadContReqDtl>();
        public string StringifyData { get; set; }
        public string Movement { get; set; }
        public string ContPOL { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
        public string CustomExaminationType { get; set; }
    }

    public class WFLDLoadContReqDtl
    {
        public int LoadContReqDetlId { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public bool IsReefer { get; set; }
        public bool IsInsured { get; set; }
        public bool OutGauge { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public int CommodityId { get; set; }
        public int CargoType { get; set; }
        public string ExamType { get; set; }
        public string ContClass { get; set; }
        public string CargoDescription { get; set; }
        public decimal GrossWt { get; set; }
        public int NoOfUnits { get; set; }
        public decimal FobValue { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CommodityName { get; set; }

        public string ContPOL { get; set; }

        public int? POLid { get; set; }

        public int? PODId { get; set; }

        public string POD { get; set; }

        public string CFSCode { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }
        public string CustomSeal { get; set; }
        public string ContLoadType { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PackUQCDescription { get; set; }

        public int SEZ { get; set; }

        public int PacketsFrom { get; set; }
        public int PacketsTo { get; set; }


    }
    public class WFLDListLoadContReq
    {
        public int LoadContReqId { get; set; }
        public string LoadContReqNo { get; set; }
        public string LoadContReqDate { get; set; }
        public string CHAName { get; set; }
    }
}