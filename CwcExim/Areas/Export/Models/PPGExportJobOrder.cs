using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class PPGExportJobOrder
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public int TrainSummarySerial { get; set; }
        public int ImpJobOrderId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PickUpLocation { get; set; }
        public int JobOrderFor { get; set; }
        public int TransportBy { get; set; }
        public string JobOrderNo { get; set; }

        //public int JobOrderId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime JobOrderDate { get; set; }
        //  [Required(ErrorMessage = "Fill Out This Field")]
        [Required]
        public String TrainNo { get; set; }
        public DateTime TrainDate { get; set; }
        public int FormOneId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrainSummaryID { get; set; }
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
        [StringLength(200)]
        public string Remarks { get; set; }
        public string StringifyXML { get; set; }
        public string deleteXML { get; set; }
        public List<PPGPickupModel> lstpickup { get; set; }
        public List<TransformList> lstPort { get; set; }

        public string TransactionType { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }

    }


    public class FormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class PPG_ImportJobOrderDtl
    {

        public int TrainSummarySerial { get; set; }
        public int JobOrderDtlId { get; set; }
        public int JobOrderId { get; set; }
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string CustomSealNo { get; set; }
        public string ShippingLineNo { get; set; }
        public int ShippingLineId { get; set; }

        public string ShippingLineName { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }
        public string TransportForm { get; set; }
        public string Line_Seal_No { get; set; }
        public string Cont_Commodity { get; set; }
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public int PortId { get; set; }
        public string Wagon_No { get; set; }
        public string S_Line { get; set; }
        public decimal Ct_Tare { get; set; }
        public decimal Cargo_Wt { get; set; }
        public decimal NoOfPackages { get; set; }
        public decimal Gross_Wt { get; set; }
        public string Ct_Status { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }

        public string TransportName { get; set; }

    }


    public class ppgexportjobordersum
    {
        public string POL { get; set; }
        public int SZ20 { get; set; }
        public int SZ40 { get; set; }
    }
    public class TransformList
    {
        public int PortId { get; set; }
        public string PortName { get; set; }
    }
    public class PPG_ImportJobOrderList
    {
        public int ImpJobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
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
    }

    public class PPGPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string OnBehalf { get; set; }
        public string ShippingLineName { get; set; }
        public string CustomSealNo { get; set; }

        public string Sline { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }

        public decimal Ct_Tare { get; set; }
        public decimal Cargo_Wt { get; set; }
        public string CFSCode { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }

    }

    public class PPG_PrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }

    public class ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }
    public class PPG_TrainList
    {
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string PickUpLoc { get; set; }
    }
    public class PPG_FormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class PPG_TrainDtl
    {
        public int TrainSummarySerial { get; set; }
        public int JobOrderDtlId { get; set; }
        public int JobOrderId { get; set; }
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public int PortId { get; set; }
        public string Wagon { get; set; }
        public string ContainerNo { get; set; }
        public string SZ { get; set; }
        public string CustomSealNo { get; set; }
        public string ShippingLineNo { get; set; }
        public int ShippingLineId { get; set; }

        public string ShippingLineName { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }
        public string TransportForm { get; set; }

        public string LineSeal { get; set; }
        public string Commodity { get; set; }
        public string SLine { get; set; }
        public decimal TW { get; set; }
        public decimal CW { get; set; }
        public decimal GW { get; set; }
        public decimal PKGS { get; set; }
        public string Ct_Status { get; set; }
        public string CustomSeal { get; set; }
        public string Shipper { get; set; }
        public string CHA { get; set; }
        public string CRRSBillingParty { get; set; }
        public string BillingParty { get; set; }

        public string StuffingMode { get; set; }
        public string SBillNo { get; set; }

        public string Date { get; set; }
        public string Origin { get; set; }

        public string POL { get; set; }

        public string POD { get; set; }

        public string DepDate { get; set; }
        public string TransportName { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }

    }
    public class PPG_Yard
    {
        public int YardId { get; set; }
        public string YardName { get; set; }
    }
    public class PPG_YardWiseLocation
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
        // public bool IsOccupied { get; set; }
    }
    /*For Job Order Print*/
    public class PPG_PrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<PrintJOModelDet> lstDet { get; set; } = new List<PrintJOModelDet>();
    }
  


    public class PrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }

    public class PPG_ImportJobDel
    {


        public int JobOrderDtlId { get; set; }
    }
}