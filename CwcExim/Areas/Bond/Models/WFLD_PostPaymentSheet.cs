using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class WFLD_PostPaymentSheet:PostPaymentSheet
    {
        public string BOLNo { get; set; } = string.Empty;
        public string BOLDate { get; set; } = string.Empty;
        public int IsLocalGST { get; set; } = 0;
        public List<WFLD_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLD_PreInvoiceContainer>();
        public List<WFLD_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLD_OperationCFSCodeWiseAmount>();
        public string StorageFromDate { get; set; }
        public string StorageToDate { get; set; }
        public decimal CalcArea { get; set; }
        public string InsuranceFromDate { get; set; }
        public string InsuranceToDate { get; set; }
        public decimal CalcCIFValue { get; set; }
        public decimal CalcDuty { get; set; }
    }
}