using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class HDBHoliday
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Discription Required")]
        public string Discription { get; set; }
       [Required(ErrorMessage = "Date Required")]
        public string Date { get; set; }
    }
}