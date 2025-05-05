using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_MiscChargePtyWise
    {
        public int ChargeId { get; set; }
        public string ChargesName { get; set; }
        public int StorageChargeId { get; set; }        

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Charge")]
        public string ChargeName { get; set; }
        public int SACId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Sac Code")]
        public string SacCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Operation Type")]
        public string OperationType { get; set; }
        public string ContainerType { get; set; }
        public string ContainerLoadType { get; set; }
        public string Size { get; set; }
        public bool IsOdc { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }
        public int Uid { get; set; }
        [Display(Name = "Custom Exam")]
        public string CartingType { get; set; }
        public int PartyID { get; set; }
        public string PartyName { get; set; }
    }
}