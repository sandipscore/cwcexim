using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_Godown
    {
        public int GodownId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Godown Name")]
        [RegularExpression(@"^[a-zA-Z0-9\- ]*$", ErrorMessage = "Godown Name Can Contain Alphabets,Numeric Digits And Special Characters '-'")]
        public string GodownName { get; set; }

        public int Uid { get; set; }
    }
}