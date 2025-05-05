using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_StockRegisterReport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string DestuffDate { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string ContNo { get; set; }
        public string ArrivalDate { get; set; }
        public string Importer { get; set; }
        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string SMTPNo { get; set; }
        public string SMTPDate { get; set; }
        public decimal Pkg { get; set; }
        public decimal WT { get; set; }
        public decimal Area { get; set; }
        public decimal CBM { get; set; }
        public string Slot { get; set; }

        public decimal CIF { get; set; }
        public decimal StoreCharge { get; set; }
        public string HAZNoHAZ { get; set; }
        public string SLA { get; set; }

        public int Days { get; set; }

        public string DESC { get; set; }
        public int ShippingLineId { get; set; } = 0;
        public string ShippingLineName { get; set; }
    }
}