using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class WljEntryThroughGate
    {
        public int EntryId { get; set; }


        public string DisPlayCfs { get; set; }
        [Display(Name = "ICD Code")]
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
        [Required(ErrorMessage = "Select Shipping Line")]
        public string ShippingLine { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Shipping Line Id Not Found.")]
        public int ShippingLineId { get; set; }

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
        [Required(ErrorMessage = "Fill Out This Field")]
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
        [Required(ErrorMessage = "Select Cargo Type")]
        [Range(1, int.MaxValue, ErrorMessage = "Select Cargo Type")]
        public int? CargoType { get; set; }

        [Display(Name = "No of Packages")]
        public int? NoOfPackages { get; set; }
        public int? ActualPackages { get; set; }
        [Display(Name = "Gross Weight")]
        public decimal? GrossWeight { get; set; }

        public decimal? TareWeight { get; set; }
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

        public string OperationType { get; set; }

        public string ReferenceNoId { get; set; }
        //------------------------------------------------------
        [Required(ErrorMessage = "Select Transport Mode")]
        public int TransportMode { get; set; }

        [Required(ErrorMessage = "Select Load Type")]
        public string ContainerLoadType { get; set; }

        [Required(ErrorMessage = "Select Transport From")]
        public string TransportFrom { get; set; }
        public int InvoiceId { get; set; }

        public string InvoiceNo { get; set; }
        //public int CBT { get; set; } = 0;
        public bool IsCBT { get; set; } = false;

        public string TPNo { get; set; }

        public string SystemDateTime { get; set; }

        public string JobOrderDate { get; set; }
        public bool IsOdc { get; set; } = false;
        public string JobOrderNo { get; set; }

        public string PrintSealCut { get; set; } = string.Empty;
        public string CBTContainer { get; set; } = string.Empty;
        public string SubCFSCode { get; set; } = string.Empty;
        public string ExportType { get; set; } = string.Empty;
        public string StuffType { get; set; } = string.Empty;
        public string StringifyData { get; set; } = string.Empty;
        public int GodownId { get; set; } = 0;
        public string GodownName { get; set; } = string.Empty;

        public IList<Wlj_AddMoreRefForCCIN> listAddRef = new List<Wlj_AddMoreRefForCCIN>();

        public string SCMTRXML { get; set; }

        public List<WLJGateEntrySCMTR> LstSCMTR { get; set; } = new List<WLJGateEntrySCMTR>();
    }

    public class Wljcontainer
    {
        public string ContainerName { get; set; }


    }
    public class WljSearchCont
    {
        public List<WFLDcontainer> lstConatiner { get; set; } = new List<WFLDcontainer>();
        public string State { get; set; }
    }
    public class WljEntryThroughGateShipping
    {

        public string ShippingLine { get; set; }


        public int ShippingLineId { get; set; }
    }
    public class WljLoadedContainerListWithData
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
    }
    public class WljEntryThroughGateCHA
    {
        public string CHAName { get; set; }
        public int CHAId { get; set; }
    }
    public class WljcontainerAgainstGp
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

        public string GPDate { get; set; }


    }

    public class WljEntryThroughGateBond//: HDB_EntryThroughGate
    {
        public int EntryId { get; set; }
        public string OperationType { get; set; }
        public string ContainerType { get; set; }
        public string EntryDateTime { get; set; }
        public string SystemDateTime { get; set; }
        public string GateInNo { get; set; }
        public string CFSCode { get; set; }
        public int IsCBT { get; set; }
        public int ReferenceNoId { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceDate { get; set; }
        public int IsVehicle { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public string TypeofPackage { get; set; }
        public string Others { get; set; }
        public string CBTNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CustomSealNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string VehicleNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string DepositorName { get; set; }
        public string Time { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }

    }
    public class Wlj_ReferenceNumberCCIN
    {

        public IList<Wlj_CCINList> ReferenceList = new List<Wlj_CCINList>();

        // public IList<GateEntryExport> listExport = new List<GateEntryExport>();

        public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    }

    public class Wlj_CCINList
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
        public int GodownId { get; set; } = 0;
        public string GodownName { get; set; } = string.Empty;

    }
    public class Wlj_AddMoreRefForCCIN
    {
        public int addRefId { get; set; } = 0;
        public string addRefNo { get; set; } = string.Empty;
        public int addRefpkg { get; set; } = 0;
    }
    public class WLJGateEntrySCMTR
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

    }
}