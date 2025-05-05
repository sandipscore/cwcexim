using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class WFLD_CartingRegister 
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string SpaceType { get; set; }
        public int IsShortCargo { get; set; }
        public List<WFLD_ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<WFLD_ShortCargoDetails>();
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
        //public int CHAEximTraderId { get; set; }
        [Display(Name = "CHA Name")]
        public string CHAName { get; set; }
        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }
        public int CartingAppId { get; set; }
        public string RegisterDate { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<WFLD_CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<WFLD_CartingRegisterDtl>();
        public IList<WFLDApplicationNoDet> lstAppNo { get; set; } = new List<WFLDApplicationNoDet>();
        public IList<WFLDGodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<WFLDGodownWiseLocation>();
        public string XMLData { get; set; }
        public string Time { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;
        public string SB { get; set; }
        public char IsGateIn { get; set; }
        public string ViewMode { get; set; }
    }
    public class WFLD_ShortCargoDetails 
    {
        public int ShortCargoDtlId { get; set; }
        public string CartingDate { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }
        public decimal ReservedSQM { get; set; }

        public decimal CBM { get; set; }

        public decimal ReservedCBM { get; set; }
        public decimal TotalSQM { get; set; }
        public string PkgType { get; set; }

        public int ShortGodownId { get; set; }
        public string ShortLocationIds { get; set; }
        public string ShortLocation { get; set; }

        public string ShortGodownName { get; set; }


    }

    public class WFLDApplicationNoDet
    {
        public string ApplicationNo { get; set; }
        public int CartingAppId { get; set; }
    }
    public class WFLDGodownWiseLocation
    {
        public int LocationId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string LocationName { get; set; } = "";
        
    }
}