using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class Vehicle
    {
        public int VehicleMasterId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(12,ErrorMessage = "Vehicle Number Can Not Be More Than 12 Characters In Length")]
        [Display(Name = "Vehicle Number:")]
        [RegularExpression(@"^[a-zA-Z0-9]+$",ErrorMessage ="Vehicle Name Should Contain Only Alphabets And Numbers")]
        public string VehicleNumber { get; set;}
        [Required(ErrorMessage ="Fill Out This Field")]
        [MaxLength(7,ErrorMessage = "Vehicle Weight Can Be No More Than 7 Digits")]
        [Display(Name = "Vehicle Weight:")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Vehicle Weight Should Contain Decimal Upto Two Decimal Places")]
        //  [RegularExpression(@"^[0-9]+.\d{0,2}$", ErrorMessage = "Vehicle Weight Should Contain Only Numbers")]
        public string VehicleWeight { get; set; }
        public int Uid { get; set; }
    }
}