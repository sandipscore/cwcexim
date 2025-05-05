using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class WFLDBondSacDetails: BondSacDetails
    {
        public int UnloadingId { get; set; } = 0;

        public string BondNo { get; set; }
        public int CHAId { get; set; } = 0;
        public string CalculationDate { get; set; } = string.Empty;
    }
}