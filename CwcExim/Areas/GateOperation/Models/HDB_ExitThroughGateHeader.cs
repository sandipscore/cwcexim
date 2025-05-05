using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class HDB_ExitThroughGateHeader
    {
        public int ExitIdHeader { get; set; }

        [Display(Name = "Gate Exit No")]
        public string GateExitNo { get; set; }

        [Display(Name = "Gate Pass No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GatePassNo { get; set; }

        [Display(Name = "Exit Date & Time")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GateExitDateTime { get; set; }

        [Display(Name = "Gate Pass Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GatePassDate { get; set; }
        
        public int Uid { get; set; }

        public int BranchId { get; set; }
        //public IList<ExitThroughGateDetails> listExitThroughGateDetails { get; set; } = new List<ExitThroughGateDetails>();

        public string StrExitThroughGateDetails { get; set; }

        public string Time { get; set; }

        public IList<containerExit> containerList { get; set; } = new List<containerExit>();

        public string shippingLineId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public int GatePassId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string VehicleNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;

       // public string CFSCode { get; set; }



    }


    //public class containerExit
    //{
    //    public string ContainerName { get; set; }

    //    public string shippingLine { get; set; }

    //    public string shippingLineId { get; set; }

    //    public string CFSCode { get; set; }
    //}


    //public class ExitThroughGateDetails    {

    //    public int ExitIdDtls { get; set; }

    //    public int ExitIdHeader { get; set; }



    //    [Display(Name = "Container No")]
    //    public string ContainerNo { get; set; }

    //    [Display(Name = "Size")]
    //    public string Size { get; set; }

    //    [Display(Name = "Reefer")]
    //    public bool Reefer { get; set; }

    //    [Display(Name = "Shipping Line")]
    //    public string ShippingLine { get; set; }

    //    [Display(Name = "CHA Name")]
    //    public string CHAName { get; set; }

    //    [Display(Name = "Cargo Description")]
    //    public string CargoDescription { get; set; }

    //    [Display(Name = "Cargo Type")]
    //    public int CargoType { get; set; }

    //    [Display(Name = "Vehicle No")]
    //    public string VehicleNo { get; set; }

    //    [Display(Name = "No Of Packages")]
    //    public string NoOfPackages { get; set; }

    //    [Display(Name = "Gross Weight")]
    //    public decimal GrossWeight { get; set; }

    //    [Display(Name = "Depositor Name")]
    //    public string DepositorName { get; set; }

    //    [Display(Name = "Remarks")]
    //    public string Remarks { get; set; }
    //}
}