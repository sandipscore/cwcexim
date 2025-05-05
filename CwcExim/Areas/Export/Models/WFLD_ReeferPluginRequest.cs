using CwcExim.Areas.Bond.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_ReeferPluginRequest
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
    public class WFLD_ContainerDetailsReefer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public int Hours { get; set; }
        public int Size { get; set; }
        public bool Selected { get; set; }
        public int LoadContainerId { get; set; }
        public string PlugInDatetime { get; set; }
        public string PlugOutDatetime { get; set; }
    }
    public class WFLD_ReeferInv : WFLD_InvoiceBase
    {
        public string ExportUnder { get; set; } = "";
        public string SEZ { get; set; } = "";
        public List<WFLD_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLD_PreInvoiceContainer>();
        public List<WFLD_PostPaymentContainerRef> lstPostPaymentCont { get; set; } = new List<WFLD_PostPaymentContainerRef>();
        public List<WFLD_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLD_PostPaymentChrg>();
        public IList<WFLD_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLD_ContainerWiseAmount>();
        public List<WFLD_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLD_OperationCFSCodeWiseAmount>();
    }
    public class WFLD_PostPaymentContainerRef : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
        public string PlugInDatetime { get; set; }
        public string PlugOutDatetime { get; set; }
        public int Shift { get; set; }
    }

    public class WFLD_ReeferInvoiceList
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }

    }

    public class WFLD_ReeferContainerDet
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
        public string  FromDate { get; set; }
        public string ToDate { get; set; }
        public string GST { get; set; }
        public string PartyState { get; set; }

    }
}