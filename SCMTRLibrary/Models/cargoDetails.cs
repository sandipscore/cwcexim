using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class cargoDetails
    {

        public string messageType { get; set; }
        public Int32 cargoSequenceNo { get; set; }
        public string documentType { get; set; }
        public string documentSite { get; set; }
        public string documentNumber { get; set; }
        public string documentDate { get; set; }
        public string shipmentLoadStatus { get; set; }
        public string packageType { get; set; }
        public Int64 quantity { get; set; }
        public Int32 packetsFrom { get; set; }
        public Int32 packetsTo { get; set; }
        public string packUQC { get; set; }
        public string mcinPcin { get; set; }

    }
}
