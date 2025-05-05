using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_InternalMovement
    {
        public int MovementId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string CargoDescription { get; set; }
        public int? NoOfPackages { get; set; }
        public decimal? GrossWeight { get; set; }
        public int FromGodownId { get; set; }
        public string OldLocationIds { get; set; }
        public string OldLctnNames { get; set; }
        public int ToGodownId { get; set; }
        public string NewLocationIds { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewLctnNames { get; set; }
        public string OldGodownName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string NewGodownName { get; set; }
    }
}