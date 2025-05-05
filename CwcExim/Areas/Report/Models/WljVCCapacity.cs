using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WljVCCapacity
    {
        public int vccapid { get; set; }
        public DateTime vcoptdate { get; set; }
        public decimal cfscapopen { get; set; }
        public decimal cfscapclose { get; set; }
        public decimal bndcapclose { get; set; }
        public decimal totcov { get; set; }

        public decimal cfsopenutlz { get; set; }
        public decimal cfscloseutlz { get; set; }

        public decimal cfscloseutlzOrg { get; set; }
        public decimal bndcloseutlz { get; set; }

        public decimal bndcloseutlzOrg { get; set; }
        public decimal totutil { get; set; }

        public string weekid { get; set; }
        public decimal Occupency { get; set; }
        public decimal Income { get; set; }
        public decimal ContHand { get; set; }
        public decimal Bond { get; set; }
        public decimal Doc { get; set; }
        public decimal Egm { get; set; }


        public int Import { get; set; }
        public int Export { get; set; }
        public int Empty { get; set; }
    }
}