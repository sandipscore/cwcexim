using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VIZ_ExitThroughGateHeader
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

        public IList<VIZ_containerExit> containerList { get; set; } = new List<VIZ_containerExit>();

        public string shippingLineId { get; set; }
        public int GatePassId { get; set; }

        public string InvoiceNo { get; set; }
        public string Module { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string expectedTimeOfArrival { get; set; }

    }

    public class VIZ_containerExit
    {
        public string ContainerName { get; set; }

        public string shippingLine { get; set; }

        public string shippingLineId { get; set; }

        public string CFSCode { get; set; }
    }
}
