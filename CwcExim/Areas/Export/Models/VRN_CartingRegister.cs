using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_CartingRegister
    {
        public int CartingRegisterId { get; set; }

        [Display(Name = "Carting Register No")]
        public string CartingRegisterNo { get; set; }

        [Display(Name = "Application No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ApplicationNo { get; set; }

        [Display(Name = "Application Date")]
        public string ApplicationDate { get; set; }
        [Display(Name = "CartingType")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CartingType { get; set; }
        [Display(Name = "CommodityType")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CommodityType { get; set; }
        
        [Display(Name = "CHA Name")]
        public string CHAName { get; set; }
        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }
        public int CartingAppId { get; set; }
        public string RegisterDate { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<VRN_CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<VRN_CartingRegisterDtl>();
        public IList<ApplicationNoDet> lstAppNo { get; set; } = new List<ApplicationNoDet>();
        public IList<GodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<GodownWiseLocation>();
        public string XMLData { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;

        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string SpaceType { get; set; }
        public string StorageType { get; set; }
        public int IsShortCargo { get; set; }
        public List<VRN_ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<VRN_ShortCargoDetails>();
        public string ShippingBill { get; set; }
    }

    public class VRN_CartingRegisterDtl
    {
        public int CartingAppDtlId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public string CommInvcNo { get; set; }
        public string Exporter { get; set; }
        public string CargoDescription { get; set; }
        public string CommodityName { get; set; }
        public int CargoType { get; set; }
        public string MarksAndNo { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal FoBValue { get; set; }
        public decimal CUM { get; set; }
        public decimal SQM { get; set; }
        public int ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public string LocationDetails { get; set; }
        public string Location { get; set; }//For Location Name
        public int ExporterId { get; set; } = 0;
        public int CommodityId { get; set; } = 0;
        public int CartingRegisterDtlId { get; set; }
        public string StorageType { get; set; }
        public decimal SQMReserved { get; set; } = 0;
        public decimal FoBValue1 { get; set; } = 0;

    }

    public class VRN_ShortCargoDetails
    {
        public int ShortCargoDtlId { get; set; }
        public string CartingDate { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }
    }
}
