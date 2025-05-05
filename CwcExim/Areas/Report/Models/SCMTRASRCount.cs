using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class SCMTRASRCount
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string StuffingNo { get; set; }
        public string NoofACK { get; set; }
        public string Fail { get; set; }
        public string Sucess { get; set; }
        public string NoOfASR { get; set; }
        public string NotRecieved { get; set; }
        public string NoOfShippingBill { get; set; }        
        public string ondate { get; set; }

    }
}