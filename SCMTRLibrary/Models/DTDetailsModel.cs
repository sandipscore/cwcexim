using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class DTDetailsModel
    {
        public declaration declaration { get; set; } = new declaration();
        public DTlocation location { get; set; } = new DTlocation();

        public List<DTcargoContainer> cargoContainer { get; set; } = new List<DTcargoContainer>();

        public DTtransportMeans transportMeans { get; set; } = new DTtransportMeans();

        public DTevents events { get; set; } = new DTevents();
        public DTCIM CIM { get; set; } = new DTCIM();


    }

    public class DTlocation
    {
        public string reportingPartyType { get; set; }
        public string reportingPartyCode { get; set; }

        public string nextDestinationOfUnlading { get; set; }
        public string reportingLocationCode { get; set; }
        public string reportingLocationName { get; set; }
        public string authorisedPersonPAN { get; set; }

       // public string bondNumber { get; set; }
    }


    public class DTcargoContainer
    {
        public string messageType { get; set; }
        public int equipmentSequenceNo { get; set; }
        public string equipmentID { get; set; }
        public string equipmentType { get; set; }
        public string equipmentSize { get; set; }
      
    }

    public class DTtransportMeans
    {
        public string transportMeansType { get; set; }
        public string transportMeansNumber { get; set; }
        public int totalEquipments { get; set; }
      
    }

    public class DTevents
    {
        public string actualTimeOfDeparture { get; set; }
    }

    public class DTCIM
    {
        public int CIMNumber { get; set; }

        public string CIMDate { get; set; }
    }
}
