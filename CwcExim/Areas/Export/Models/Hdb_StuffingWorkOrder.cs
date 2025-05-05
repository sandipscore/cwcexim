using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_StuffingWorkOrder
    {
        public int WorkOrderId { get; set; }

        [DisplayName("Work Order No.")]
        public string WorkOrderNo { get; set; }
        public string Forwarder { get; set; }

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
        public IList<Hdb_StuffingWorkOrderDtl> lstStuffingWorkOrderDtl { get; set; } = new List<Hdb_StuffingWorkOrderDtl>();
        public IList<Hdb_StuffingNoList> lstStuffingNoList { get; set; } = new List<Hdb_StuffingNoList>();
        public string StuffingNoListJS { get; set; }
        public IList<Hdb_GodownList> lstGodownList { get; set; } = new List<Hdb_GodownList>();
        public IList<Hdb_ContainerList> lstContainerList { get; set; } = new List<Hdb_ContainerList>();
        public IList<Commodity> lstCommodity { get; set; } = new List<Commodity>();
        public string StuffingWorkOrderDtlJS { get; set; }
    }
    public class Hdb_StuffingWorkOrderDtl
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
    public class Hdb_StuffingNoList
    {
        public int StuffingRequestId { get; set; }
        public string StuffingRequestNo { get; set; }
        public string StuffingRequestDate { get; set; }
    }
    public class Hdb_GodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
    }
    public class Hdb_ContainerList
    {
        public string StuffingRequestDate { get; set; }
        public int StuffingRequestDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightPerUnit { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }

    }
}