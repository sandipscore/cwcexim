using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_JobOrder: JobOrder
    {
    public    int MovementId { get; set; }
        public int tolocationId { get; set; }
    }
}