using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class WFLD_SlabWiseDeliveryReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string FirstSlab { get; set; }
        public string SecondSlab { get; set; }
        public string ThirdSlab { get; set; }
        public string FourthSlab { get; set; }

         
    }
}