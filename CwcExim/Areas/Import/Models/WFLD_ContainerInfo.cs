using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLD_ContainerInfo
    {
       
            public string CFSCode { get; set; }
            public string ContainerNo { get; set; }
            public string Size { get; set; }
            public string MovementType { get; set; }
            public string TPNo { get; set; }
            public string PortName { get; set; }
            public Int32 ShippingLineId { get; set; }
            public string ShippingLine { get; set; }
        public Int32 PortId { get; set; }

        public decimal NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }



    }
}