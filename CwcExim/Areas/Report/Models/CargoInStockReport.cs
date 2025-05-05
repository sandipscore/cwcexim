using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CargoInStockReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Date { get; set; }
        public string BOEorBl { get; set; }

        public string Party { get; set; }

        // public string ContainerNo { get; set; }


        public string Commodity { get; set; }

        public string NoOfPackage { get; set; }

        public string WT { get; set; }

        public string GoDown { get; set; }

        public string Location { get; set; }

        public string CHAName { get; set; }
                                           //public string value { get; set; }
    }

   

}