using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class AMDReefer
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
        public decimal? MonitoringCharge { get; set; }
    }
}