using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDDailyPdaActivityReport
    {       
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public List<WFLDDailyPdaActivityList> LstDailyPdaActivityReport { get; set; } = new List<WFLDDailyPdaActivityList>();

    }
    public class WFLDDailyPdaActivityList
    {
        public string Party { get; set; }
        public string partycode { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }

        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
    }
}