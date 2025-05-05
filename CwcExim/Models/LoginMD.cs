using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class LoginMD
    {
        [Required(ErrorMessage = "Fill Out This Field")]
      //  [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User ID can contain only alphabets, numeric digits and special characters '.' and '_'")]
        [Display(Name = "User ID")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}