using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DND_ContainerStatus
    {
       


     
            public int ShippingLineId { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            public string ShippingLineName { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            public string ContainerNo { get; set; }
            public string Size { get; set; } = "";
        }

        public class DND_TrackContainerStatusList
        {
            public string LineNo { get; set; }
            public string Rotation { get; set; }
            public string AppraisementDate { get; set; }
            public string DestuffingDate { get; set; }
            public string StuffingDate { get; set; }
            public string GatePassDate { get; set; }
            public string GatePassNo { get; set; }
            public string GateExitDate { get; set; }
            public string GateEntryDate { get; set; }
            public string ContainerNo { get; set; }
            public string JobOrderDate { get; set; }
            public string GodownName { get; set; }
            public string Location { get; set; }
            public string Size { get; set; } = "";
            public string ShippingLineName { get; set; } = "";
            public string CFSCode { get; set; } = "";
            public string ContainerClass { get; set; }
            public string ExportType { get; set; }
            public string VehicleNo { get; set; }
            public string CustomSeal { get; set; }
            public string ShippingSeal { get; set; }
            public string POL { get; set; }
            public string PODestination { get; set; }
    }
        public class DND_TrackContainer
        {
            public string ContainerNo { get; set; }

        }

        public class DND_ICDList
        {
            public string ICDCode { get; set; }

        }

        public class DND_ShippingLineList
        {
            public string ShippingLineName { get; set; }
            public int ShippingLineId { get; set; }
        }
    }

