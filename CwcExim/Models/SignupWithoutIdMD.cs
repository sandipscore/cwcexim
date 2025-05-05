using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class SignupWithoutIdMD
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User ID can contain only alphabets, numeric digits and special characters '.' and '_'")]
        [StringLength(maximumLength: 10, MinimumLength = 4, ErrorMessage = "User ID must be minimum 4 characters long and maximum 10 characters long.")]
        [Display(Name ="User ID")]
        public string LoginId { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d._$#@-]+$", ErrorMessage = "Password must contain at least : 1 Lowercase character,1 Upper Case character and 1 numeric digit")]       
        //[StringLength(maximumLength: 20, MinimumLength = 8, ErrorMessage = "Password must be minimum 8 characters and maximum 20 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Compare(otherProperty: "Password", ErrorMessage = "Password and confirm password should be same")]
        public string ConfirmPassword { get; set; }

        // [Required(ErrorMessage = "*")]
        // public string GeneratedCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Mobile No. contain only 10 digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("[A-Z]{5}[(0-9)]{4}[A-Z]{1}", ErrorMessage = "Invalid PAN No.Or PAN No.always in Capital Letter")]
        public string PanNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must contain Alphabets only")]
        [StringLength(maximumLength: 100, ErrorMessage = "Name must not be more than 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]        
        [StringLength(maximumLength: 150, ErrorMessage = "Name must not be more than 150 characters.")]
       // [RegularExpression("^[A-Za-z0-9-/, ]+$", ErrorMessage = "Address can contain only alphabets, numeric digits and special characters ':','/' and '-'")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must contain only 6 numeric digits.")]
        [StringLength(maximumLength: 6, MinimumLength = 6, ErrorMessage = "Pincode contain only 6 digits")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]       
        [Display(Name ="District")]       
        public int DistrictId { get; set; }

    }
}