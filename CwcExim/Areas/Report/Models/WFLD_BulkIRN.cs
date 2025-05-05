using System.Collections.Generic;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_BulkIRN 
    {        
        public IList<WFLD_BulkIRNDetails> lstPostPaymentChrg { get; set; } = new List<WFLD_BulkIRNDetails>();
    }
   
    public class WFLD_BulkIRNDetails
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string GstNo { get; set; }
        public string SupplyType { get; set; }
        public string InvoiceType { get; set; }
        public string OperationType { get; set; }
    }
}