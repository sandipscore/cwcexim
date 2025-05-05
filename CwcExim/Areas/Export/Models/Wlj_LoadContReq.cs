using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Wlj_LoadContReq
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
        public List<Wlj_LoadContReqDtl> lstContDtl { get; set; } = new List<Wlj_LoadContReqDtl>();
        public string StringifyData { get; set; }
        public string ExportType { get; set; }

        public string ExaminationType { get; set; } //Full/OnWheel
        public bool IsEMGDocument { get; set; }
    }
    public class Wlj_LoadContReqDtl
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

       
    }
}
