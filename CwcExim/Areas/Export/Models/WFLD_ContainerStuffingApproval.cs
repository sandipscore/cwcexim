using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_ContainerStuffingApproval
    { 
        public int StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }
        public string ContainerNo { get; set; } 
        public string Size { get; set; }
        public string CFSCode { get; set; }       
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
    }

}