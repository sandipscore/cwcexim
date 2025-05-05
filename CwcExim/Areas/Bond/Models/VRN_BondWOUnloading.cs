using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class VRN_BondWOUnloading
    {
        public string BondNo { get; set; }
        public string BondDate { get; set; }
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
        [Required(ErrorMessage = "Fill Out This Field")]
        public string UnloadedDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal AreaOccupied { get; set; }
        public string CargoDescription { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Unloaded Units must be Integer")]
        public int UnloadedUnits { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal UnloadedWeights { get; set; }
        public int BalancedUnits { get; set; }
        public decimal BalancedWeights { get; set; }
        public string PackageCondition { get; set; }
        public string Remarks { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public int GodownId { get; set; }

        public string CfsCodes { get; set; } = "";
        public List<BondUnloadingCFSCode> CfsCodeList { get; set; }

    }
    public class VRN_ListOfBOENo
    {
        //public string WorkOrderNo { get; set; }
        //public int BondWOId { get; set; }
        public string BondNo { get; set; }
        public int DepositAppId { get; set; }
    }


    public class VRN_BondWOUnloadingPDF
    {
        public string DepositNo { get; set; }
        public string UnloadingDate { get; set; }
        public int UnloadingId { get; set; }
        public string GodownNo { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal Area { get; set; }
        public string PkgCondition { get; set; }
        public string Remarks { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }

    }
    public class VRN_BOENoDetails
    {
        public decimal AreaReserved { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }

        public string DepositNo { get; set; }
        public string Remarks { get; set; }
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
    }
}