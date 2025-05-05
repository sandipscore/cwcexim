
using System.Collections.Generic;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_BulkIRN 
    {        
        public IList<HDB_BulkIRNDetails> lstPostPaymentChrg { get; set; } = new List<HDB_BulkIRNDetails>();
    }
   
    public class HDB_BulkIRNDetails 
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