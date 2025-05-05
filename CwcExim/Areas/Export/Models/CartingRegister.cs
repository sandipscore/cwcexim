using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class CartingRegister
    {
        public int CartingRegisterId { get; set; }

        [Display(Name = "Carting Register No")]
        public string CartingRegisterNo { get; set; }

        [Display(Name = "Application No")]
        [Required(ErrorMessage ="Fill Out This Field")]
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
        public IList<CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<CartingRegisterDtl>();
        public IList<ApplicationNoDet> lstAppNo { get; set; } = new List<ApplicationNoDet>();
        public IList<GodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<GodownWiseLocation>();
        public string XMLData { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;
    }
    public class ApplicationNoDet
    {
        public string ApplicationNo { get; set; }
        public int CartingAppId { get; set; }
    }
    public class GodownWiseLocation
    {
        public int LocationId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string LocationName { get; set; }= "";
       // public bool IsOccupied { get; set; }
    }
}