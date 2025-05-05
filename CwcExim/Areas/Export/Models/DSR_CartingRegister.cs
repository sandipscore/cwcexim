using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class DSR_CartingRegister
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingBillNo { get; set; }
        public string SpaceType { get; set; }
        public int IsShortCargo { get; set; }
        public List<DSR_ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<DSR_ShortCargoDetails>();
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
        public IList<DSR_CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<DSR_CartingRegisterDtl>();
        public IList<DSRApplicationNoDet> lstAppNo { get; set; } = new List<DSRApplicationNoDet>();
        public IList<DSRGodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<DSRGodownWiseLocation>();
        public string XMLData { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;

    }
    public class DSR_ShortCargoDetails 
    {
        public int ShortCargoDtlId { get; set; }
        public string CartingDate { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }
        public decimal ReservedSQM { get; set; }
        public decimal TotalSQM { get; set; }
        public decimal ManualWT { get; set; }
        public decimal MechanicalWT { get; set; }

        public string modalGodownName { get; set; }

        public int modalGodownId { get; set; }
    }

    public class DSRApplicationNoDet
    {
        public string ApplicationNo { get; set; }
        public int CartingAppId { get; set; }
    }
    public class DSRGodownWiseLocation
    {
        public int LocationId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string LocationName { get; set; } = "";
        
    }
}