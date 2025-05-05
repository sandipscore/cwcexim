using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class ATcargoContainer
    {
        public string messageType { get; set; }
        public int equipmentSequenceNo { get; set; }
        public string equipmentID { get; set; }
        public string equipmentType { get; set; }

        public string equipmentSize { get; set; }
    }

    public class ATlocation
    {
        public string reportingPartyCode { get; set; }
        public string reportingPartyType { get; set; }
        public string nextDestinationOfUnlading { get; set; }

        public string reportingLocationCode { get; set; }

        public string authorisedPersonPAN { get; set; }

    }



}
