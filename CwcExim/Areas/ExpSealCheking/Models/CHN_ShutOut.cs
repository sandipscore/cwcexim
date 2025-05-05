using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_ShutOut
    {      
        public int ShutOutId { get; set; }

        [Display(Name = "Truck Slip No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }

        [Display(Name = "Truck Slip Date")]
        public string TruckSlipDate { get; set; }

        [Display(Name = "Container/CBT No")]
        public string ContainerNo { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Shut Out")]
        public bool ShutOut { get; set; }

        [Display(Name = "Reason")]
        [StringLength(5000, ErrorMessage = "Reason Cannot Be Longer Than 5000 Characters.")]
        public string Reason { get; set; }

        public int UId { get; set; }
    }
}