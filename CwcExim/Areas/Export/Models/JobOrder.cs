using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class JobOrder
    {
        public int JobOrderId { get; set; }
        public int ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceDate { get; set; }
        public int? CHAId { get; set; }
        public string CHAName { get; set; }
        public int? ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public string FromLocation { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToLocation { get; set; }
        public string PickUpRefNo { get; set; }
        public string PickUpRefDate { get; set; }
        public string Remarks { get; set; }
        public string StringifiedText { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(1,999999999,ErrorMessage = "No of Container must be greater then 0 and less then 999999999")]
        public int NoOfContainer { get; set; }
    }
    public class JobOrderDetails
    {
        public int JobOrderDtlId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public int Reefer { get; set; }
    }
    //For List Of Job Order
    public class JobOrderList
    {
        public int JobOrderId { get; set; }
        [Display(Name = "Job Order No")]
        public string JobOrderNo { get; set; }
        [Display(Name = "Job Order Date")]
        public string JobOrderDate { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
    }
    /*For Print*/
    public class PrintJobOrderModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<PrintJobOrderModelDet> lstDet { get; set; } = new List<PrintJobOrderModelDet>();
    }
    public class PrintJobOrderModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }
}
