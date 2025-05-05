using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class SealCuttingCHAV2
    {
        public int CHAShippingLineId { get; set; }
        public string CHAShippingLine { get; set; }
        public string FolioNo { get; set; }
        public decimal Balance { get; set; }

        public string PartyCode { get; set; }
    }
}