using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_ExpLoadedContrOut
    {
       
            [Required(ErrorMessage = "Fill Out This Field")]
            public string FromDate { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            public string ToDate { get; set; }
        }

        public class Ppg_LoadedContrOutList
        {
            public string ICDCode { get; set; }
            public string ForwarderName { get; set; }
            public string ContainerNo { get; set; }
            public string Size { get; set; }
            public string Seal { get; set; }
            public string ShippingLine { get; set; }
             public string GatePassNo { get; set; }
             public string GatePassDate { get; set; }
             public string GateOutDate { get; set; }
             public string TransportMode { get; set; }
   

    }
    
}