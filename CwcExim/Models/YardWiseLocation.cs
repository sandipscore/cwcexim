using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class YardWiseLocation
    {
        public int LocationId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 ]",ErrorMessage = "Location Name Can Contain Only Alphabets And Numeric Digits")]
        public string LocationName { get; set; }
        public int YardId { get; set; }

        [RegularExpression(@"^[a-zA-Z]",ErrorMessage = "Row Can Contain Only Alphabets")]
        [StringLength(10, ErrorMessage = "Row Cannot Be More Than 10 Characters In Length")]
        public string Row { get; set; }

        [RegularExpression(@"^[0-9]+$",ErrorMessage = "Column Can Contain Only Numeric Digits")]
        [Range(1,99999999999,ErrorMessage = "Column Should Be Between 1-99999999999")]
        public int Column { get; set; }
    }
}