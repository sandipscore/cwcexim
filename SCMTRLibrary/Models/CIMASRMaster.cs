using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class CIMASRMaster
    {
        public declaration declaration { get; set; } = new declaration();
        public location location { get; set; } = new location();

        public List<CIMASRCargoDetails> cargoDetails { get; set; } = new List<CIMASRCargoDetails>();
    }

}
