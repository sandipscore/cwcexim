using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_CartingRegister
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
        //public int CHAEximTraderId { get; set; }
        [Display(Name = "CHA Name")]
        public string CHAName { get; set; }
        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }
        public int CartingAppId { get; set; }
        public string RegisterDate { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<Dnd_CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<Dnd_CartingRegisterDtl>();
        public IList<ApplicationNoDet> lstAppNo { get; set; } = new List<ApplicationNoDet>();
        public IList<GodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<GodownWiseLocation>();
        public string XMLData { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;
        public string ShippingBill { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string SpaceType { get; set; }
        public string StorageType { get; set; }
        public int IsShortCargo { get; set; }
        public bool IsDirectStuffing { get; set; } = false;
        //public List<ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<ShortCargoDetails>();
        public List<Dnd_ShortCargoDetails> lstShortCargoDetails { get; set; } = new List<Dnd_ShortCargoDetails>();

    }
    public class Dnd_CartingRegisterDtl : CartingRegisterDtl
    {
        public decimal SQMReserved { get; set; } = 0;
        public decimal FoBValue1 { get; set; } = 0;
        public decimal RemUnit { get; set; } = 0;
        public decimal RemWeight { get; set; } = 0;
        public decimal RemFOB { get; set; } = 0;

    }

    public class Dnd_ShortCargoDetails
    {
        public int ShortCargoDtlId { get; set; }
        public string CartingDate { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }
        public decimal RemUnit { get; set; } = 0;
        public decimal RemWeight { get; set; } = 0;
        public decimal RemFOB { get; set; } = 0;
    }
}