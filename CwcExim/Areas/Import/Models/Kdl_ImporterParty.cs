using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class Kdl_ImporterParty
    {

        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyCode { get; set; }
        public bool IsInsured { get; set; }
        public bool IsTransporter { get; set; }
        public string InsuredFrmDate { get; set; }
        public string InsuredToDate { get; set; }

    }


    public class Kdl_EmptyMovement
    {
        public int EntryId { get; set; }
        [Required(ErrorMessage = "Please Select Container No")]
        public string ContainerNo { get; set; }
        public string CfsCode { get; set; }
        public string Size { get; set; }

        public string EmptyDate { get; set;}
        public string ShippingLineName { get; set; }
        public int ShippingLineId { get; set; }
        public int? MovementId { get; set; } = 0;
        public string MovementEntryDate { get; set; }
        public String Remarks { get; set; }
    }
}