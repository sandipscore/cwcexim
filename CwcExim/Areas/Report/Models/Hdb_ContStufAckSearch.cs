using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_ContStufAckSearch
    {

        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string shippingbillno { get; set; }
        public string shippingbilldate { get; set; }
    }

    public class Hdb_ContStufAckRes
    {
        public string shipbill { get; set; }
        public string status { get; set; }                  
        public string reason { get; set; }
    }


    public class Hdb_DTAckSearch
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }       
    }

    public class Hdb_GatePassDTAckSearch
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
    }
    public class Hdb_ContDTAckSearch
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }       
    }

    public class Hdb_DTAckRes
    {
        public string ContainerNo { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }

}
