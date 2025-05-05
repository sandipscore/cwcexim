using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class CHNMiscellaneous
    {
        public int MiscellaneousId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999.99, ErrorMessage = "Fumigation Should Be More Than 0 And Less Than Or Equal To 99999.99")]
        public decimal Fumigation { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999.99, ErrorMessage = "Washing Should Be Less Than Or Equal To 99999.99")]
        public decimal Washing { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999.99, ErrorMessage = "Reworking Should Be  Less Than Or Equal To 99999.99")]
        public decimal Reworking { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999.99, ErrorMessage = "Bagging Should Be  Less Than Or Equal To 99999.99")]
        public decimal Bagging { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999.99, ErrorMessage = "Palletizing Should Be  Less Than Or Equal To 99999.99")]
        public decimal Palletizing { get; set; }
        [Range(0, 99999.99, ErrorMessage = "Palletizing Should Be  Less Than Or Equal To 99999.99")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal PrintingCharges { get; set; }
        public string EffectiveDate { get; set; }
        public int Uid { get; set; }

        [Display(Name = "SAC Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
    }
}