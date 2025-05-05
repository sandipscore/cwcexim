using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRPartyInsurance
    {
        public int PartyInsuranceId { get; set; }
        public int Uid { get; set; }
        public int PartyId { get; set; }
        public int BranchId { get; set; }

        public string PartyCode { get; set; }

        [Display(Name = "Party Name")]
        public string PartyName { get; set; }

        [Display(Name = "Insurance To")]      
        public string InsuranceTo { get; set; }

        [Display(Name = "Insurance From")]
        public string InsuranceFrom { get; set; }
    }
    public class DSRPartylistForPage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
    }
}