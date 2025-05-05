using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Chn_AgewiseBreakUp
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }




        // #listforAgewiseBreakUp 
        public int opnNOOfBound { get; set; }
        public decimal opnDuty { get; set; }
        public int ReciptNOOfBound { get; set; }
        public decimal ReciptDuty { get; set; }
        public int DipNOOfBound { get; set; }
        public decimal DipDuty{ get; set; }
        public decimal NoFBond { get; set; }
        public decimal DutyAmount { get; set; }
       



    }
}