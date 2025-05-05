using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRDailyPdaActivityReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public List<DSRDailyPdaActivityList> LstDailyPdaActivityReport { get; set; } = new List<DSRDailyPdaActivityList>();

    }
    public class DSRDailyPdaActivityList
    {
        public string Party { get; set; }
        public string partycode { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }

        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
    }
}