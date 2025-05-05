using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class DSR_JobOrder
    {
        public int TrainSummarySerial { get; set; }
        public int ImpJobOrderId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PickUpLocation { get; set; }
        public int JobOrderFor { get; set; }
        public int TransportBy { get; set; }
        public string JobOrderNo { get; set; }


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
       [ StringLength(200)]
        public string Remarks { get; set; }
        public string StringifyXML { get; set; }
        public string deleteXML { get; set; }
        public int ImporterId { get; set; } = 0;
        public string ImporterName { get; set; } = "";
        public int PortId { get; set; } = 0;
        public string PortName { get; set; } = "";
        public int PolId { get; set; } = 0;
        public string PolName { get; set; } = "";
        public int SelectPortId { get; set; }
        public int PayerId { get; set; } = 0;
        public string PayerName { get; set; } = "";
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = "";
        public List<DSRPickupModel> lstpickup { get; set; }
        public List<DSRTransformList> lstPort { get; set; }
        public List<DSR_ImportJobOrderDtl> lstInv { get; set; } = new List<DSR_ImportJobOrderDtl>();
        public List<DSR_ImportContainerJobOrderDtl> lstCont { get; set; } = new List<DSR_ImportContainerJobOrderDtl>();

        public List<DSR_ImportContainerJobOrderInvDtl> lstContView { get; set; } = new List<DSR_ImportContainerJobOrderInvDtl>();
        

    }
    public class DSR_ImportJobOrderDtl
    {
        
        public int TrainSummarySerial { get; set; }
        public int JobOrderDtlId { get; set; }
        public int JobOrderId { get; set; }
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string CustomSealNo { get; set; }
        public string ShippingLineNo { get; set; }
        public int ShippingLineId { get; set; }
        public int PolId { get; set; }
        public string PolName { get; set; }
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
        public string Ct_ODC { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }
        
        public string TransportName { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public int NewImporterId { get; set; }
        public string NewImporterName { get; set; }
        public int PayerId { get; set; }
        public string PayerName { get; set; }
        public string InvoiceAmt { get; set; }
        public string Roundup { get; set; }
        public string Taxable { get; set; }
        public string IGSTPer { get; set; }
        public string IGSTAmt { get; set; }
        public string CGSTPer { get; set; }
        public string CGSTAmt { get; set; }
        public string SGSTPer { get; set; }
        public string SGSTAmt { get; set; }
        public string InoviceId { get; set; }
        public string OperationId { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public string Clause { get; set; }
        public string GST { get; set; }

        public int ChargeId { get; set; }
        public bool Showhidden { get; set; } = false;
    }


    public class DSR_ImportContainerJobOrderDtl
    {
        public int ChargeId { get; set; }
        public int TrainSummarySerial { get; set; }
        public int JobOrderDtlId { get; set; }
        public int JobOrderId { get; set; }
        public int PolId { get; set; }
        public string PolName { get; set; }
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
        public string Ct_ODC { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }

        public string TransportName { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public int NewImporterId { get; set; }
        public string NewImporterName { get; set; }
        public int PayerId { get; set; }
        public string PayerName { get; set; }
      
    }
    public class DSR_ImportContainerJobOrderInvDtl
    {
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string ShippingLineNo { get; set; }
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
        public string Ct_ODC { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }
        public int PolId { get; set; }
        public string PolName { get; set; }
        public string TransportName { get; set; }
     
        public string ImporterName { get; set; }

        public string NewImporterName { get; set; }
      
        public string PayerName { get; set; }

        public string Roundup { get; set; }
        public string Taxable { get; set; }
        public string GST { get; set; }
       
        public string Total { get; set; }
        public bool Showhidden { get; set; } = false;
        public string InvoiceAmt { get; set; }


    }
    public class DSRTransformList
    {
        public int PortId { get; set; }
        public string PortName { get; set; }
    }
    public class DSR_ImportJobOrderList
    {
        public int ImpJobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate{ get; set; }
        public string Importer { get; set; }
        public string Port { get; set; }
        
        
    }
    public class DSR_TrainList
    {
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string PickUpLoc { get; set; }
    }
    public class DSR_FormOneForImpJO
    {
        public string FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
    public class DSR_TrainDtl
    {
        public int TrainSummarySerial { get; set; }
        public int JobOrderDtlId { get; set; }
        public int JobOrderId { get; set; }
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public int PortId { get; set; }
        public string Wagon_No { get; set; }
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string CustomSealNo { get; set; }
        public string ShippingLineNo { get; set; }
        public int ShippingLineId { get; set; }

        public string ShippingLineName { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public int PayerId { get; set; }
        public string PayerName { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }
        public string TransportForm { get; set; }

        public string Line_Seal_No { get; set; }
        public string Cont_Commodity { get; set; }
        public string S_Line { get; set; }
        public decimal Ct_Tare { get; set; }
        public decimal Cargo_Wt { get; set; }
        public decimal Gross_Wt { get; set; }
        public decimal NoOfPackages { get; set; }
        public string Ct_Status { get; set; }
        public string Ct_ODC { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public string TransportName { get; set; }
        public string Remarks { get; set; }
        public string CargoDescription { get; set; }
        public string InvoiceAmt { get; set; }
        public string Roundup { get; set; }
        public string Taxable { get; set; }
        public string IGSTPer { get; set; }
        public string IGSTAmt { get; set; }
        public string CGSTPer { get; set; }
        public string CGSTAmt { get; set; }
        public string SGSTPer { get; set; }
        public string SGSTAmt { get; set; }
        public bool Showhidde { get; set; } = true;


    }
    public class DSR_Yard
    {
        public int YardId { get; set; }
        public string YardName { get; set; }
    }
    public class DSR_YardWiseLocation
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
        // public bool IsOccupied { get; set; }
    }
    /*For Job Order Print*/
    public class DSR_PrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public IList<DSR_PrintJOModelDet> lstDet { get; set; } = new List<DSR_PrintJOModelDet>();
    }
    public class DSR_PrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }

    public class DSR_ImportJobDel
    {
        public int JobOrderDtlId { get; set; }
    }

    public class DSR_TrainDateList
    {
        public string TrainDate { get; set; }

    }
}