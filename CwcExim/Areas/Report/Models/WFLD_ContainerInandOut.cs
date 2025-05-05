using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ContainerInandOut
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string VehicleNo { get; set; }
        public string Size { get; set; }
        public string OperationType { get; set; }
        public string EntryDateTime { get; set; }
        public string GateExitDateTime { get; set; }
        public string CBTCONT { get; set; }
    }
}