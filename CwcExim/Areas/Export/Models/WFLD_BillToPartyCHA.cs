using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_BillToPartyCHA
    {
       
            public int CHAId { get; set; }
            public string CHAName { get; set; }
            public string GSTNo { get; set; }          
            public string Address { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }            
            public int BillToParty { get; set; }          
        
    }
}