using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DSRPartyWiseReservation
    {
        public int PartyReservationId { get; set; }
        public int Uid { get; set; }
        public int PartyId { get; set; }
        public int BranchId { get; set; }

        public string PartyCode { get; set; }

        [Display(Name = "Party Name")]
        [Required]
        public string PartyName { get; set; }

        [Display(Name = "Reservation To")]
        [Required]
        public string ReservationTo { get; set; }

        [Display(Name = "Reservation From")]
        [Required]
        public string ReservationFrom { get; set; }

        [Display(Name = "Operation Type")]
        [Required]
        public string OperationType { get; set; } 
        public int GodownId { get; set; }

        [Display(Name = "Godown Name")]
        [Required]
        public string GodownName { get; set; }

        public decimal GF { get; set; }
        public decimal MF { get; set; }
        public decimal TotalSpace { get; set; }
        //public int ResMonths { get; set; }
        public string ResType { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string AreaType { get; set; }
    }

    public class DSRReservationGodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public bool OperationTypeImport { get; set; }
        public bool OperationTypeExport { get; set; }
        public bool OperationTypeBond { get; set; }
    }
    
}