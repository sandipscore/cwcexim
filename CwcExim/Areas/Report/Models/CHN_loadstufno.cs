using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_loadstufno
    {
        public string Jobno { get; set; }
        public string Stufno { get; set; }
        public string Loadno { get; set; }
        public string Asrjobno { get; set; }
        public string Asrstufno { get; set; }
        public string Asrloadno { get; set; }
        public string Dpjobno { get; set; }
        public string Dplodgpno { get; set; }
        public string Dpstugpno { get; set; }
        public string Dtjobno { get; set; }
        public string Dtstugeno { get; set; }
        public string Dtlodgeno { get; set; }
    }
    public class CHN_loadstuf
    {
        public string loadstufreqno { get; set; }
        public string expstufreqno { get; set; }
       // public string reason { get; set; }
    }
    public class CHN_loadstufasr
    {
        public string loadstufasrreqno { get; set; }
        public string expstufasrreqno { get; set; }
        // public string reason { get; set; }
    }
    public class CHN_loadstufdp
    {
        public string loadstufdpreqno { get; set; }
        public string expstufdpreqno { get; set; }
        // public string reason { get; set; }
    }
    public class CHN_loadstufdt
    {
        public string loadstufdtreqno { get; set; }
        public string expstufdtreqno { get; set; }
        // public string reason { get; set; }
    }
}