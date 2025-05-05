using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_SealClosingReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Date { get; set; }

        public string ContainerNo { get; set; }
        public string Size { get; set; }

        public string Commodity { get; set; }

        public string ShippingLine { get; set; }

        public string CHAOrPort { get; set; }

        public string Weight { get; set; }


        public string value { get; set; }

        public string LCLFCL { get; set; }


    }
}