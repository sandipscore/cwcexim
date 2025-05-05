using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLD_ContainerModel
    {
       
            public List<WFLD_PartyDet> lstExim { get; set; } = new List<WFLD_PartyDet>();
            public bool State { get; set; }
        
    }
}