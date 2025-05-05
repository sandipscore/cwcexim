using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CwcExim.Areas.Master.Models;
namespace CwcExim.Areas.Import.Models

{
    public class Hdb_ImportJobOrder
    {
        public int ImpJobOrderId { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]
        public string PickUpLocation { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PortName { get; set; }
        public int PortId { get; set; }
        public bool RoundTrip { get; set; }
        public int JobOrderFor { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal? Distance { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }

        ///[Required(ErrorMessage = "Fill Out This Field")]
        ///
        
        public string FormOneNo { get; set; }

        public string FormOneDate { get; set; }
        /*public string SealNo { get; set; }
        public string ContainerNo { get; set; }*/
        public int ShippingLineId { get; set; }
        
        ///[Required(ErrorMessage = "Fill Out This Field")]

        public int? CHAId { get; set; }
        public int FormOneId { get; set; }

        // [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]

        public int? ToYardId { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]


        public string YardWiseLocationIds { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]
        public string YardWiseLctnNames { get; set; }


        public int FromContainer { get; set; }
        public string StringifyXML { get; set; }
        public string StringifyClauseXML { get; set; }
      //  [Required(ErrorMessage = "Fill Out This Field")]
      //  public int OperationId { get; set; }
        public string JobOrderDetailsJS { get; set; }
        public string JobOrderClauseJS { get; set; }


    }
    public class Hdb_ImportJobOrderDtl
    {

        public int JobOrderDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string ContainerSize { get; set; }
        public string SealNo { get; set; }
        public decimal? GrossWeight { get; set; }
        public string CustomSealNo { get; set; }
        
        public int? CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ContainerLoadType { get; set; }
        public string YardWiseLctnNames { get; set; }
        public string YardWiseLocationIds { get; set; }
        public string Size { get; set; }
        public int? NoofPackages { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int IsODC { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public int? ToYardId { get; set; }
        public string YardName { get; set; }
        public string Commodity { get; set; }
        public int CommodityId { get; set; }
        public string FromLocation { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]
        public string ToLocation { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }
        ///[Required(ErrorMessage = "Fill Out This Field")]
        ///
        public string DisplayCargoType { get; set; }
        public string OBLNO { get; set; }
        public string LineNo { get; set; }

        public string PackageType { get; set; }

        public string Others { get; set; }
        public string OperationName { get; set; }
        public string OtherOperation { get; set; }
        public int? GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownWiseLocationIds { get; set; }
        public string GodownWiseLctnNames { get; set; }
        public string Purpose { get; set; }

    }

    public class Hdb_ImportClauseDtl
    {

        public int OperationId { get; set; }
        public string OperationCode { get; set; }
      
    }
    public class Hdb_ImportJobOrderList
    {
        public int JobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string FormOneNo { get; set; }
        public int JobOrderFor { get; set; }
        public string Purpose { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }

    }
    public class Hdb_BlNoList
    {
        public int FormOneId { get; set; }
        public string BlNo { get; set; }
    }
    public class Hdb_FormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class Hdb_FormOneDtlForImpJO
    {
        public int FormOneDetailId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Reefer { get; set; }
        public string SealNo { get; set; }
    }
    /* public class Hdb_Yard
     {
         public int YardId { get; set; }
         public string YardName { get; set; }
     }
     public class Hdb_YardWiseLocation
     {
         public int LocationId { get; set; }
         public string Location { get; set; }
        // public bool IsOccupied { get; set; }
     }*/
    /*For Job Order Print*/
    public class Hdb_PrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<Hdb_PrintJOModelDet> lstDet { get; set; } = new List<Hdb_PrintJOModelDet>();     

    }
    public class Hdb_PrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Sline { get; set; }
    }
    
    public class PrintJobOrder
    {
        public string Location { get; set; }
        public string PartyName { get; set; }
        public string NoOfUnits { get; set; }
        public string BOENo { get; set; }
        public string ShipBillNo { get; set; }
        public string NatureOfOperation { get; set; }
        public string Weight { get; set; }
        //public List<string> LstOperation { get; set; } = new List<string>();
    }

    public class OperationList
    {
        public string OperationName { get; set; }
    }
}
