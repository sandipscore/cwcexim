using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.Export.Models
{
    public class Hdb_LoadContSF
    {                
        public int StuffingReqId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CFSCode { get; set; }
        public int ApprovalId { get; set; }

    }

}