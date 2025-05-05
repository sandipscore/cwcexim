using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class OblSbinformation
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

       

        public string Ref { get; set; }


        public string Date { get; set; }

        public string OBLNo { get; set; }

        public string BEO { get; set; }


        public string BolDate { get; set; }
        public string FobValue { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string ImporterName { get; set; }

        public string CHAName { get; set; }
        public string Duty { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }

    }
}