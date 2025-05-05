using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class Dnd_Internal_Movement
    {
        public int MovementId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int DestuffingEntryId { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string CargoDescription { get; set; }
        public int? NoOfPackages { get; set; }
        public decimal? GrossWeight { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int? NewNoOfPackages { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal? NewGrossWeight { get; set; }
        public int FromGodownId { get; set; }
        public string OldLocationIds { get; set; }
        public string OldLocationNames { get; set; }
        public int ToGodownId { get; set; }
        public string NewLocationIds { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewLocationNames { get; set; }
        public string OldGodownName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewGodownName { get; set; }


        public decimal OldCIFValue { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal NewCIFValue { get; set; }
        public decimal OldGrossDuty { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal NewGrossDuty { get; set; }
        public decimal OldArea { get; set; }//SQM
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal NewArea { get; set; }//SQM
        public decimal OldAreaCbm { get; set; }//CBM
        public decimal NewAreaCbm { get; set; }//CBM


        public IList<DSRCharges_inv> Charges { get; set; } = new List<DSRCharges_inv>();
        public decimal TotalCharges { get; set; } = 0;
        public string Party { get; set; }
        public string Invoice { get; set; }
        public int DeliveryCount { get; set; }
        public int StockDetailsId { get; set; } = 0;



    }
    public class DndGodownListWithDestiffDetails
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownNo { get; set; }
        public int DstuffReceivedPackages { get; set; }
        public decimal DestuffWeight { get; set; }
        public decimal DstuffSQM { get; set; }
        public decimal DstuffCBM { get; set; }
        public decimal DstuffCIFValue { get; set; }
        public decimal DstuffGrossDuty { get; set; }

        public decimal FDstuffSQM { get; set; }
        public decimal FDstuffCBM { get; set; }
        public decimal FDestuffWeight { get; set; }
        public decimal FDstuffReceivedPackages { get; set; }

        public string LocationIds { get; set; }
        public string LocationNames { get; set; }



    }
    public class DndGodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownNo { get; set; }



    }

    public class DndCharges_inv
    {
        // public int DBChargeID { get; set; }
        // public string ChargeId { get; set; }
        // public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGSTAmt { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGSTAmt { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGSTAmt { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }
}