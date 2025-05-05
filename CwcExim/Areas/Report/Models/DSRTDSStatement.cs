using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRTDSStatement
    {
        public decimal CWCTDS { get; set; }
        public decimal HTTDS { get; set; }
        public decimal TDS { get; set; }
        public decimal TDSCol { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptNo { get; set; }
        public decimal ReceivedTDS { get; set; }

    }
   
}