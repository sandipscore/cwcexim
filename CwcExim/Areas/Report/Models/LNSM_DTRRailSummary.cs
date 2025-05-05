using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_DTRRailSummary
    {
        public int SlNo { get; set; }
        //public string CostCode { get; set; }
        public string Movementdate { get; set; }
        public string TrainNumber { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string ContType { get; set; }
        public string WagonNo { get; set; }
        public string ContNumber { get; set; }
        public string ContStatus { get; set; }
        public string Size { get; set; }

        public string Cargo { get; set; }

        public string CargoWt { get; set; }
        public string TareWt { get; set; }
        public string OperatonType { get; set; }
        public string TotalWt { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
       }
}