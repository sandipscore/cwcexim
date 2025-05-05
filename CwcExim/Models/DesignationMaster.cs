using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class DesignationMaster
    {
        [Display(Name = "Sl No.")]
        public int GeneratedSerialNo { get; set; }
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Designation must contain only Alphabet")]
        [StringLength(maximumLength:50,ErrorMessage ="Designation must not be more than 50 characters.")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        public int? HigherAuthority { get; set; }

        [Display(Name ="Higher Authority")]
        public string HigherAuthorityName { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Approval Level must be numeric")]
        [Range(minimum:1,maximum:10)]
        [Display(Name = "Approval Level")]     
        public int ApprovalLevel { get; set; }
        public int CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedByName { get; set; }
        public string UpdatedOn { get; set; }
        public IEnumerable<DesignationMaster> HigherAuthorityList { get; set; }
    }
}