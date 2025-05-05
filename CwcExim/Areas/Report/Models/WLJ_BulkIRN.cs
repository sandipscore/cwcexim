using System.Collections.Generic;

namespace CwcExim.Areas.Report.Models
{
    public class WLJ_BulkIRN 
    {        
        public IList<WLJ_BulkIRNDetails> lstPostPaymentChrg { get; set; } = new List<WLJ_BulkIRNDetails>();
    }
   
    public class WLJ_BulkIRNDetails
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