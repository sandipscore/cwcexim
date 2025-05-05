using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class ReserveSpaceReport
    {
        public int SLNo { get; set; }
        public string BillingDate { get; set; }
        public string BillingNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }

        public string PartyName { get; set; }

        public string PartyCode { get; set; }
        public string Month { get; set; }

        public decimal Area { get; set; }

        public decimal Rate { get; set; }

        public decimal ReservationAmount { get; set; }

        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TotalAMount { get; set; }
        public decimal AmountReceivable { get; set; }
        public string Remarks { get; set; }


        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }

    }


}