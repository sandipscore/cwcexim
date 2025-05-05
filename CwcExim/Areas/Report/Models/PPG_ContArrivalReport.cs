using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_ContArrivalReport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ContainerSize { get; set; }
        public int SLNO { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCODE { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public string ModeOfTransport { get; set; }
        public string CustomSealNo { get; set; }
        public string Remarks { get; set; }
    
        public string VehicleNo { get; set; }

        public string GateInDateTime { get; set; }

    }
}