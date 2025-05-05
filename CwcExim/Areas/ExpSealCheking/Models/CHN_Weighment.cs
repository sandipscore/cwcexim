using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_Weighment
    {
        public int WeighmentId { get; set; }

        public int Uid { get; set; }

        [Display(Name = "Truck Slip No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }

        [Display(Name = "Truck Slip Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipDate { get; set; }

        [Display(Name = "Container/CBT No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "On Wheel Inspection")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string OnWheelInspection { get; set; }

        [Display(Name = "Gross Weight")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Gross Weight Cannot Be More Than 9999999999999999.99")]
        public decimal GrossWeight { get; set; }

        [Display(Name = "Tare Weight")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Tare Weight Cannot Be More Than 9999999999999999.99")]
        public decimal TareWeight { get; set; }

        public string TruckSlipDetails { get; set; }

        public int SealChangeEntryId { get; set; }

        public int BranchId { get; set; }

        public string CFSCode { get; set; }
    }

    public class TruckSlipForWeighment
    {
        public string TruckSlipNo { get; set; }
    }
}