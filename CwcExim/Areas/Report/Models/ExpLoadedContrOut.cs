using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class ExpLoadedContrOut
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
    }

    public  class LoadedContrOutList
    {
        public string ShippingBillNo { get; set; }
        public string Commodity { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Seal { get; set; }
        public string Weight { get; set; }

    }
}