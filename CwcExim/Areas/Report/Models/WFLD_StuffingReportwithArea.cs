using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_StuffingReportwithArea
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EntryNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SLA { get; set; }
        public string CustomSeal { get; set; }
        public decimal Area { get; set; }
        public decimal Weight { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string Transporter { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string VehicleNo { get; set; }
        public string InvoiceNo { get; set; }
        public int TotalSB { get; set; }
        public decimal TotalWeight { get; set; }
        public string StuffingNo { get; set; }
        public string Forwarder { get; set; }
        public decimal TotalArea { get; set; }
        public int TotalShipBillCount { get; set; }

    }
}