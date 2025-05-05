using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class ImportBondConversion
    {

        public int DestuffingId { get; set; }
        public string TransferNo { get; set; }
        [Required(ErrorMessage = "Select OBL No.")]
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int FromGodownId { get; set; }
        public string FromGodownName { get; set; }
        public string FromLocationId { get; set; }
        public string FromLocationName { get; set; }
        public int NoOfPkg { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal CUM { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
        public int GodownId { get; set; }

        [Required(ErrorMessage = "Select Godown")]
        public string GodownName { get; set; }
        public int SACId { get; set; }
        [Required(ErrorMessage = "Select SAC")]
        public string SACNo { get; set; }
        public string SACDate { get; set; }
        public int MovementId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int DestuffingEntryId { get; set; }
        public string MovementNo { get; set; }
        [Required(ErrorMessage = "Select Date")]
        public string MovementDate { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string CustomBondNo { get; set; }
        public string CustomBondDate { get; set; }
        public string CustomSealNo { get; set; }

    }

    public class OBLNoForBondConversion
    {
        public string OBLNo { get; set; }
        public int DestuffingId { get; set; }
    }

    public class LocationForBondTransfer
    {
        public int FromLocationId { get; set; }
        public string FromLocationName { get; set; }
        public int DestuffingEntryId { get; set; }
    }
}