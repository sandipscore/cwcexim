using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class StuffingRequest
    {
        [Required(ErrorMessage ="Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string ToDate { get; set; }
    }

    public class StuffingRequestList
    {
        public string ShippingBillNo { get; set; }
        public string Exporter { get; set; }
        public string ShippingLine { get; set; }
        public string Units { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
    }
}