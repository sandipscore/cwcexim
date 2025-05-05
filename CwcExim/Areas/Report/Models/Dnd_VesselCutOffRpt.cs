using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class Dnd_VesselCutOffRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public int PortId { get; set; }
        public string PortName { get; set; }
        public string ViaNo { get; set; }
        public string VesselName { get; set; }
        public string CutOffDate { get; set; }
        public string CutOffTime { get; set; }
        public string OpenDate { get; set; }
        public string DepartureDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        public List<Dnd_VesselCutOffRpt> LstVessel { get; set; } = new List<Dnd_VesselCutOffRpt>();
    }
  
   
}