using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class DSRGateExitFactoryStuffing
    {
        public int ExitIdHeader { get; set; }

        [Display(Name = "Gate Exit No")]
        public string GateExitNo { get; set; }

        

        [Display(Name = "Exit Date & Time")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GateExitDateTime { get; set; }

        [Display(Name = "Request No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FSRequestNo { get; set; }

        [Display(Name = "Request Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FSRequestDate { get; set; }
        
        public int Uid { get; set; }

        public int BranchId { get; set; }
        
        public string StrExitThroughGateDetails { get; set; }

        public string Time { get; set; }

        public IList<DSRcontainerExitFS> containerList { get; set; } = new List<DSRcontainerExitFS>();

        public string shippingLineId { get; set; }
        public int FSRequestId { get; set; }
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }

    }

    public class DSRcontainerExitFS
    {
        public int ExitIdHeader { get; set; }
        public string ContainerNo { get; set; }
        public string ShippingLine { get; set; }
        public string ShippingLineId { get; set; }
        public string CFSCode { get; set; }
        public int IsReefer { get; set; }
        public string size { get; set; }    
        public string CargoDescription { get; set; }
        public string CargoType { get; set; }
        public string NoOfUnits { get; set; }
        public string Weight { get; set; }
        public string OperationType { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string DepositorName { get; set; }
        public string FSRequestNo { get; set; }
        public string FSRequestDate { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public int ExitIdDtls { get; set; }

    }

}