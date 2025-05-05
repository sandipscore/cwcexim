using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class ARDetails
    {
        public declaration declaration { get; set; } = new declaration();
        public ARlocation location { get; set; } = new ARlocation();
        public ARTransportMeans transportMeans { get; set; } = new ARTransportMeans();

        public ARevents events { get; set; } = new ARevents();

        public List<ARcargoContainer> cargoContainer { get; set; } = new List<ARcargoContainer>();

    }

    public class ARlocation
    {
        public string reportingPartyType { get; set; }
        public string reportingPartyCode { get; set; }

        public string nextDestinationOfUnlading { get; set; }
        public string reportingLocationCode { get; set; }
        //public string reportingLocationName { get; set; }
        public string authorisedPersonPAN { get; set; }

        public string bondNumber { get; set; }
    }
    public class ARTransportMeans
    {
        public string transportMeansType { get; set; }
        public string transportMeansNumber { get; set; }
        public int totalEquipments { get; set; }
    
    }


    public class ARevents
    {        
        public string expectedTimeOfArrival { get; set; }
    }


    public class ARcargoDocument
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



    public class ARcargoContainer
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
        public List<ARcargoDocument> cargoDocument { get; set; }

    }
}
