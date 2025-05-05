using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class PPGLocationV2
    {
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(50, ErrorMessage = "Location Name Can Be No More Than 50 Characters In Length Including Spaces")]
        [Display(Name = "Location Name:")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Country Name Should Contain Only Alphabets")]
        public string LocationName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(5, ErrorMessage = "Location Alias Can Be No More Than 5 Characters In Length Including Spaces")]
        [Display(Name = "Location Alias:")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Location Alias Should Contain Only Alphabets")]
        public string LocationAlias { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
    }
}