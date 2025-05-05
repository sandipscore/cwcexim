using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DepositApp
    {
        public int DepositAppId { get; set; }
        public int SpaceappId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacNo { get; set; }
        public string SacDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string DepositDate { get; set; }
        public string DepositNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(45, ErrorMessage = "Bond/BOEDate Cannot Be More Than 45 Characters")]
        public string BondBOENo { get; set; }

        //   [Required(ErrorMessage = "Fill Out This Field")]
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
    }
    public class PrintDA
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
        public List<SACDet> lstSAC { get; set; } = new List<SACDet>();

    }
    public class SACDet
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string ImporterName { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
    }
}