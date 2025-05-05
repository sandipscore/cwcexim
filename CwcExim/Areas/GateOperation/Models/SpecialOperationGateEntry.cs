using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class SpecialOperationGateEntry
    {
        public string ID { get; set; }
        public string ContainerNo { get; set; }
    }
    public class RefNoSpecialOperation
    {
        public string RefNo { get; set; }
        public int ID { get; set; }
        public string RefDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

    }
}