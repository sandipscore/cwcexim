using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DndExportEIR
    {
        public int EIRId { get; set; } = 0;
        public string ContainerNo { get; set; }
        public string CFSCOde { get; set; }
        public string GateExitDate { get; set; }
        public string GateExitTime { get; set; }
        public string PortInDate { get; set;  }
        public string PortIntime { get; set; }
        public int PortOfLoadingid { get; set; } = 0;
        public string PortOfLoading { get; set; }
    }
}