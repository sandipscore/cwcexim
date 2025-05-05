using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class CIMASRCargoDetails
    {
        public string messageType { get; set; }
        public int cargoSequenceNo { get; set; }
        public string documentType { get; set; }
        public string documentSite { get; set; }
        public int documentNo { get; set; }
        public string documentDate { get; set; }
        public string shipmentLoadStatus { get; set; }
        public string packageType { get; set; }
        public decimal quantity { get; set; }
        public int packetsFrom { get; set; }
        public int packetsTo { get; set; }
        public string packUQC { get; set; }
        public string MCINPCIN { get; set; }

        public List<CIMASRCargoContainer> cargoContainer { get; set; } = new List<CIMASRCargoContainer>();
        public List<cargoItnry> cargoItnry { get; set; } = new List<cargoItnry>();

    }

}
