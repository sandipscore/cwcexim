using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_SACRegister
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string ValidUpto { get; set; }
        public string IMPName { get; set; }
        public string CHAName { get; set; }
        public string BOLAWBNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string CargoDescription { get; set; }
        public string NoOfUnits { get; set; }
        public string AreaReserved { get; set; }
        public string InvoiceAmt { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string  Remarks { get; set; }
        public string SealNo { get; set; } = string.Empty;
    }
    public class VRN_SAC
    {
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
    }
}