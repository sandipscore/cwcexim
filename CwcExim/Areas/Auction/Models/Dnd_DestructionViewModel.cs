using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_DestructionViewModel
    {
        public int? DestructionID { get; set; }
        public string RefFlag { get; set; }
        public string Type { get; set; }

        [Required]
        public string RefNo { get; set; }
        public string RefDate { get; set; }

        [Required]
        public string GodownID { get; set; }

        [Required]
        public string GodownName { get; set; }

        [Required]
        public string CustomGatePassNo { get; set; }

        [Required]
        public string DestructionAgencyName { get; set; }

        public string DestructionDate { get; set; }

        [Required]
        public string VehicleNo { get; set; }

        [Required]
        public string ShedNo { get; set; }    

        public string Remarks { get; set; }

        public string Shipbill { get; set; }
        public string OBL { get; set; }

        public string ContainerNo { get; set; }
    }
}