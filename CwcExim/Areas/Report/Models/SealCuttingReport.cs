using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class SealCuttingReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string SealCuttingDate { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public int GodownID { get; set; }
        public string GodownName { get; set; }
 
    }
}