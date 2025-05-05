using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDSDStatement
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int Year { get; set; }
        public decimal TotalAmount { get; set; } = 0M;
        public decimal OpeningAmount { get; set; } = 0M;
        public decimal Collections { get; set; } = 0M;
        public decimal Adjustment { get; set; } = 0M;

        public decimal closingAmount { get; set; } = 0M;

        public List<WFLDSDList> LstSD { get; set; } = new List<WFLDSDList>();
    }

   
}