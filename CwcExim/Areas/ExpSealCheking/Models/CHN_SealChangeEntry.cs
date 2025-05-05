using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_SealChangeEntry
    {
        [Display(Name = "Job Order No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string JobOrderNo { get; set; }

        public string JobOrderDate { get; set; }

        [Display(Name = "Truck Slip No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

        [Display(Name = "Container/CBT No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "Present Seal No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PresentSealNo { get; set; }

        [Display(Name = "New Seal No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewSealNo { get; set; }

        [Display(Name = "Lock Provided")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public bool LockProvided { get; set; }


        public string JobOrderDetailsJS { get; set; }

        public int Uid { get; set; }

        public int EntryId { get; set; }

        public int JobOrderid { get; set; }
    }

    public class JobOrderList
    {
        public string JobOrderNo { get; set; }
    }
}