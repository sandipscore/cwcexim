using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class ChangePassword
    {
        public int Uid { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Old Password")]
        public string OldPassword{get;set;}

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Compare(otherProperty: "NewPassword", ErrorMessage = "NewPassword and confirm password should be same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}