using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DailyTransactionExpLodRpt
    {
       
            public int SerialNo { get; set; }
            // public int InvoiceId { get; set; }           
            public string GateInDate { get; set; }
        public string GateInTime { get; set; }
        public string RRDate { get; set; }
            public string GateOutDate { get; set; }
        public string GateOutTime { get; set; }

        public int Days { get; set; }
            public string Exporter { get; set; }
            public string CHA { get; set; }
            public string CustodianCode { get; set; }
            public string VehicleNo { get; set; }
            public string ContainerNo { get; set; }
            public string ShippingBillNo { get; set; }
            public string ShippingBillDate { get; set; }
            public string LEODate { get; set; }
            public string ShippingLineName { get; set; }
            public int NoOFPkg { get; set; }
            public decimal GrossWeight { get; set; }
            public string POC { get; set; }
            public string PortName { get; set; }
            public string POD { get; set; }
            public string ExportUnder { get; set; }
            public int Size { get; set; }
            public string Type { get; set; }
            public decimal FOB { get; set; }
            public decimal InvoiceAmt { get; set; }

        }
    }
