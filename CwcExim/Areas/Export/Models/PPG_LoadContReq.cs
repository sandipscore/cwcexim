using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_LoadContReq: LoadContReq
    {
        public string Origin { get; set; }
        public string Movement { get; set; }
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
        public List<Ppg_LoadContReqDtl> lstppgContDtl { get; set; } = new List<Ppg_LoadContReqDtl>();
    }
}