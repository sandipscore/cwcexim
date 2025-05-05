using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class DSRExitThroughGateHeader
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
        
        public string StrExitThroughGateDetails { get; set; }

        public string Time { get; set; }

        public IList<DSRcontainerExit> containerList { get; set; } = new List<DSRcontainerExit>();

        public string shippingLineId { get; set; }
        public int GatePassId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string expectedTimeOfArrival { get; set; }

        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        public string Module { get; set; }

    }

    public class DSRcontainerExit
    {
        public string ContainerName { get; set; }

        public string shippingLine { get; set; }

        public string shippingLineId { get; set; }

        public string CFSCode { get; set; }
    }
    
}