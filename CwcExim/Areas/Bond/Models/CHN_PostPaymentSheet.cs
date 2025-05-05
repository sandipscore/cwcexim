using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class CHN_PostPaymentSheet:PostPaymentSheet
    {
        public string BOLNo { get; set; } = string.Empty;
        public string BOLDate { get; set; } = string.Empty;
        public List<CHN_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<CHN_PreInvoiceContainer>();
        public List<CHN_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<CHN_OperationCFSCodeWiseAmount>();

        public string SEZ { get; set; }
        public decimal Discount { get; set; } = 0M;
    }
    public class CHNListOfBondInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
    }
}