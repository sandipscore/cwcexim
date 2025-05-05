using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_StockRegisterModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Decimal StorageCharge { get; set; }
        public string EntryNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public string Exporter { get; set; }
        public string CartingDate { get; set; }
        public decimal NoPkg { get; set; }
        public decimal Fob { get; set; }
        public decimal GrossWgt { get; set; }
        public decimal CBM { get; set; }
        public string SlotNo { get; set; }

    }
}