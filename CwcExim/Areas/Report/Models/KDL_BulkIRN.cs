
using System.Collections.Generic;

namespace CwcExim.Areas.Report.Models
{
    public class KDL_BulkIRN 
    {        
        public IList<KDL_BulkIRNDetails> lstPostPaymentChrg { get; set; } = new List<KDL_BulkIRNDetails>();
    }
   
    public class KDL_BulkIRNDetails
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