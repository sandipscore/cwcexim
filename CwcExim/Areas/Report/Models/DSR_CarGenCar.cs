using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_CarGenCar
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
    }
    public class DSR_ExpCarGen
    {
        public string EntryNo { get; set; }
        public string InDate { get; set; }
        public string SbNo { get; set; }
        public string SbDate{ get; set; }
        public string Shed { get; set; }
        public decimal  Area { get; set; }
        public int NoOfDays { get; set; }
        public int NoOfWeek { get; set; }
        public decimal GeneralAmount { get; set; }
       
    }
}