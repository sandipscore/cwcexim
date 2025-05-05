using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class TempRakeWagonHdr
    {
        public string InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string WgonDetailsJS { get; set; }
    }
    public class TempWgonDtl
    {
        public string WagonNo { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerFlag { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerPosition { get; set; }
        public string CommodityLoaded { get; set; }
        public string TrafficType { get; set; }
        public string CommodityStatisticalCode { get; set; }
        public string ContainerTareWeight { get; set; }
        public string ContainerLoadedWeight { get; set; }
        public string SMTPNumber { get; set; }
        public string SMTPDate { get; set; }
        public string HSNCode { get; set; }

    }
}