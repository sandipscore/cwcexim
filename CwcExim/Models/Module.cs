using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class Module
    {
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Module Name must contain only alphabets")]
        [StringLength(maximumLength: 30, ErrorMessage = "Module Name must not be more than 30 characters")]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Module Prefix must contain only alphabets")]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Module Prefix must contain 3 characters only")]
        [Display(Name = "Module Prefix")]
        public string ModulePrefix { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Module Fees must be numeric")]
        //[Range(typeof(int), "0", "99999", ErrorMessage = "Value must be between 0-99999")]
        //[Display(Name = "Module Fees")]
        //public int ModuleFees { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression(@"-?(0|([1-9]\d*))(\.\d+)?", ErrorMessage = "Fine% must contain decimal value only")]
        //[Range(typeof(decimal), "0", "100", ErrorMessage = "Value must be between 0-100")]
        //[Display(Name = "Fine(%)")]
        //public decimal FinePerct { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression(@"-?(0|([1-9]\d*))(\.\d+)?", ErrorMessage = "Rebate% must contain decimal value only")]
        //[Range(typeof(decimal), "0", "100", ErrorMessage = "Value must be between 0-100")]
        //[Display(Name = "Rebate(%)")]
        //public decimal RebatePerct { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Review Fees must be numeric")]
        //[Range(typeof(int), "0", "99999", ErrorMessage = "Value must be between 0-99999")]
        //[Display(Name = "Review Fees")]
        //public int ReviewFees { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Revision Fees must be numeric")]
        //[Range(typeof(int), "0", "99999", ErrorMessage = "Value must be between 0-99999")]
        //[Display(Name = "Revision Fees")]
        //public int RevisionFees { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Appeal Fees must be numeric")]
        //[Range(typeof(int), "0", "99999", ErrorMessage = "Value must be between 0-99999")]
        //[Display(Name = "Appeal Fees")]
        //public int AppealFees { get; set; }

        //[Required(ErrorMessage = "Fill out this field")]
        //public int? HighestApprovalAuthority { get; set; }

        //[Display(Name = "Approval Authority")]
        //public string HighestAppAuthName { get; set; }
        //public IEnumerable<DesignationMaster> HighestAppAuthList { get; set; }
        public int? CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedByName { get; set; }

        [Display(Name = "Updated On")]
        public string UpdatedOn { get; set; }
        public string IconFile { get; set; }

    }
}