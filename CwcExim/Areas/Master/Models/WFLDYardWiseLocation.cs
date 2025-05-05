using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDYardWiseLocation
    {
        public int LocationId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 ]", ErrorMessage = "Location Name Can Contain Only Alphabets And Numeric Digits")]
        public string LocationName { get; set; }
        public int YardId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]", ErrorMessage = "Row Can Contain Alphabets,Numeric Digits")]
        public string Row { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]", ErrorMessage = "Columns Name Can Contain Alphabets,Numeric Digits")]
        public string Column { get; set; }
    }
}