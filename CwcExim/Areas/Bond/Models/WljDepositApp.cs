using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class WljDepositApp
    {

        public int DepositAppId { get; set; }
        public int SpaceappId { get; set; }

        public int EntryId { get; set; }
        public string EntryDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacNo { get; set; }
        public string SacDate { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string DepositDate { get; set; }
        public string DepositNo { get; set; }

        //    [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(45, ErrorMessage = "Bond/BOEDate Cannot Be More Than 45 Characters")]
        public string BondBOENo { get; set; }

        // [Required(ErrorMessage = "Fill Out This Field")]
        public string BondBOEDate { get; set; }
        public string WRDate { get; set; }

        [StringLength(45, ErrorMessage = "WRNo Cannot Be More Than 45 Characters")]
        public string WRNo { get; set; }
        public int GodownId { get; set; }
        public int ImporterId { get; set; }

        [StringLength(1000, ErrorMessage = "Cargo Description Cannot Be More Than 1000 Characters")]
        public string CargoDescription { get; set; }
        public int Units { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "Weight Cannot Be More Than 99999999.99")]
        public decimal Weight { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "Weight Cannot Be More Than 99999999.99")]
        public decimal Value { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "Weight Cannot Be More Than 99999999.99")]
        public decimal Duty { get; set; }

        [StringLength(1000, ErrorMessage = "Remarks Cannot Be More Than 1000 Characters")]
        public string Remarks { get; set; }
        public bool IsInsured { get; set; }
        public string CHA { get; set; }
        public string Importer { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string BondNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string BondDate { get; set; }
        public string CustomBondNo { get; set; }
        public string CustomBondDate { get; set; }

        public string CustomSealNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string VehicleNo { get; set; }


    }

    public class VehicleForWljBondDeposit
    {
        public string VehicleNo { get; set; }
        public int EntryId { get; set; }
        public int SpaceappId { get; set; }
    }

    public class WljPrintDA
    {
        public string DepositeNo { get; set; }
        public string DepositeDate { get; set; }
        public string BondNo { get; set; }
        public string BondDt { get; set; }
        public string WRNo { get; set; }
        public string WRDt { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string GodownName { get; set; }
        public List<WljSACDet> lstSAC { get; set; } = new List<WljSACDet>();
        public List<WljGodDet> lstGodown { get; set; } = new List<WljGodDet>();
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }

    }
    public class WljSACDet
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string ImporterName { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
    }


    public class WljGodDet
    {
        public string GodownName { get; set; }
        public decimal AreaReserved { get; set; }
        public string CargoDesc { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public string Remarks { get; set; }

        public string NatureOfPackages { get; set; }
    }
}