using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Bond.Models
{
    public class BondWOUnloading
    {
        public int UnloadingId { get; set; }
        public int DepositAppId { get; set; }
        public int SpaceAppId { get; set; }

        //[Display(Name = "Work Order No")]
        //[Required(ErrorMessage ="Fill Out This Field")]
        public string DepositNo { get; set; }
        //[Display(Name = "Work Order Date")]
        public string DepositDate { get; set; }

      //  [Required(ErrorMessage = "Fill Out This Field")]
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }

        [Display(Name = "WR No")]
        public string WRNo { get; set; }

        [Display(Name = "WR Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string WRDate { get; set; }

        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Sac No")]
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public string UnloadedDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal AreaOccupied { get; set; }        
        public string CargoDescription { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]+$",ErrorMessage = "Unloaded Units must be Integer")]
        public int UnloadedUnits { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal UnloadedWeights { get; set; }
        public int BalancedUnits { get; set; }
        public decimal BalancedWeights { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PackageCondition { get; set; }
        public string Remarks { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public int GodownId { get; set; }
        public decimal HandlingChargeManual { get; set; }
        public decimal HandlingChargeMechanical { get; set; }
        
        public string CfsCodes { get; set; } = "";
        public string Others { get; set; }
        public List<BondUnloadingCFSCode> CfsCodeList { get; set; }
        public List<DSRGodownWiseDtl> LstGodownWiseDtl { get; set; } = new List<DSRGodownWiseDtl>();
        public string GodownWiseLocXml { get; set; }

    }
    public class ListOfWOunloading
    {
        public int UnloadingId { get; set; }
        [Display(Name ="Bond No")]
        public string BondBOENo { get; set; }
        [Display(Name = "Bond Date")]
        public string BondBOEDate { get; set; }
        [Display(Name = "Deposit No ")]
        public string DepositNo { get; set; }
        [Display(Name = "Deposit Date")]
        public string DepositDate { get; set; }

        public string SACNo { get; set; }

        public string SACDate { get; set; }
    }
    public class ListOfBOENo
    {
        //public string WorkOrderNo { get; set; }
        //public int BondWOId { get; set; }
        public string DepositNo { get; set; }
        public int DepositAppId { get; set; }
    }
    public class BOENoDetails
    {
        public int BondWOId { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int SpaceAppId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string CargoDescription { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public string DepositDate { get; set; }

        public List<DSRGodownWiseDtl> LstGodownWiseDtl { get; set; } = new List<DSRGodownWiseDtl>();
    }
    public class GodownDetails
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class BondUnloadingCFSCode
    {
        public int Id { get; set; } = 0;
        public int UnloadingId { get; set; } = 0;
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; } = false;
    }
}