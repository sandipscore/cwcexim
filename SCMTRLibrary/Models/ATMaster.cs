using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class ATMaster
    {
        public declaration declaration { get; set; } = new declaration();
        public ATlocation location { get; set; } = new ATlocation();
        public ATcargoContainer cargoContainer { get; set; } = new ATcargoContainer();
        public ATtransportMeans transportMeans { get; set; } = new ATtransportMeans();

        public ATEvent events { get; set; } = new ATEvent();

        public DTCIM CIM { get; set; } = new DTCIM();
    }
}
