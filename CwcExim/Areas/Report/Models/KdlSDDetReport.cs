using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class KdlSDDetReport
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }

    public class PartyForSDDetail
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string PartyCode { get; set; }

    }

    public class KdlSDDetailsStatement
    {
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public string CompanyGst { get; set; }
        public decimal? UtilizationAmount { get; set; }
        public List<KdlSDInvoiceDet> lstInvc { get; set; } = new List<KdlSDInvoiceDet>();
    }

    public class KdlSDInvoiceDet
    {
        public int SL { get; set; }

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public decimal? ReceiptAmt { get; set; }
        public string TranType { get; set; }
        public decimal? TranAmt { get; set; }
    }
}