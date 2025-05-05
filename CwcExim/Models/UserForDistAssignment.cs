using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class UserForDistAssignment
    {
        public int Uid { get; set; }


       
        public string LoginId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage ="Fill out this field")]
        public bool IsSelected { get; set; }
    }
}