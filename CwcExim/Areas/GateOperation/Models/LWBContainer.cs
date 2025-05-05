using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class LWBContainer
    {
        public int Id { get; set; } = 0;
        public string LWBDate { get; set; }
        public string GateInDate { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }

    }
}