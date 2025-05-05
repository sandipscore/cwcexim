using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class DndEntryThroughGate
    {
        public int EntryId { get; set; }


        public string DisPlayCfs { get; set; }
        [Display(Name = "CFS Code")]
        public string CFSCode { get; set; }
        public string NewCFSCode { get; set; }

        [Display(Name = "Gate In No")]
        public string GateInNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string EntryDateTime { get; set; }

        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Fill Out This Field")]

        [Display(Name = "Reference Date")]
        public string ReferenceDate { get; set; }

        [Display(Name = "Shipping Line")]
      // [Required(ErrorMessage = "Select Shipping Line")]
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

        [Display(Name = "Actual Gross Weight")]
        public decimal? ActualGrossWeight { get; set; }

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

        public string JobOrderNo { get; set; }

        public bool IsOdc { get; set; } = false;
        public string CBTContainer { get; set; } = string.Empty;

        public string StuffType { get; set; } = string.Empty;

        public string ExportType { get; set; } = string.Empty;

        public int GodownId { get; set; } = 0;
        public string GodownName { get; set; } = string.Empty;

        public string StringifyData { get; set; } = string.Empty;

        public IList<Dnd_AddMoreRefForCCIN> listAddRef = new List<Dnd_AddMoreRefForCCIN>();
        public string Cargo= string.Empty;
        public bool IsScan { get; set; }

    }

    public class Dnd_EntryThroughGateBond//: HDB_EntryThroughGate
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
        [Range(0, 99999999.99, ErrorMessage = "Weight Cannot Be More Than 99999999.99")]
        public decimal GrossWeight { get; set; }
        public string DepositorName { get; set; }
        public string Time { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }

    }

    public class Dnd_GateEntrySealCutting
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SealNo { get; set; }
        public string BLNo { get; set; }
        public string Importer { get; set; }
        public string LCLFCL { get; set; }
        public string ArrivalDate { get; set; }
        public string NoOfUnits { get; set; }
        public string GodownNo { get; set; }
        public string CargoDescription { get; set; }
        public string ReferenceNo { get; set; }
        public string CHA { get; set; }
        public string Exporter { get; set; }
        public string VehicleNo { get; set; }

        public string SACNo { get; set; } = string.Empty;

        public string SACDate { get; set; } = "";
        public string AWBNo { get; set; } = string.Empty;
        public string AWBDate { get; set; } = "";
        public string Purpose { get; set; }
        public string EntryDate { get; set; }

        public string CompanyAddress { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string CBTFrom { get; set; } = string.Empty;

    }

    public class Dnd_LoadedContainerListWithData
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
        public string ExportType { get; set; } = string.Empty;
        public string CustomSealNo { get; set; } = string.Empty;
        public string ShippingSealNo { get; set; } = string.Empty;
        public decimal? TareWt { get; set; }
        public string ContClass { get; set; } = string.Empty;
    }

    public class Dnd_ReferenceNumberCCIN
    {

        public IList<Dnd_CCINList> ReferenceList = new List<Dnd_CCINList>();

        // public IList<GateEntryExport> listExport = new List<GateEntryExport>();

        public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    }

    public class Dnd_CCINList
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

    public class Dnd_AddMoreRefForCCIN
    {
        public int addRefId { get; set; } = 0;
        public string addRefNo { get; set; } = string.Empty;
        public int addRefpkg { get; set; } = 0;
    }


}