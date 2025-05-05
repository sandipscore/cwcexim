using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class Dnd_ExitThroughGateHeader
    {
        public int ExitIdHeader { get; set; }

        [Display(Name = "Gate Exit No")]
        public string GateExitNo { get; set; }

        [Display(Name = "Gate Pass No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GatePassNo { get; set; }

        [Display(Name = "Exit Date & Time")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GateExitDateTime { get; set; }

        [Display(Name = "Gate Pass Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GatePassDate { get; set; }

        public int Uid { get; set; }

        public int BranchId { get; set; }
        //public IList<ExitThroughGateDetails> listExitThroughGateDetails { get; set; } = new List<ExitThroughGateDetails>();

        public string StrExitThroughGateDetails { get; set; }

        public string Time { get; set; }

        public IList<Dnd_containerExit> containerList { get; set; } = new List<Dnd_containerExit>();

        public string shippingLineId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public int GatePassId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string VehicleNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string DriverName { get; set; }
       
        public string ContactNo { get; set; }

        public int ViaId { get; set; } = 0;
        public string Via { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public int PortId { get; set; } = 0;
        public string PortName { get; set; } = string.Empty;

        // public string CFSCode { get; set; }
        public string Module { get; set; }

    }
    public class Dnd_containerExit
    {
        public string ContainerName { get; set; }

        public string shippingLine { get; set; }

        public string shippingLineId { get; set; }

        public string CFSCode { get; set; }
    }

}