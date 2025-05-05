using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_ReeferPluginRequest
    {
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }
        public string Date { get; set; }
        public string GSTNo { get; set; }
        public string CHA { get; set; } = string.Empty;
        public string Exporter { get; set; } = string.Empty;
        public string PartyName { get; set; }
        public int PartyId { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string ContainerNo { get; set; }
    }
    public class Hdb_ContainerDetails
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public int Hours { get; set; }
        public bool Selected { get; set; }
        public string PlugInDatetime { get; set; }
        public string PlugOutDatetime { get; set; }
    }
    public class Hdb_ReeferInv:Hdb_InvoiceBase
    {
        public string ExportUnder { get; set; } = "";
        public int Distance { get; set; }
        public List<Hdb_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Hdb_PreInvoiceContainer>();
        public List<Hdb_PostPaymentContainerRef> lstPostPaymentCont { get; set; } = new List<Hdb_PostPaymentContainerRef>();
        public List<Hdb_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Hdb_PostPaymentChrg>();
        public IList<Hdb_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Hdb_ContainerWiseAmount>();
        public List<Hdb_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Hdb_OperationCFSCodeWiseAmount>();
    }
    public class Hdb_PostPaymentContainerRef: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
        public string PlugInDatetime { get; set; }
        public string PlugOutDatetime { get; set; }
        public int Shift { get; set; }
    }
}