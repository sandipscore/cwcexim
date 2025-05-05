using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class WbdedForcedUserMD
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User ID can contain only alphabets, numeric digits and special characters '.' and '_'")]
        [StringLength(maximumLength: 10, MinimumLength = 4, ErrorMessage = "User ID must be minimum 4 characters long and maximum 10 characters long.")]
        [Display(Name = "User ID")]
        public string LoginId { get; set; }
           
        [StringLength(maximumLength: 30, ErrorMessage = "Registration No contains maximum 30 character")]
        [Display(Name = "Registration No")]
        public string RegistrationNo { get; set; }

        
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Mobile No. contain only 10 digits")]
        public string MobileNo { get; set; }

        
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
       

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must contain Alphabets only")]
        [StringLength(maximumLength: 100, ErrorMessage = "Name must not be more than 100 characters.")]
        public string Name { get; set; }
        
        [StringLength(maximumLength: 150, ErrorMessage = "Name must not be more than 150 characters.")]      
        public string Address { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must contain only 6 numeric digits.")]
        [StringLength(maximumLength: 6, MinimumLength = 6, ErrorMessage = "Pincode contain only 6 digits")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
    }
}