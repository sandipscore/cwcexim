using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class DistrictForNonKol:DistrictBase
    {
       // [Required(ErrorMessage = "Please enter District Code")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "CESC District Code must be alphanumeric only")]
        [StringLength(maximumLength: 10)]
        [Display(Name ="CESC District Code:")]
        public string CESCDistrictCode { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Non-CESC District Code must be alphanumeric only")]
        [StringLength(maximumLength: 10)]
        [Display(Name = "Non CESC District Code:")]
        public string NonCESCDistrictCode { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [StringLength(maximumLength: 150)]
        [Display(Name = "DM Office Address:")]
        public string DmOfficeAddress { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        [EmailAddress(ErrorMessage ="Invalid Email Address:")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "DM Office Email Id:")]
        public string DmOfficeEmail { get; set; }
      
    }
}