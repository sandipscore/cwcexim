using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRCWCFranchiseCharges
    {
        public int franchisechargeid { get; set; }
        public string ChargesFor { get; set; }

        public bool ODC { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Size")]
        public string ContainerSize { get; set; }

        [Display(Name = "SAC Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "FranchiseCharge Should Be Less Than Or Equal To 99999999.99")]
        public decimal FranchiseCharge { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "RoalstyCharge Should Be Less Than Or Equal To 99999999.99")]
        public decimal RoaltyCharge { get; set; }

        public int PortId { get; set; }
        [Display(Name = "Port Name")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PortName { get; set; }

        public IList<DSRPort> LstPort = new List<DSRPort>();

    }
}