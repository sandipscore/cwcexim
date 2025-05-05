using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kol_LoadShipBillNo
    {
        public string SBId { get; set; }
        public string SBNo { get; set; }
        public string weight { get; set; }
        public string Pacakage { get; set; }
        public string PacakageFrom { get; set; }
        public string PacakageTo { get; set; }
        public string StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
    }
}