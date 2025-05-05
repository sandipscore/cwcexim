using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class Weighment
    {
        public int WeightmentId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }

        public string CFSCode { get; set; }
        public int VehicleMasterId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Truck")]
        public string VehicleNumber { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name ="Date")]
        public string WeightmentDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Weight")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Empty Weight Cannot Be More Than 9999999999999999.99")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Empty Weight")]
        [Range(0,9999999999999999.99,ErrorMessage = "Empty Weight Cannot Be More Than 9999999999999999.99")]
        public decimal EmptyWeight { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string OperationType { get; set; }
    }
}