using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRBondApp
    {
        public int SpaceappId { get; set; }
        [Display(Name = "Date")]
        public string ApplicationDate { get; set; }
        [Display(Name = "Application No")]
        public string ApplicationNo { get; set; }
        public int CHAId { get; set; }
        public int ImporterId { get; set; }
        [Display(Name = "Importer License No")]
        public string ImporterLicenseNo { get; set; }
        [Display(Name = "BOL/AWBNo")]
        public string BOLAWBNo { get; set; }
        [Display(Name = "BOE No")]
        public string BOENo { get; set; }

        [Display(Name = "BOE Date")]
        public string BOEDate { get; set; }
        
        public string GodownWiseDtlXml { get; set; }
        
        [Display(Name = "IGM No")]
        public string IGMNo { get; set; }

        [Display(Name = "IGM Date")]
        public string IGMDate { get; set; }



        public int CommodityId { get; set; } // 'CommodityId from mstcommodity',
        public string CommodityName { get; set; }

        // [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(500, ErrorMessage = "Description cannot be greater than 500 Character.")]
        [Display(Name = "Description of Cargo")]

        public string CargoDescription { get; set; }
        [Display(Name = "Nature Of Packages")]
        public string NatureOfPackages { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "No of Units")]
        [RegularExpression(@"^[1-9][0-9]*$",
        ErrorMessage = "Value must be number greater than 0")]
        public int NoOfUnits { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Nature of Material")]
        public int NatureOfMaterial { get; set; }
        [Display(Name = "Dimension Per Unit")]
        public decimal DimensionPerUnit { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$",
        ErrorMessage = "Value must be number greater than 0")]
        public decimal Weight { get; set; }
        //[Range(3, 1000000000, ErrorMessage = "Minimum space 3 Sq. mts")]
        [Display(Name = "Requirement Of Space Unreserved (in sq. mts.)")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal SpaceReq { get; set; }
        [Display(Name = "Requirement Of Space Reserved (in sq. mts.)")]
        public decimal SpaceReqReserved { get; set; }
        [Display(Name = "Assessable/CIF Value")]
        public decimal AssessCIFvalue { get; set; }
        [Display(Name = "Duty Amount")]
        public decimal DutyAmt { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Expected Date of Warehouse")]
        public string ExpDateofWarehouse { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
        [Display(Name = "Importer Name")]
        public string ImporterName { get; set; }

        public string StorageType { get; set; }
        public bool IsInvoiceCopy { get; set; }
        public bool IsPackingList { get; set; }
        public bool IsBOLAWB { get; set; }
        public bool IsBOE { get; set; }
        public bool OnlyForReserved { get; set; } 
        public string Others { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }

        public string BolAwbDate { get; set; }
        public string AWBNo { get; set; }
        public string AWBDate { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string CustomSealNo { get; set; }
        public string DimensionUOM { get; set; }
        public string WeightUOM { get; set; }
        public string SpaceType { get; set; }
        public string AreaType { get; set; }
        public List<DSRGodownWiseDtl> LstGodownWiseDtl { get; set; } = new List<DSRGodownWiseDtl>(); 
}

    public class DSRListOfBondApp
    {
        public int SpaceappId { get; set; }
        [Display(Name = "Application No")]
        public string ApplicationNo { get; set; }
        [Display(Name = "Importer License No")]
        public string ImporterLicenseNo { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
        [Display(Name = "Importer")]
        public string ImporterName { get; set; }
        public string BOENo { get; set; }
    }
}