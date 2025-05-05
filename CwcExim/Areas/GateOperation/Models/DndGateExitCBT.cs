using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class DndGateExitCBT
    {
        public int ExitId { get; set; }

        public string GateExitNo { get; set; }

        public string GateExitDateTime { get; set; }
        public string GateInDate { get; set; }

        public string CBTNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
        public string Time { get; set; }
        public string GateInTime { get; set; }
    }

    public class CBTList
    {
        public string CBTNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string GateInDate { get; set; }
        public string GateInTime { get; set; }

    }
}