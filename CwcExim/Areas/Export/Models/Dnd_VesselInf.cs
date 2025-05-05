using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.Export.Models
{
    public class Dnd_VesselInf
    {
        public int VesselId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(50, ErrorMessage = "VIA Can Be No More Than 50 Characters In Length Including Spaces")]
        [Display(Name = "VIA:")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "VIA Should Contain Only Alphabets")]
        public string VIA { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(50, ErrorMessage = "Vessel Can Be No More Than 50 Characters In Length Including Spaces")]
        [Display(Name = "Vessel:")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Vessel Should Contain Only Alphabets")]
        public string Vessel { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
    
        public int PortOfLoadingId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
       
        public string PortOfLoadingName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
      
        public string ETA { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
    
        public string ETD { get; set; }
        [Required]
        public string CutOfTime { get; set; }
        public string IPAddress { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string Updatedon { get; set; }
        public string EntryDateTime { get; set; }
        public string SystemDateTime { get; set; }
        public string Time { get; set; }
    }
}