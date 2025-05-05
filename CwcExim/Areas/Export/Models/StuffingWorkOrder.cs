using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class StuffingWorkOrder
    {
        public int WorkOrderId { get; set; }

        [DisplayName("Work Order No.")]
        public string WorkOrderNo { get; set; }

        [DisplayName("Work Order Date")]
        public string WorkOrderDate { get; set; }

        public int StuffingRequestId { get; set; }

        [DisplayName("Stuffing No"), Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingRequestNo { get; set; }

        [DisplayName("Stuffing Date"), Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingRequestDate { get; set; }

        public int GodownId { get; set; }

        [DisplayName("Godown"), Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Order Type")]
        public string OrderType { get; set; }
        public IList<StuffingWorkOrderDtl> lstStuffingWorkOrderDtl { get; set; } = new List<StuffingWorkOrderDtl>();
        public IList<StuffingNoList> lstStuffingNoList { get; set; } = new List<StuffingNoList>();
        public string StuffingNoListJS { get; set; }
        public IList<GodownList> lstGodownList { get; set; } = new List<GodownList>();
        public IList<ContainerList> lstContainerList { get; set; } = new List<ContainerList>();
        public IList<Commodity> lstCommodity { get; set; } = new List<Commodity>();
        public string StuffingWorkOrderDtlJS { get; set; }
    }
    public class StuffingWorkOrderDtl
    {
        public int WorkOrderDetailId { get; set; }
        public int WorkOrderId { get; set; }
        public string ContainerNo { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightPerUnit { get; set; }
    }
    public class StuffingNoList
    {
        public int StuffingRequestId { get; set; }
        public string StuffingRequestNo { get; set; }
        public string StuffingRequestDate { get; set; }
    }
    public class GodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
    }
    public class ContainerList
    {
        public int StuffingRequestDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightPerUnit { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
    }
}