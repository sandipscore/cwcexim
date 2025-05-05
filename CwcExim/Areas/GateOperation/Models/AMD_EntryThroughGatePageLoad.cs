using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class AMD_EntryThroughGatePageLoad
    {


        public IList<AMD_CHAForPage> lstCHA { get; set; } = new List<AMD_CHAForPage>();

        public IList<AMD_CommodityForPage> lstCommodity { get; set; } = new List<AMD_CommodityForPage>();

        public IList<AMD_ShippingLineForPage> lstShippingLine { get; set; } = new List<AMD_ShippingLineForPage>();

        public IList<AMD_ImporterForPage> lstImporter { get; set; } = new List<AMD_ImporterForPage>();

    }

    public class AMD_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }


    }

    public class AMD_CommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class AMD_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }

    public class AMD_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public string PartyCode { get; set; }
    }

    public class AMD_ImpPartyForpage
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