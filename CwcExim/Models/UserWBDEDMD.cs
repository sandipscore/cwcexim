using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CwcExim.Models
{
    public class UserWBDEDMD
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User ID can contain only alphabets, numeric digits and special characters '.' and '_'")]
        [StringLength(maximumLength: 10,MinimumLength =4, ErrorMessage = "User ID must be minimum 4 characters long and maximum 10 characters long.")]
        [Display (Name ="User ID")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must contain Alphabets only")]
        [StringLength(maximumLength: 50, ErrorMessage = "Name must not be more than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d._$#@-]+$",
        // ErrorMessage = "Password must contain at least : 1 Lowercase character, 1 Upper Case character and 1 numeric digit. Special Character is optional and only specified ones are allowed : '@ # . _ - $' ")]
        //[StringLength(maximumLength: 20, MinimumLength = 8, ErrorMessage = "Password must be minimum 8 characters long and maximum 20 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<RoleMaster> RoleList { get; set; }

        //[Required(ErrorMessage = "Fill out this field ")]
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public IEnumerable<DesignationMaster> DesignationList { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        [StringLength(maximumLength:10,MinimumLength =10,ErrorMessage ="Mobile No. must contain only 10 numeric digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Fill out this field ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
        
    }
}