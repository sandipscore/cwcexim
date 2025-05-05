using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class IRNResponseModel
    {
        

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int InvoiceId { get; set; }

        public string  IRNDate { get; set; }

        public string InvoiceNumber { get; set; }
   
        public string IRNRefNo { get; set; }

        public string IRNResponse { get; set; }
        public string ErrorMessage { get; set; }

       
        public List<IRNResponseModel> ListInvc { get; set; }
    }



}
