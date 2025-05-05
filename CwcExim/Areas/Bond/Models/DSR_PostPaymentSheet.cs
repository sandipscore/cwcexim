using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class DSR_PostPaymentSheet : PostPaymentSheet
    {  
        public string BOLNo { get; set; } = string.Empty;
        public string BOLDate { get; set; } = string.Empty;
        public int IsLocalGST { get; set; } = 0;
        public List<DSR_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSR_PreInvoiceContainer>();
        public List<DSR_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSR_OperationCFSCodeWiseAmount>();
    }
}