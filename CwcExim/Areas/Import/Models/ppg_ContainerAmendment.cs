using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class ppg_ContainerAmendment
    {
        public string ContainerNo { get; set; }

     

        public int FormOneDetailId { get; set; }

        public int FormOneId { get; set; }
        public string OldContainerSize { get; set; }
        public string newContainerSize { get; set; }
        public int OldShippingLineId { get; set; }

        public int newShippingLineId { get; set; }
        public string OldShippingLineName { get; set; }
        public string NewShippingLineName { get; set; }
        public int AmmendmentId { get; set; }
        public string AmendNo { get; set; }
        public string AmendDate { get; set; }


    }
}