using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class CartingWorkOrder
    {
        public int CartingWorkOrderId { get; set; }
        public int BranchId { get; set; }
        public string WorkOrderNo { get; set; }
        public int CartingAppId { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string CartingNo { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string WorkOrderDate { get; set; }
        public int GodownId { get; set; }

        [StringLength(1000,ErrorMessage = "Remarks Cannot Be More Than 1000 Characters")]
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public IList<Godown> LstGodown { get; set; } = new List<Godown>();
        public IList<CartingWorkOrder> LstCarting { get; set; } = new List<CartingWorkOrder>();
        public string CartingDate { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string GodownName { get; set; }
        public string CommodityName {get;set;}
        public string CargoDescription { get; set; }
        public int ? NoOfUnits { get; set; }
        public decimal ?Weight { get; set; }
    }
}