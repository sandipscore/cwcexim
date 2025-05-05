using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_teus_search
    {
        [Required(ErrorMessage ="Fill out this field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage ="Fill out this field")]
        public string PeriodTo { get; set; }
        public string PortName { get; set; }
        public string  ContainerNo { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public string CFSCode { get; set; }
        public string Type { get; set; }
    }
}