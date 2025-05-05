using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VCCapacityModel
    {
        public int vccapid { get; set; }
        public DateTime vcoptdate { get; set; }
        public decimal cfscap { get; set; }
        public decimal bndcap { get; set; }
        public decimal cfsutlz { get; set; }
        public decimal bndutlz { get; set; }
        public string weekid { get; set; }
        public decimal Occupency { get; set; }
        public decimal OccupencyCFS { get; set; }
        public decimal OccupencyBond { get; set; }
        public decimal Income { get; set; }
        public int Import { get; set; }
        public int Export { get; set; }
        public int Empty { get; set; }

        public decimal CurrPCSIncome { get; set; }
        public decimal PrevPCSIncome { get; set; }
        public decimal YearPCSIncome { get; set; }

    }
}