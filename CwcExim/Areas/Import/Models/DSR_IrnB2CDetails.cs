using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_IrnB2CDetails
    {
        public string SellerGstin { get; set; }
        public string BuyerGstin { get; set; }
        public string DocNo { get; set; }
        public string DocTyp { get; set; }
        public string DocDt { get; set; }
        public int TotInvVal { get; set; }
        public int ItemCnt { get; set; }
        public string MainHsnCode { get; set; }
        public string Irn { get; set; }
        public string IrnDt { get; set; }
        public string iss { get; set; } //
        public string ver { get; set; }
        public int orgId { get; set; }
        public int mode { get; set; }
        public string tid { get; set; }

        public string tr { get; set; }
        public string tn { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public string am { get; set; }
        public string mam { get; set; }
        public string mid { get; set; }
        public string msid { get; set; }
        public string mtid { get; set; }
        public string gstBrkUp { get; set; }
        public int qrMedium { get; set; }

        public string invoiceNo { get; set; }

        // public string InvoiceDate { get; set; }
        public string InvoiceName { get; set; }
        public string QRexpire { get; set; }

        public int pinCode { get; set; }
        public string tier { get; set; }

        public string gstIn { get; set; }


        public string sign { get; set; }

        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal GSTIncentive { get; set; }
        public decimal GSTPCT { get; set; }
    }
}