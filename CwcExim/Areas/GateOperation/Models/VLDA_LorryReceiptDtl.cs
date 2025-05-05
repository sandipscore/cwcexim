using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VLDA_LorryReceiptDtl
    {
        public string ContainerNo { get; set; }
        public int Size { get; set; }
        public int TareWt { get; set; }
        public int Weight { get; set; }
        public string VehicleNo  { get; set; }
        public string SealNo { get; set; }
        public string RemarksBooking { get; set; }
    }
}