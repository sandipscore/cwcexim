using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class ChnEntryThroughGate
    {
        public int EntryId { get; set; }


        public string DisPlayCfs { get; set; }
        [Display(Name = "ICD Code")]
        public string CFSCode { get; set; }

        [Display(Name = "Gate In No")]
        public string GateInNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string EntryDateTime { get; set; }

        public string TerminalOutDateTime { get; set; }

        [Display(Name = "Reference No")]
        [Required(ErrorMessage = "Select ReferenceNo")]
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

        [Display(Name = "Importer Name")]
        public string ImporterName { get; set; }
        public int ImporterId { get; set; }

        [Display(Name = "Container No 1")]
        public string ContainerNo1 { get; set; }


        [Display(Name = "Size")]
       // [Required(ErrorMessage = "Fill Out This Field")]
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

        public string JobOrderNo { get; set; }
        public bool IsOdc { get; set; } = false;
        public string StringifyData { get; set; } = string.Empty;
        public IList<CHN_AddMoreRefForCCIN> listAddRef = new List<CHN_AddMoreRefForCCIN>();

        public bool Onwheel { get; set; }
    }

    public class ChnEntryThroughGateShipping
    {

        public string ShippingLine { get; set; }


        public int ShippingLineId { get; set; }
    }
    public class ChnEntryThroughGateCHA
    {
        public string CHAName { get; set; }
        public int CHAId { get; set; }
    }
    public class CHN_AddMoreRefForCCIN
    {
        public int addRefId { get; set; } = 0;
        public string addRefNo { get; set; } = string.Empty;
        public int addRefpkg { get; set; } = 0;
    }

}