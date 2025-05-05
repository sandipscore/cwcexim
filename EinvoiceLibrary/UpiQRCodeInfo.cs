using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class UpiQRCodeInfo
    {
        public string ver { get; set; }
        public int orgId { get; set; }
        public int mode { get; set; }
        public string tid { get; set; }
      
        public string tr { get; set; }
        public string tn { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public decimal am { get; set; }
        public decimal mam { get; set; }
        public string mid { get; set; }
        public string msid { get; set; }
        public string mtid { get; set; }
        public string gstBrkUp { get; set; }
        public int qrMedium { get; set; }

        public string invoiceNo { get; set; }

        public string InvoiceDate { get; set; }
        public string InvoiceName { get; set; }
        public string QRexpire { get; set; }

        public int pinCode { get; set; }
        public string tier { get; set; }

        public string gstIn { get; set; }
       

        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal GSTIncentive { get; set; }
        public decimal GSTPCT { get; set; }

        public int merchant_id { get; set; }
        public int order_id { get; set; }


        public string redirect_url { get; set; }
        public string cancel_url { get; set; }
        public string language { get; set; }

        public string billing_name { get; set; }
        public string billing_address { get; set; }

        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public string billing_tel { get; set; }
        public string billing_email { get; set; }

        public string delivery_tel { get; set; }
        public int merchant_param1 { get; set; }

    }
}
