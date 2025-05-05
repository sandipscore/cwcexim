using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EinvoiceLibrary
{
    public class IrnResponse
    {
        public int Status { get; set; }
        public ErrorDetails ErrorDetails { get; set; }

        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string QRCodeImageBase64 { get; set; }
        public string IrnStatus { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }

    }
}
