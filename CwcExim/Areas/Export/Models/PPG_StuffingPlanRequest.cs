using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_StuffingPlanRequest
    {
        public string CartingRegisterNo { get; set; }
        public int ShortCargoDtlId { get; set; }
        public int CartingRegisterId { get; set; }
    }
}