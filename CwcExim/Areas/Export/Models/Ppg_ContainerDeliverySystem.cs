using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_ContainerDeliverySystem
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int Id { get; set; }
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string PartyName { get; set; }
        public string ActualTimeOfArrival { get; set; }
        public string PartyId { get; set; }


    }
    public class Ppg_containerList {
    public string ContainerNo { get; set; }
    public string CFSCode { get; set; }
    public int GatePassId { get; set; }
    }
}