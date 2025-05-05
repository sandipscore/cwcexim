using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class JobOrderSheet
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int SlNo { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string EximTraderName { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceDate { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public int NoOfContainer { get; set; }
    }
}