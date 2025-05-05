using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class EntryThroughGate
    {
        public int EntryId { get; set; }

        [Display(Name = "CFS Code")]
        public string CFSCode { get; set; }

        [Display(Name = "Gate In No")]
        public string GateInNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string EntryDateTime { get; set; }

        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]

        [Display(Name = "Reference Date")]
        public string ReferenceDate { get; set; }

        [Display(Name = "Shipping Line")]
        //[Required(ErrorMessage = "Select Shipping Line")]
        public string ShippingLine { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Shipping Line Id Not Found.")]
        public int ShippingLineId { get; set; } = 0;

        [Display(Name = "CHA Name")]
       // [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

        [Display(Name = "Container No")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }


        [Display(Name = "Container No 1")]
        public string ContainerNo1 { get; set; }
        

        [Display(Name = "Size")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "Reefer")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public bool Reefer { get; set; }

        [Display(Name = "Custom Seal No")]
        public string CustomSealNo { get; set; }

        [Display(Name = "Shipping Line Seal No")]
        public string ShippingLineSealNo { get; set; }


        
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string VehicleNo { get; set; }

        [Display(Name = "Challan No")]
        public string ChallanNo { get; set; }

        [Display(Name = "Cargo Description")]
        [MaxLength(1000)]
        public string CargoDescription { get; set; }

        [Display(Name = "Cargo Type")]
        //[Required(ErrorMessage = "Select Cargo Type")]
        //[Range(1, int.MaxValue, ErrorMessage = "Select Cargo Type")]
        public int? CargoType { get; set; }

        [Display(Name = "No of Packages")]
        public int? NoOfPackages { get; set; }

        [Display(Name = "Gross Weight")]
        public decimal? GrossWeight { get; set; }

        [Display(Name = "Depositor Name")]
        public string DepositorName { get; set; }

        [Display(Name = "Remarks")]
        [RegularExpression("^[A-Za-z0-9\\-,./\r\n:'&?() ]*$", ErrorMessage = "Invalid Character.")]
        [MaxLength(1000)]
        public string Remarks { get; set; }
        public int Uid { get; set; }

        public int BranchId { get; set; }

        public string ContainerId { get; set; }

        public string Time { get; set; }
        public string EntryTime { get; set; }

        public string ContainerType { get; set; }
        public string ContainerClass { get; set; } = "";

        public string OperationType { get; set; }

        public string ReferenceNoId { get; set; }
    //   public string SCMTRXML { get; set; }

    //    public List<GateEntrySCMTR> LstSCMTR { get; set; } = new List<GateEntrySCMTR>();

    }

    public class container
    {
        public string ContainerName { get; set; }

        
    }
    public class SearchCont
    {
        public List<container> lstConatiner { get; set; } = new List<container>();
        public string State { get; set; }
    }
  /*  public class GateEntrySCMTR
    {
        public Int64 Id { get; set; }
        public Int64 CIMNo { get; set; }
        public string CIMDate { get; set; }
        public string ReportingpartyCode { get; set; }
        public string DestinationUnlading { get; set; }
        public string TransportMeansType { get; set; }
        public string TransportMeansNo { get; set; }
        public Int64 TotalEquipment { get; set; }
        public string ActualArrival { get; set; }
        public string ContainerID { get; set; }
        public string Equipmenttype { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipStatus { get; set; }

        public Int64 EquipmentSerialNo { get; set; }

        public Int64 DocumentSerialNo { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentReferenceNo { get; set; }

    }*/
    public class containerAgainstGp
    {
        public string ContainerName { get; set; }

        public int IsReefer { get; set; }


        public int size { get; set; }

        public string CargoDescription { get; set; }

        public string CargoType { get; set; }
        public string NoOfUnits { get; set; }

        public string VehicleNo { get; set; }

        public string Weight { get; set; }



        public string CFSCode { get; set; }
        public string shippingLine { get; set; }
        public string ShippingLineId { get; set; }

        public string OperationType { get; set; }
        public string GatePassDate { get; set; }
        public string CHAName { get; set; }

        public bool IsBond { get; set; }
        public string Module { get; set; }

    }


    public class ReferenceNumberList
    {
        public int CartingAppId { get; set; }
        public string CartingNo { get; set; }

    }


    public class BondReferenceNumberList
    {
        public int SpaceappId { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }

        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }

        public string CHAName { get; set; }
        

    }


    public class LoadContainerReferenceNumberList
    {
        public int LoadContReqId { get; set; }
        public string LoadContReqNo { get; set; }
        public string LoadedReq { get; set; }
        public string LoadContReqDate { get; set; }

        public string ContainerNo { get; set; }

    }
    public class ShippingLineList
    {
        public  string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }


    }

    public class LoadedContainer
    {
        List<LoadContainerReferenceNumberList> listLoadContainer = new List<LoadContainerReferenceNumberList>();
        List<ShippingLineList> listShippingLine = new List<ShippingLineList>();

    }
    public class ReferenceNumber
    {

        public IList<ReferenceNumberList> ReferenceList = new List<ReferenceNumberList>();

        public IList<GateEntryExport> listExport = new List<GateEntryExport>();

        public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    }


    public class GateEntryExport
    {
        public string ContainerName { get; set; } //ReferenceNo
        public string ReferenceNo { get; set; }
        public string ReferenceDate { get; set; }
        public string ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public int CargoType { get; set; }
        public string CHAName { get; set; }

        public int NoOfUnits { get; set; }

        public decimal Weight { get; set; }
        public string CargoDescription { get; set; }
        public string VehicleNo { get; set;}

        public int Reefer { get; set; }
        //public string ContainerName { get; set; }
        //public string ContainerName { get; set; }
        //public string ContainerName { get; set; }
        //public string ContainerName { get; set; }
        //public string ContainerName { get; set; }

    }


    public class LoadedContainerListWithData
    {

        public string ContainerNo { get; set; } //ReferenceNo
        public string ExporterId { get; set; }
        public string ShippingLineId { get; set; }
        
        public string ShippingLineName { get; set; }
        public string Size { get; set; }
        public string Reefer { get; set; }
        public string CargoType { get; set; }
        public string CargoDescription { get; set; }
        public string NoOfUnits { get; set; }
        public string GrossWt { get; set; }
        public string CHAName { get; set; }
        public string ReferenceDate { get; set; }

        public int CHAId { get; set; }

        public string CustomSeal { get; set; }
    }

    public class ReferenceNumberCCIN
    {

        public IList<CCINList> ReferenceList = new List<CCINList>();

       // public IList<GateEntryExport> listExport = new List<GateEntryExport>();

        public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    }

    public class CCINList
    {
        public int CCINId { get; set; }
        public string CCINNo { get; set; }
        public string CCINDate { get; set; }
        public int ShippingLineId { get; set; }      
        public string Weight { get; set; }
        public string NoOfUnits { get; set; }


        public string ShippingLine { get; set; } = "";


        public string CHAName { get; set; } = "";
        public int CargoType { get; set; }
        public int CHAId { get; set; }

    }


    public class containerAgainstGp_PPG
    {
        public string ContainerName { get; set; }

        public int IsReefer { get; set; }


        public int size { get; set; }

        public string CargoDescription { get; set; }

        public string CargoType { get; set; }
        public string NoOfUnits { get; set; }

        public string VehicleNo { get; set; }

        public string Weight { get; set; }



        public string CFSCode { get; set; }
        public string shippingLine { get; set; }
        public string ShippingLineId { get; set; }
        public string OperationType { get; set; }

        public string GPDate { get; set; }
        public string CHAName { get; set; }

        public string Module { get; set; }

    }

    public class containerAgainstGp_Hdb
    {
        public string ContainerName { get; set; }

        public int IsReefer { get; set; }


        public int size { get; set; }

        public string CargoDescription { get; set; }

        public string CargoType { get; set; }
        public string NoOfUnits { get; set; }

        public string VehicleNo { get; set; }

        public string Weight { get; set; }



        public string CFSCode { get; set; }
        public string shippingLine { get; set; }
        public string ShippingLineId { get; set; }

        public string OperationType { get; set; }
        public string GatePassDate { get; set; }
        public string CHAName { get; set; }

        public bool IsBond { get; set; }

        public string DepositorName { get; set; }
        public string Module { get; set; }

    }
}