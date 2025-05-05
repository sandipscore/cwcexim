using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class HDBGodownWiseLocation
    {
        public int LocationId { get; set; } 
        public int GodownId { get; set; }

       // [Required(ErrorMessage ="Fill Out This Field")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",ErrorMessage = "Location Name Can Contain Only Alphabets And Numeric Digits")]
        public string LocationName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Row Can Contain Only Alphabets And Numeric Digits")]
        public string Row { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Column Can Contain Only Alphabets And Numeric Digits")]
        public string Column { get; set; }
    }
}