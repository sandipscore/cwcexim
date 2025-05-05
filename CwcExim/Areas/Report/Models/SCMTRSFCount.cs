using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class SCMTRSFCount
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
         
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }


        public string StuffingNo { get; set; }
        public string NoofACK { get; set; }
        public string Fail { get; set; }
        public string Sucess { get; set; }
        public string NoOfSf { get; set; }
        public string NotRecieved { get; set; }

        public string NOOFStuffinf { get; set; }
        public string NOOFSFStuffinf { get; set; }
        public string ondate { get; set; }




    }
}