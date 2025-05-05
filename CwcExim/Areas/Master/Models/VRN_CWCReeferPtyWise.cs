using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_CWCReeferPtyWise
    {
        public int ReeferChrgId { get; set; }
        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }

        [Display(Name = "Electricity Charge")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal ElectricityCharge { get; set; }

        [Display(Name = "Sac Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }

        [Display(Name = "Container/CBT Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerSize { get; set; }
        public int PartyID { get; set; }
        public string PartyName { get; set; }
    }
}