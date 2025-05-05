using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRInsurance
    {
        public int InsuranceId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Charge Should Be Less Than Or Equal To 99999999.99")]
        public decimal Charge { get; set; }

        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        public int Uid { get; set; }

        [Display(Name = "SAC Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
    }
}