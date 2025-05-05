using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ExportLoadedContainerOutReport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string CFSCode { get; set; }
        public string InDate { get; set; }
        public string ReceiptNo { get; set; }
        public string Forwarder { get; set; }

        public string POD { get; set; }

        public string MoveNo { get; set; }
        public string GatePassNo { get; set; }
        public string VehicleNo { get; set; }
        public decimal FOBValue { get; set; }
        public string OutDate { get; set; }

        public string PartyName { get; set; }

        public string Movement { get; set; }
    }
}