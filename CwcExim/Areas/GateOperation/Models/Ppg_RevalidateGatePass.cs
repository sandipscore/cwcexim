using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class Ppg_RevalidateGatePass
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string ExpiryDT { get; set; }
        public string DeliveryDate { get; set; }
        public string ExtendDT { get; set; }       
        public string GatePassDate { get; set; }
    }
}