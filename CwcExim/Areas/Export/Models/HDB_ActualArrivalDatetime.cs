using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class HDB_ActualArrivalDatetime
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        [Display(Name = "CFSCode")]
        public string CFSCode { get; set; }


        [Display(Name = "Gate Pass No")]
        public string GatePassNo { get; set; }

        [Required(ErrorMessage = "Please Check Reefer")] 
        public bool Reefer { get; set; }



        [Display(Name = "Arrival Date Time")]
        public string ArrivalDateTime { get; set; }
        public string CreatedBy { get; set; }
    }
}