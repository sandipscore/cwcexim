using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class PPG_Internal_MovementV2
    {
        public int MovementId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int DestuffingEntryId { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string LocationId { get; set; }
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
        public IList<Charges_inv> Charges { get; set; } = new List<Charges_inv>();
        public decimal TotalCharges { get; set; } = 0;
        public string Party { get; set; }
        public string Invoice { get; set; }
        public int TransferId { get; set; }
        public string Status { get; set; }
        public string TransferNo { get; set; }
        public string TransferDate { get; set; }
    }
}