using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CartingWorkOrder
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int SlNo { get; set; }
        public string ContainerNo { get; set; }
        public string CommodityName { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string CartingNo { get; set; }
        public string ApplicationDate { get; set; }
    }
}