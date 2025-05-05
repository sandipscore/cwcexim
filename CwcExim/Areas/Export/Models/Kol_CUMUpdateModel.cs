using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kol_CUMUpdateModel
    {
        public int CartingRegisterId { get; set; }
        public int CartingAppDtlId { get; set; }

        public string CartingRegisterNo { get; set; }

        public decimal CUM { get; set; }
    }
}