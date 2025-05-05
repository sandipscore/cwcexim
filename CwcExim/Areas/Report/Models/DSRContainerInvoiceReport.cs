using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class DSRContainerInvoiceReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }

        public Int32 InvoiceId { get; set; }
    }
}