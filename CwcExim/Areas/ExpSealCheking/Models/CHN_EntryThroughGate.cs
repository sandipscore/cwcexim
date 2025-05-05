using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_EntryThroughGate
    {
      
            public int EntryId { get; set; }

            [Display(Name = "Gate In No")]
            public string GateInNo { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            public string EntryDateTime { get; set; }

            [Display(Name = "Truck Slip No")]
            public string TruckSlipNo { get; set; }

            //[Required(ErrorMessage = "Fill Out This Field")]

            [Display(Name = "Truck Slip Date")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string TruckSlipDate { get; set; }

            [Display(Name = "Container No")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string ContainerNo { get; set; }


            [Display(Name = "Size")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string Size { get; set; }

            [Display(Name = "CHA:")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string CHAName { get; set; }
            public int CHAId { get; set; }

            public int ExporterId { get; set; }

            //[Display(Name = "Container No")]
            ////[Required(ErrorMessage = "Fill Out This Field")]
            //public string ContainerNo { get; set; }

            [Display(Name = "Custom Seal No")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string CustomSealNo { get; set; }

            [Display(Name = "Vehicle No")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string VehicleNo { get; set; }

            [Display(Name = "Driving License No")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string DrivingLicenseNo { get; set; }

            [Display(Name = "Cargo Description")]
            [MaxLength(1000)]
            public string CargoDescription { get; set; }

            [Display(Name = "Cargo Type")]
            [Required(ErrorMessage = "Select Cargo Type")]
            //[Range(1, int.MaxValue, ErrorMessage = "Select Cargo Type")]
            public int? CargoType { get; set; }

            [Display(Name = "No of Packages")]
            public int? NoOfPackages { get; set; }

            [Display(Name = "Gross Weight")]
            public decimal? GrossWeight { get; set; }

            [Display(Name = "Exporter")]
            [Required(ErrorMessage = "Fill Out This Field")]
            public string Exporter { get; set; }

            [Display(Name = "Remarks")]
            [RegularExpression("^[A-Za-z0-9\\-,./\r\n:'&?() ]*$", ErrorMessage = "Invalid Character.")]
            [MaxLength(1000)]
            public string Remarks { get; set; }

            public string Time { get; set; }
            public string EntryTime { get; set; }

            public string SystemDateTime { get; set; }

            public int Uid { get; set; }

            public string ContainerNo1 { get; set; }

            public string CFSCode { get; set; }

            public string ReferenceNo { get; set; }

            public string ReferenceDt { get; set; }

            public int CBT { get; set; }

            public int IsODC { get; set; }

            public string ShippingLine { get; set; }

            public string Reefer { get; set; }

            public string ShippingLineSealNo { get; set; }

            public string ChallanNo { get; set; }

            public string DepositorName { get; set; }

            public int BranchId { get; set; }

            public string ContainerType { get; set; }

            public string OperationType { get; set; }

            public string LCLFCL { get; set; }

            public string PrintSealCut { get; set; } = string.Empty;
            public string CBTContainer { get; set; } = string.Empty;

            public string ExamRequired { get; set; }

            public string CustomId { get; set; }           

    }


    public class CHA
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }

    public class Exporter
    {
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
    }



    public class PrintTruckSlip
    {
        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

        public string GateInNo { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string Exporter { get; set; }

        public string ChaName { get; set; }

        public string Entrydate { get; set; }

        public string EntryTime { get; set; }

        public string CustomSealNo { get; set; }

        public string Remarks { get; set; }

        public string NoOfUnits { get; set; }

        public string Cargo { get; set; }

        public string VehicleNo { get; set; }
    }
}