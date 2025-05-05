using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Pest.Models
{
    public class WLJAllPartyModel
    {
        public IList<DSR_CHAForPage> lstCHA { get; set; } = new List<DSR_CHAForPage>();
        public IList<DSR_ShippingLineForPage> lstShippingLine { get; set; } = new List<DSR_ShippingLineForPage>();
        public IList<DSR_ImporterForPage> lstImporter { get; set; } = new List<DSR_ImporterForPage>();
        public IList<DSR_PartyForPage> lstParty { get; set; } = new List<DSR_PartyForPage>();
        public IList<DSR_PayeeForPage> lstPayee { get; set; } = new List<DSR_PayeeForPage>();
    }

    public class WLJ_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }


    public class WLJ_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }

    public class WLJ_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string ImporterCode { get; set; }
        public string ImporterGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }

    public class WLJ_PartyForPage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class WLJ_PayeeForPage
    {
        public int PartyId { get; set; }
        public string PayeeName { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class WLJ_ImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyCode { get; set; }
    }
}