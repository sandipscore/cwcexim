using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VIZ_ListStuffingInvoice
    {
        public string SInvoiceId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int StuffingId { get; set; }
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
        public string ContainerNo { get; set; }
        public string SupplyType { get; set; }
    }
}