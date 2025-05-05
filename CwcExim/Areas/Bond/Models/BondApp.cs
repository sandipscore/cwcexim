using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Bond.Models
{
    public class BondApp
    {
        public int SpaceappId { get; set; }
        [Display(Name ="Date")]
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
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "BOE Date")]
        public string BOEDate { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "Description of Cargo")]
        public string CargoDescription { get; set; }
        [Display(Name = "Nature Of Packages")]
        public string NatureOfPackages { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "No of Units")]
        public int NoOfUnits { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Nature of Material")]
        public int NatureOfMaterial { get; set; }
        [Display(Name = "Dimension Per Unit")]
        public decimal DimensionPerUnit { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name ="Requirement Of Space (in sq. mts.)")]
        public decimal SpaceReq { get; set; }
        [Display(Name = "Assessable/CIF Value")]
        public decimal AssessCIFvalue { get; set; }
        [Display(Name = "Duty Amount")]
        public decimal DutyAmt { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Expected Date of Warehouse")]
        public string ExpDateofWarehouse { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
        [Display(Name = "Importer Name")]
        public string ImporterName { get; set; }
    }
    public class ListOfBondApp
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
    public class CHA
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class Importer
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
    }
}