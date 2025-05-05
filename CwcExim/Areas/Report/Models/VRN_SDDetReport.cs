using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_SDDetReport
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }
    public class VRN_PartyForSDDet
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string PartyCode { get; set; }

    }

    public class VRN_SDDetailsStatement
    {
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public string CompanyGst { get; set; }
        public string CompanyAddress { get; set; }
        public decimal? UtilizationAmount { get; set; }
        public List<VRN_SDInvoiceDet> lstInvc { get; set; } = new List<VRN_SDInvoiceDet>();
    }

    public class VRN_SDInvoiceDet
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