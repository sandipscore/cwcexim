using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_JsonRespnStatusModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public string ReceiptNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Message { get; set; }
        public string jsonEInvoice { get; set; }
        public string SendStatus { get; set; }
        public string StatusCode { get; set; }


        public List<LNSM_JsonRespnStatusModel> ListInvc { get; set; }
    }
}