using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class PPGCoreData
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public decimal ICDCash { get; set; }
        public decimal ICDBill { get; set; }
        public decimal ICDTotal { get; set; }
        public decimal DESSCash { get; set; }
        public decimal DESSTotal { get; set; }
        public decimal CFSCash { get; set; }
        public decimal CFSTotal { get; set; }
        public decimal IRRCash { get; set; }
        public decimal IRRTotal { get; set; }
        public decimal GrossCash { get; set; }
        public decimal GrossBill { get; set; }
        public decimal GrossTotal { get; set; }


    }
}