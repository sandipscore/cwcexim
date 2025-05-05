using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class DestuffingWO
    {
        public int DeStuffingWOId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int DeStuffingId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate{ get; set; }
        [Display(Name ="Godown Name")]
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        public string GodwnWiseLctnIds { get; set; }
        [Display(Name = "Godown Location")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodwnWiseLctnNames { get; set; }
        public int DeliveryType { get; set; }
        public string ShippingLine { get; set; }
        public string Rotation { get; set; }
        public string Remarks { get; set; }
        public string StringifyXML { get; set; }
        public string LctnXML { get; set; }
        public string DestuffingNo { get; set; }
        public string ContractorName { get; set; }
    }
    public class DestuffingWODtl
    {
        public int DestuffingDtlId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string CHAName { get; set; }
        public string ImporterName { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight{ get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }

    }
    public class ListDestuffingWO
    {
        public int DeStuffingWOId { get; set; }
        //public string ContainerNo { get; set; }
        public string WorkOrderNo { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
    }
    public class Godown
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
    }
    public class GodownWiseLctn
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
     //   public int IsOccupied { get; set; }
    }
    public class ContDetails//For Container Details
    {
        public int DestuffingId { get; set; }
        public string ContainerNo { get; set; }
    }
}
