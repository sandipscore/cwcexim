using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class DSR_RevalidateDOValidity
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int CustomAppraisementId { get; set; }
        public string ExpiryDT { get; set; }
        public string DeliveryDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExtendDT { get; set; }
        public string RevalidateDate { get; set; }
    }
}