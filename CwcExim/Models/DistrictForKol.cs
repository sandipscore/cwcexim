using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class DistrictForKol:DistrictBase
    {
        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Region Code must be alphanumeric only and must not contain any Space")]
        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pin Code must be numeric only")]
        [StringLength(maximumLength:6,MinimumLength =6)]
        [Display(Name = "Pin Code")]
        public string PinCode { get; set; }

       
    }
}