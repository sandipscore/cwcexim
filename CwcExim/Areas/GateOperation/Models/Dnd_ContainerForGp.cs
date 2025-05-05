using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class Dnd_ContainerForGp:containerAgainstGp
    {
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
        public string GPDate { get; set; }
        public int ViaId { get; set; } = 0;
        public string Via { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public int PortId { get; set; } = 0;
        public string PortName { get; set; } = string.Empty;
    }
}