using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_LCLDelivery
    {
        public string SlNo { get; set; }

        public string AsOnDate { get; set; }
        public string OBLNo { get; set; }

        public string IGMNo { get; set; }

        public string Importer { get; set; }

        public string EntryNo { get; set; }

        public string DSTFNO { get; set; }
        public string ItemNo { get; set; }

        public decimal Pkg { get; set; }
        public decimal GrWt { get; set; }

        public decimal Area { get; set; }

        public string SlotNo { get; set; }

        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }

        public string VehicleNo { get; set; }

        public string ShippingLine { get; set; }


    }
}