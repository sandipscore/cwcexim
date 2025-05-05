using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_WheelStuffingModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EntryNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SLACD { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string TransporterName { get; set; }
        public string CHACD { get; set; }
        public decimal FOBValue { get; set; }
        public decimal Units { get; set; }
        public string PartyName { get; set; }
        public string ReceiptNo { get; set; }
        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }
        public string MainLine { get; set; }
        public string VehicleNo { get; set; }

    }
}