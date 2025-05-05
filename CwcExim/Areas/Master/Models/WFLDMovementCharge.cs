using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDMovementCharge
    {
        public int MovementChargeId { get; set; }

        //[Display(Name = "By")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        //public string MovementBy { get; set; }

        [Display(Name = "Origin")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Origin { get; set; }

        [Display(Name = "Via")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string MovementVia { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "Cargo Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate must be  less than 99999999.99")]
        public decimal Rate { get; set; }

        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        public int UserId { get; set; }
    }
}