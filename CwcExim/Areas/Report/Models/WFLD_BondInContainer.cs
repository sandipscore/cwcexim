using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_BondInContainer
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string InDate { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public string origin { get; set; }
        public string Status { get; set; }
        public string ContClass { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string CHAName { get; set; }
        public string Importer { get; set; }
    }
}