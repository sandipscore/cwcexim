using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_RentIncomeReportViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string BillingDate { get; set; }

        public string BillingNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public string PartyName { get; set; }

        public string PartyCode { get; set; }

        public string Month { get; set; }
        public string TDSCertification { get; set; }
        public Decimal RentReceived { get; set; }
        public Decimal TDSAmount { get; set; }
        public Decimal SGST { get; set; }
        public Decimal CGST { get; set; }

        public Decimal IGST { get; set; }

        public Decimal TotalAmountReceived { get; set; }

        public Decimal AmountOutstanding { get; set; }

        public string Remarks { get; set; }
    }
}