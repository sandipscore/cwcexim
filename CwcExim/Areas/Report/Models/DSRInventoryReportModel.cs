using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRInventoryReportModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public List<DSRInventoryReportContainer> LstInventoryReportContainer { get; set; } = new List<DSRInventoryReportContainer>();

       public List<DSRInvenontoryReportCargo> LstInventoryReportCargo { get; set; } = new List<DSRInvenontoryReportCargo>();
    }
    public class DSRInventoryReportContainer
    {
        public string CFSCode { get; set; }
        public string Party { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Commodity { get; set; }
        public string DaysAtCFS { get; set; }
    }
    public class DSRInvenontoryReportCargo
    {
        //public string CFSCode { get; set; }
        public string Party { get; set; }
        public string Cargo { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }
        public string DaysAtCFS { get; set; }
    }    
}