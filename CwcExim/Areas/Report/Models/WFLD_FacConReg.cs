using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_FacConReg
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string CFSCode { get; set; }
        public string InDate { get; set; }
        public string ReceiptNo { get; set; }
        public string Forwarder { get; set; }

        public string POD { get; set; }

        public string MoveNo { get; set; }
        public string GatePassNo { get; set; }
        public string VehicleNo { get; set; }

        public string OutDate { get; set; }

        public string PartyName { get; set; }

        public string Movement { get; set; }

        public string ShippingLine { get; set;}
         public string   DateOfArrival{get;set;}
        public string Status { get; set; }
        public string  Remarks { get; set; }
        public string TransPort { get; set; }
        public string ContainerClass { get; set; }
    }
}