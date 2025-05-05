using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class ChargeCalculator
    {
        [Required]
        public string OBLNo { get; set; }

        [Required]
        public string CIFValue { get; set; }

        [Required]
        public string Duty { get; set; }
    }

    public class ChargeDetailsCal
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}