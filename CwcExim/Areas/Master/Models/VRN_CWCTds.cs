using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_CWCTds
    {
        public int TdsId { get; set; }
        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }

        [Display(Name = "CWC TDS %")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal CWCTdsPrcnt { get; set; }

        [Display(Name = "H&T TDS %")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal HTTdsPrcnt { get; set; }

        [Display(Name = "SAC Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
    }
}