using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_TotalBond
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string CFSCode { get; set; }
        public string EntryDateTime { get; set; }

        public string TransportMode { get; set; }
        public string ContClass { get; set; }
        public string Remarks { get; set; }
        public string VehicleNo { get; set; }
        public string ShippingLine { get; set; }
        public string ContainerType { get; set; }
    }
}