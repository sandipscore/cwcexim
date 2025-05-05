using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CwcExim.Models
{
    public class ForgotPasswordMD
    {
        //[Required(ErrorMessage = "*")]
        [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User ID can contain only alphabets, numeric digits and special characters '.' and '_'")]      
        public string LoginId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No. must contain only 10 numeric digits.")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Mobile No. contain only 10 digits")]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
     
    }
}