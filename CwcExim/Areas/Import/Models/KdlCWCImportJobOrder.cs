using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class KdlCWCImportJobOrder
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
        public string StringifyXML { get; set; }
    }
    public class KdlImportJobOrderDtl
    {
        public int FormOneDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Reefer { get; set; }
        public string SealNo { get; set; }
    }
    public class KdlImportJobOrderList
    {
        public int ImpJobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string FormOneNo { get; set; }
    }
    public class KdlCWCBlNoList
    {
        public int FormOneId { get; set; }
        public string BlNo { get; set; }
    }
    public class KdlCWCFormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class KdlCWCFormOneDtlForImpJO
    {
        public int FormOneDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Reefer { get; set; }
        public string SealNo { get; set; }
    }
    public class KdlCWCYard
    {
        public int YardId { get; set; }
        public string YardName { get; set; }
    }
    public class KdlYardWiseLocation
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
       // public bool IsOccupied { get; set; }
    }
    /*For Job Order Print*/
    public class KdlPrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<KdlPrintJOModelDet> lstDet { get; set; } = new List<KdlPrintJOModelDet>();
    }

    public class KdlPPGPrintJOModel
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
    }
    public class KdlPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }

    public class KdlPPGPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Sline { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }

    }
}