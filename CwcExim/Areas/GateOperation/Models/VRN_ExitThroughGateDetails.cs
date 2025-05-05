using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VRN_ExitThroughGateDetails
    {
        public int ExitIdDtls { get; set; }

        public int ExitIdHeader { get; set; }


        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Reefer")]
        public bool Reefer { get; set; }

        [Display(Name = "Shipping Line")]
        public string ShippingLine { get; set; }

        [Display(Name = "CHA Name")]
        public string CHAName { get; set; }

        [Display(Name = "Cargo Description")]
        public string CargoDescription { get; set; }

        [Display(Name = "Cargo Type")]
        public int? CargoType { get; set; }

        [Display(Name = "Vehicle No")]
        public string VehicleNo { get; set; }      

        [Display(Name = "No of Packages")]
        public int? NoOfPackages { get; set; }

        [Display(Name = "Gross Weight")]
        public decimal? GrossWeight { get; set; }

        [Display(Name = "Depositor Name")]
        public string DepositorName { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public int ShippingLineId { get; set; }
        public int BranchId { get; set; }
        public string CFSCode { get; set; }
        public string OperationType { get; set; }
    }
}