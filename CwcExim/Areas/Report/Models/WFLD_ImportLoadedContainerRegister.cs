using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ImportLoadedContainerRegister
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string Sline { get; set; }
        public string Forwarder { get; set; }
        public string Origin { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
    }
}