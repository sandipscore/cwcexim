using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class ContainerBackToTown
    {
        public string ContainerNo { get; set; }
        public int ID { get; set; }

        public string CFSCode { get; set; }

        public string OpCode { get; set; }
    }


    public class RefNoBackToTown
    {
        public string RefNo { get; set; }
        public int ID { get; set; }

       
    }
}