using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_CartingRegister
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
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        public int CartingAppId { get; set; }
        public string RegisterDate { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<Hdb_CartingRegisterDtl> lstRegisterDtl { get; set; } = new List<Hdb_CartingRegisterDtl>();
        public IList<Hdb_ApplicationNoDet> lstAppNo { get; set; } = new List<Hdb_ApplicationNoDet>();
        public IList<Hdb_GodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<Hdb_GodownWiseLocation>();
        public string XMLData { get; set; }
        public string ClearLocation { get; set; }

        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;
        public string RequestDate { get; set; }

        public string SBNo { get; set; }

        public string SBDate { get; set; }

    }

    public class Hdb_ApplicationNoDet
    {
        public string ApplicationNo { get; set; }
        public int CartingAppId { get; set; }
    }
    public class Hdb_GodownWiseLocation
    {
        public int LocationId { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
       
    }

    public class PrintCartingReg
    {
        public string CartingRegisterNo { get;set;}
        public string RegisterDate { get; set; }
      //  public string CartingRegNo { get; set; }
        public string ShipBillNo { get; set; }
        public string Exporter { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public decimal FobValue { get; set; }
        public decimal Weight { get; set; }
        public int ActualUnits { get; set; }
        public int SBUnits { get; set; }
        public int BalanceUnits { get; set; }

    }
    public class Hdb_BTTCartingRegister
    {
        public int CartingRegisterId { get; set; }

        [Display(Name = "Carting Register No")]
        public string CartingRegisterNo { get; set; }

        [Display(Name = "Container No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        [Display(Name = "CartingType")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CartingType { get; set; }
        [Display(Name = "CommodityType")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CommodityType { get; set; }
        //public int CHAEximTraderId { get; set; }
        [Display(Name = "CHA Name")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }
        public string RegisterDate { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<Hdb_BTTCartingRegisterDtl> lstRegisterDtl { get; set; } = new List<Hdb_BTTCartingRegisterDtl>();
        public IList<Hdb_GodownWiseLocation> lstGdnWiseLctn { get; set; } = new List<Hdb_GodownWiseLocation>();
        public string XMLData { get; set; }
        public int GodownId { get; set; } = 0;
        public int CHAId { get; set; } = 0;
        public string OldCFSCode { get; set; }
    }
    public class Hdb_BTTCartingRegisterDtl
    {
        public int CartingAppDtlId { get; set; } = 0;
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
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
        public string Storage { get; set; }
    }
}