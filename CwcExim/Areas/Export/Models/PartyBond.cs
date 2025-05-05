using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class PartyBond
    {
        public int PartyBondId { get; set; } = 0;

        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(45,ErrorMessage = "Tr No Cannot Be More than 45 Characters")]
        public string TrNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(minimum:0,maximum:9999999999999999.99, ErrorMessage = "Value Should Be Between 0-9999999999999999.99")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ValidUpto { get; set; }
        public int BranchId { get; set; }
        public int Uid { get; set; }
    }

    public class PartyBondList
    {
        public int PartyBondId { get; set; }
        public string TrNo { get; set; }
        public string TrDate { get; set; }
        public string PartyName { get; set; }
        public decimal Value { get; set; }
        public string ValidUpto { get; set; }

    }
}