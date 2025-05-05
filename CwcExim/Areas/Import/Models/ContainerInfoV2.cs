using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class ContainerInfoV2
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }

        public string EximTraderName { get; set; }
        public int EximTraderId { get; set; }
    }
}