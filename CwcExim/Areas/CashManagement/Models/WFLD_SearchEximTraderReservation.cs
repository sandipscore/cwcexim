using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLD_SearchEximTraderReservation
    {
        public List<WfldPartyDet> lstExim { get; set; } = new List<WfldPartyDet>();
        public bool State { get; set; }
    }

  public  class WfldPartyDet
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GstNo { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }

    }
}