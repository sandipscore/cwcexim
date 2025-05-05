using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_CWCWeighmentPtyWise
    {
        private decimal _TruckRate = 0;
        public int WeighmentId { get; set; }
        //[Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "Container Rate")]
        [Range(minimum: 0, maximum: 99999999.99, ErrorMessage = "Container Rate must be  less than 99999999.99")]
        public decimal ContainerRate { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Size")]
        //[Range(minimum: 0.1, maximum: 99999999.99, ErrorMessage = "Truck Rate must be  less than 99999999.99")]
        public string ContainerSize { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "SAC Code")]
        public string SacCode { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Truck Rate")]
        [Range(minimum: 0, maximum: 99999999.99, ErrorMessage = "Truck Rate must be  less than 99999999.99")]
        public decimal? TruckRate { get; set; }
        public int PartyID { get; set; }
        public string PartyName { get; set; }
    }
}