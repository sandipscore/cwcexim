using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class RoleMaster
    {
        [Display(Name ="Role ID")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Role Name must contain only alphabets")]
        [StringLength(maximumLength:50,ErrorMessage ="Role Name must not be more than 50 characters")]
        [Display(Name ="Role")]
        public string RoleName { get; set; }
        public int CreatedBy { get; set; }

        [Display(Name ="Created By")]
        public string CreatedByName { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedByName { get; set; }

        [Display(Name = "Updated On")]
        public string UpdatedOn { get; set; }
       
    }
}