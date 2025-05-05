using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class StmtRefCont
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ShippingLineName { get; set; }
        public string ContainerType { get; set; }
        public string GateEntryDate { get; set; }
        public string GateExitDate { get; set; }
    }
}