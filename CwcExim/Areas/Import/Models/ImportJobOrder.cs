using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class ImportJobOrder
    {
        public int ImpJobOrderId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PickUpLocation { get; set; }
        public int JobOrderFor { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public int FormOneId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FormOneNo { get; set; }
        public string FormOneDate { get; set; }
        /*public string SealNo { get; set; }
        public string ContainerNo { get; set; }*/
        public int ShippingLineId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLineName { get; set; }
        public int? CHAId { get; set; }

       // [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromLocation { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int ToYardId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string YardName { get; set; }
        public string YardWiseLocationIds { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string YardWiseLctnNames { get; set; }
        public string Remarks { get; set; }
        public int JobOrderId { get; set; }

        public string StringifyXML { get; set; }
    }
    public class ImportJobOrderDtl
    {
        public int JobOrderId { get; set; }

        public int JobOrderDtlId { get; set; }
        public int FormOneDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Reefer { get; set; }
        public string SealNo { get; set; }
    }
    public class ImportJobOrderList
    {
        public int ImpJobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string FormOneNo { get; set; }
    }
    public class BlNoList
    {
        public int FormOneId { get; set; }
        public string BlNo { get; set; }
    }
    public class FormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class FormOneDtlForImpJO
    {
        public int FormOneDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Reefer { get; set; }
        public string SealNo { get; set; }
    }
    public class Yard
    {
        public int YardId { get; set; }
        public string YardName { get; set; }
    }
    public class YardWiseLocation
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
       // public bool IsOccupied { get; set; }
    }
    /*For Job Order Print*/
    public class PrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<PrintJOModelDet> lstDet { get; set; } = new List<PrintJOModelDet>();
    }

    public class PPGPrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public IList<PPGPrintJOModelDet> lstDet { get; set; } = new List<PPGPrintJOModelDet>();
        public string PortName { get; set; }
    }
    public class PrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }

    public class PPGPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Sline { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }
        public string LineSealNo { get; set; }
        public decimal TareWt { get; set; }
        public decimal CargoWt { get; set; }
        public string TPNo { get; set; }
        public string WagonNo { get; set; }

        public string TPDate { get; set; }

        public decimal Gross_Wt { get; set; }

        public string CargoDescription { get; set; }

        public string Destination { get; set; }
    }

   
    public class WFLDPrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public IList<WFLDPrintJOModelDet> lstDet { get; set; } = new List<WFLDPrintJOModelDet>();
    }
    public class WFLDPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Sline { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }

    }
}