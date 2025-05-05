using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_PdSummary
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
       // public string PeriodTo { get; set; }

       

        public string PartyName { get; set; }

        // public string ContainerNo { get; set; }


        public string Amount { get; set; }

       


        //public string value { get; set; }
            }

   

}