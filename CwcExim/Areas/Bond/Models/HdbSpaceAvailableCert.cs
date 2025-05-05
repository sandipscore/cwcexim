using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Bond.Models
{
    public class HdbSpaceAvailableCert:HdbBondApp
    {
        //[Required(ErrorMessage ="Fill Out This Field")]
        // [StringLength(30,ErrorMessage = "Sac No Cannot Be More Than 30 Characters In Length")]
        public string SacNo { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string SacDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Area Reserved Cannot Be More Than 99999999.99")]
        public decimal AreaReserved { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ValidUpto { get; set; }
        public int IsApproved { get; set; }
        public int IsSubmitted { get; set; }
        public int ApprovedBy { get; set; }
        public string NextApprovalDate { get; set; }
    }

    public class HdbSpaceAvailCertPdf
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string BOLAWBNo { get; set; }
        public string BOENo { get; set; }
        public string BOENoDate { get; set; }
        public decimal AreaReserved { get; set; }
        public string ValidUpto { get; set; }
        public string Importer { get; set; }
        public string CHAName { get; set; }
        public string BOLAWBDate { get; set; }
        public string NextApprovalDate { get; set; }
        public string AWBNo { get; set; }
        public string AWBDate { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyAddress { get; set; }
        public string Location { get; set; }

    }


    public class HdbSpaceAvailAppCertPdf
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string BOLAWBNo { get; set; }
        public string BOLAWBNoDate { get; set; }
        public string BOENoDate { get; set; }
        public string AWBNoDate { get; set; }
      
        public decimal AreaReserved { get; set; }
        public string ValidUpto { get; set; }
        public string Importer { get; set; }
        public string CHAName { get; set; }
        public string SysDt { get; set; }
        public string ImportLicenseNo { get; set; }
        public string CargeDescrip { get; set; }
        public int Unit { get; set; }
        public string Packages { get; set; }
        public decimal DUnit { get; set; }
        public decimal Weight { get; set; }
        public decimal CIF { get; set; }

        public decimal Duty { get; set; }

        public string NatureMaterial { get; set; }

        public string Encls { get; set; }


        public string ApplicationNo { get; set; }
        public string ExpDateofWarehouse { get; set; }
        public string OthersValue { get; set; }

        public string WeightUOM { get; set; }
        public string DimensionUOM { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyAddress { get; set; }

        //public string BOLAWB { get; set; }
        //public string BOE { get; set; }


    }


}
