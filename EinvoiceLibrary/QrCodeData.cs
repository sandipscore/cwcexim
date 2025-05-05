using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
   public class QrCodeData
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
        public string iss { get; set; }
    }
}
