using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class cargoItnry
    {
        public int prtOfCallSeqNmbr { get; set; }
        public string prtOfCallCdd { get; set; }

        public string prtOfCallName { get; set; }

        public string nxtPrtOfCallCdd { get; set; }

        public string nxtPrtOfCallName { get; set; }

        public string modeOfTrnsprt { get; set; }
    }
}
