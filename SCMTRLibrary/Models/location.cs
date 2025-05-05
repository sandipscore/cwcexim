using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class location
    {
        public string reportingPartyType { get; set; }
        public string reportingPartyCode { get; set; }       
        public string reportingLocationCode { get; set; }
        public string reportingLocationName { get; set; }     
        public string authorisedPersonPAN { get; set; }
    }
}
