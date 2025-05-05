using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_StuffingPendency
    {
        public string StuffingReqNo { get; set; } 
        public string StuffingReqDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public string InDate { get; set; }
        public int NoOfSbs { get; set; }
        public decimal NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal FOB { get; set; }
        public string Via { get; set; }
        public string Vessel { get; set; }
        public string Godown { get; set; }
    }
}