using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Kdl_ServiceCodeWiseInvDtls: ServiceCodeWiseInvDtls
    {
        public string Taxable { get; set; }
        public string Totalgst { get; set; }
        public string supplytype { get; set; }
        public string partystate { get; set; }
        public string partystatecode { get; set; }
    }
}