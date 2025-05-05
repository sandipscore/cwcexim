using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kol_LoadContReq: LoadContReq
    {
        public string Movement { get; set; }
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }
    }
}