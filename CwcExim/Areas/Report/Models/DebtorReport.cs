using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DebtorReport
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
      
        public string Values { get; set; }
        
        public string ClosingBalance { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string PartyName { get; set; }

        public string partyNameId { get; set; }


        public string GstNo { get; set; }
      


    }

    public class EximTraderWithInvoice
    {
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }


        public string GstNo { get; set; }
    }


}