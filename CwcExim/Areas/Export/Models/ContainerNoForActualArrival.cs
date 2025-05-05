using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class ContainerNoForActualArrival
    {
       
        [Required]
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        [Display(Name = "CFSCode")]   
        public string CFSCode { get; set; }
        [Display(Name = "Gate Pass No")]
        public string GatePassNo { get; set; }

    }
}