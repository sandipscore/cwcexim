using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_DPAckSearch
    {
        public string GatePassNo { get; set; }
        public int GatePassId { get; set; }
        public int GatePassdtlId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
    }
    public class WFLD_ContDPAckSearch
    {
        public string ContainerNo { get; set; }
        public int GatePassdtlId { get; set; }
        public string CFSCode { get; set; }
    }

    public class WFLD_DPAckRes
    {
        public string ContainerNo { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
    public class WFLD_GatePassDPAckSearch
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
    }
}