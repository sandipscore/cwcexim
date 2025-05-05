using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
namespace CwcExim.Areas.Import.Models
{
    public class Hdb_PostPaymentSheet: PostPaymentSheet
    {
        public string BOLNo { get; set; } = string.Empty;
        public string BOLDate { get; set; } = string.Empty;
        public int ForwarderId { get; set; } = 0;
        public string ExportUnder { get; set; } = string.Empty;
        public string Forwarder { get; set; } = string.Empty;

        public string Distance { get; set; }
        public List<Hdb_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Hdb_PreInvoiceContainer>();
        public List<Hdb_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Hdb_OperationCFSCodeWiseAmount>();

    }
}