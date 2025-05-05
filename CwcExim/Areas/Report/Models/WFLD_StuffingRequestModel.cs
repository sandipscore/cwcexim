using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_StuffingRequestModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StuffingReqNo { get; set; }
        public string RequestDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public decimal NoOfSBS { get; set; }
        public decimal NoOfUnits { get; set; }
        public decimal CBM { get; set; }
        public string InDate { get; set; }
        public string POL { get; set; }
        public string ContainerClass { get; set; }
        public decimal StuffWeight { get; set; }
    }
}