using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLD_PartyDet
    {
        
            public int PartyId { get; set; }
            public string PartyName { get; set; }
            public string PartyCode { get; set; }
            public string GstNo { get; set; }
            public string StateCode { get; set; }
        public string Statename { get; set; }

    }
}