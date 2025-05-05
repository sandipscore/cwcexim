using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_OutstandingAmountReport
    {

       
        public string FromDate { get; set; }

        
        public string ToDate { get; set; }
        public string BillingDate { get; set; }
        public string BillingNo { get; set; }

        public string PartyName { get; set; }

        public string PartyCode { get; set; }

        public string Month { get; set; }

        public decimal Area { get; set; }

        public decimal SQM { get; set; }
        public decimal AmountReceivalbe { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
      




    }
}