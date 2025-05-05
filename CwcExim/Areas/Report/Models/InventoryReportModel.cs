using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class InventoryReportModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public List<InventoryReportContainer> LstInventoryReportContainer { get; set; } = new List<InventoryReportContainer>();

       public List<InvenontoryReportCargo> LstInventoryReportCargo { get; set; } = new List<InvenontoryReportCargo>();
    }
    public class InventoryReportContainer
    {
        public string CFSCode { get; set; }
        public string Party { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Commodity { get; set; }
        public string DaysAtCFS { get; set; }
    }
    public class InvenontoryReportCargo
    {
        //public string CFSCode { get; set; }
        public string Party { get; set; }
        public string Cargo { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }
        public string DaysAtCFS { get; set; }
    }    
}