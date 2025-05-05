using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDConsolidatedExportContainerModel
    {
        public int SR { get; set; }
        public string EntryNo { get; set; }
            public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SlaCd { get; set; }

        public string POL { get; set; }
        public string POD { get; set; }
        public string TransporterName { get; set; }
        public string CHACD { get; set; }
        public string Units { get; set; }
        public string PartyName { get; set; }
        public string ReceiptNo { get; set; }
        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }
        public string MainLine { get; set; }
        public string VehicleNo { get; set; }


        public string ContainerClass { get; set; }

        public string Forwarder { get; set; }

        public string IWBNo { get; set; }
        public string Amount { get; set; }


    }
}