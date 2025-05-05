using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class DPDetails
    {
        public declaration declaration { get; set; } = new declaration();
        public DPlocation location { get; set; } = new DPlocation();
        public DPTransportMeans transportMeans { get; set; } = new DPTransportMeans();

        public DPevents events { get; set; } = new DPevents();

        public List<DPcargoContainer> cargoContainer { get; set; } = new List<DPcargoContainer>();

    }

    public class DPlocation
    {
        public string reportingPartyType { get; set; }
        public string reportingPartyCode { get; set; }

        public string nextDestinationOfUnlading { get; set; }
        public string reportingLocationCode { get; set; }
        public string reportingLocationName { get; set; }
        public string authorisedPersonPAN { get; set; }

        public string bondNumber { get; set; }
    }
    public class DPTransportMeans
    {
        public string transportMeansType { get; set; }
        public string transportMeansNumber { get; set; }
        public int totalEquipments { get; set; }
    
    }


    public class DPevents
    {
        public string expectedTimeOfDeparture { get; set; }
        public string expectedTimeOfArrival { get; set; }
    }


    public class DPcargoDocument
    {
        public string messageType { get; set; }
        public int cargoSequenceNo { get; set; }
        public string documentType { get; set; }
        public string documentSite { get; set; }
        public int documentNumber { get; set; }

        public string documentDate { get; set; }
        public string shipmentLoadStatus { get; set; }
        public string packageType { get; set; }
        public string MCINPCIN { get; set; }
    }



    public class DPcargoContainer
    {
        public string messageType { get; set; }
        public int equipmentSequenceNo { get; set; }
        public string equipmentID { get; set; }
        public string equipmentType { get; set; }
        public string equipmentSize { get; set; }
        public string equipmentLoadStatus { get; set; }
        public string finalDestinationLocation { get; set; }

        public string equipmentSealType { get; set; }
        public string equipmentSealNumber { get; set; }
        public List<DPcargoDocument> cargoDocument { get; set; }

    }
}
