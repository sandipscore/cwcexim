using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class VIZ_Internal_Movement
    {
        public int MovementId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int DestuffingEntryId { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string LocationName { get; set; }
        public string CargoDescription { get; set; }
        public int? NoOfPackages { get; set; }
        public decimal? GrossWeight { get; set; }
        public int FromGodownId { get; set; }
        public string OldLocationIds { get; set; }
        public string OldLctnNames { get; set; }
        public int ToGodownId { get; set; }
        public string NewLocationIds { get; set; }
       
        public string NewLctnNames { get; set; }
        public string OldGodownName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewGodownName { get; set; }


        public IList<VIZCharges_inv> Charges { get; set; } = new List<VIZCharges_inv>();
        public decimal TotalCharges { get; set; } = 0;
        public string Party { get; set; }
        public string Invoice { get; set; }


    }
    public class VIZGodownListWithDestiffDetails
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownNo { get; set; }
        public int DstuffReceivedPackages { get; set; }
        public decimal DestuffWeight { get; set; }
        public decimal DestuffResWeight { get; set; }
        public decimal DstuffSQM { get; set; }
        public decimal DstuffCBM { get; set; }
        public decimal DstuffCIFValue { get; set; }
        public decimal DstuffGrossDuty { get; set; }

        public decimal DstuffRevSQM { get; set; }



    }
    public class VIZGodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownNo { get; set; }


    





    }

    public class VIZCharges_inv
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