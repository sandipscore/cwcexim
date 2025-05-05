using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VIZ_EntryThroughGate
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
       

        [Display(Name = "Reference Date")]
        public string ReferenceDate { get; set; }

        [Display(Name = "Shipping Line")]
        [Required(ErrorMessage = "Select Shipping Line")]
        public string ShippingLine { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Shipping Line Id Not Found.")]
        public int ShippingLineId { get; set; }

        [Display(Name = "CHA Name")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

        [Display(Name = "Container No")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }



        [Display(Name = "Container No 1")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo1 { get; set; }


        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "Reefer")]       
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
        public bool IsCBT { get; set; } = false;

        public string TPNo { get; set; }

        public string SystemDateTime { get; set; }

        public string JobOrderDate { get; set; }

        public string JobOrderNo { get; set; }

        public bool IsOdc { get; set; }
        public int TransPortFromID { get; set; }
        public bool OnWheel { get; set; }

        public bool Bond { get; set; }
        [Display(Name = "Forwarder/Consolidator")]
        public string Forwarder { get; set; }
        public int ForwarderId { get; set; }
        public string ContClass { get; set; }
    }

    public class VIZ_container
    {
        public string ContainerName { get; set; }


    }
    public class VIZ_SearchCont
    {
        public List<WFLDcontainer> lstConatiner { get; set; } = new List<WFLDcontainer>();
        public string State { get; set; }
    }
    public class VIZ_EntryThroughGateShipping
    {

        public string ShippingLine { get; set; }


        public int ShippingLineId { get; set; }
    }
    public class VIZ_LoadedContainerListWithData
    {

        public string ContainerNo { get; set; }
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
    public class VIZ_EntryThroughGateCHA
    {
        public string CHAName { get; set; }
        public int CHAId { get; set; }
    }
    public class VIZ_containerAgainstGp
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
        public string Module { get; set; }


    }

    public class VIZ_EntryThroughGateBond
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
        public decimal? GrossWeight { get; set; }
        public string DepositorName { get; set; }
        public string Time { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }

    }
}