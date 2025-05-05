using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class TallyResponse
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
         
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

        public string Date { get; set; }
        public string Bill { get; set; }
        public string Invoice { get; set; }
        public string Dr { get; set; }
        public string Cr { get; set; }
        public string Receipt { get; set; }
        

    }
}