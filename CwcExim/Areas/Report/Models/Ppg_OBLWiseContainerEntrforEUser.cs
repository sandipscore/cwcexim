using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_OBLWiseContainerEntrforEUser : CwcExim.Areas.Report.Models.Ppg_OBLWiseContainerEntry
    {
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string GateExitNo { get; set; }
        public string GateExitDate { get; set; }
        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }

        public decimal CBM { get; set; }
        public decimal SQM { get; set; }

        public string ShippingLine { get; set; }

        public string DestuffingNo { get; set; }
        public string DestuffingDate { get; set; }

    }

    public class OBLNoForPages
    {
        public string OBLNo { get; set; }
        public string TSANo { get; set; }
        public string LINENo { get; set; }
    }
}