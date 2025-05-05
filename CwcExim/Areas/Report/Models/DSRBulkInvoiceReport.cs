using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRBulkInvoiceReport
    {
        public string InvoiceModule { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string InvoiceNumber { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public int PartyId { get; set; }
        public string PartyName { get; set; }

        public string InvoiceModuleName { get; set; }

        public int PayeeId { get; set; }
        public string PayeeName { get; set; }


        //****************************************************
        public string ShippingLine { get; set; } = "";
        public int ShippingLineId { get; set; } = 0;

        public string CHAName { get; set; } = "";
        public int CHAId { get; set; } = 0;

        public string All { get; set; }

    }

    public class DSRinvoiceLIst
    {
        public string InvoiceNumber { get; set; }
        public string Module { get; set; }
    }
   


}