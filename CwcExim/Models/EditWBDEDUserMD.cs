using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class EditWBDEDUserMD
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Fill out this field  ")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must contain Alphabets only")]
        [StringLength(maximumLength: 50, ErrorMessage = "Name must not be more than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<RoleMaster> RoleList { get; set; }

        //[Required(ErrorMessage = "Fill out this field ")]
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public IEnumerable<DesignationMaster> DesignationList { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        public bool IsBlocked { get; set; }
    }
}