using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public abstract class DistrictBase
    {
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-z A-Z0-9]+$", ErrorMessage = "District Name must contain Alphabets and Digits only")]
        [StringLength(maximumLength: 30, ErrorMessage = "Designation must not be more than 30 characters.")]
        [Display(Name ="District")]       
        public string DistrictName { get; set; }

        public bool ForKolkata { get; set; }
        public int CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedByName { get; set; }

        [Display(Name = "Updated On")]
        public string UpdatedOn { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        public int? BranchId { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }
       // public IEnumerable<WBDEDBranch> BranchList { get; set; }

    }
}