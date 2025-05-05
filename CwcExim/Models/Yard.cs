using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class Yard
    {
        public int YardId { get; set; }

        [Display(Name = "Yard Name")]
        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(30,ErrorMessage = "Yard Name Cannot Be More Than 30 Characters")]
        [RegularExpression(@"^[a-zA-Z0-9\- ]+$",ErrorMessage = "Yard Name Can Contain Alphabets,Numeric Digits And Special Characters '-'")]
        public string YardName { get; set; }
        public int Uid { get; set; }
    }
}