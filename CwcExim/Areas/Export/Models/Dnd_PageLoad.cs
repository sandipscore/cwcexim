using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_PageLoad
    {

        public IList<Dnd_CHAForPage> lstCHA { get; set; } = new List<Dnd_CHAForPage>();

        public IList<Dnd_CommodityForPage> lstCommodity { get; set; } = new List<Dnd_CommodityForPage>();

        public IList<ShippingLineForPage> lstShippingLine { get; set; } = new List<ShippingLineForPage>();

        public IList<Dnd_ImporterForPage> lstImporter { get; set; } = new List<Dnd_ImporterForPage>();

        public IList<ExporterForPage> lstExporter { get; set; } = new List<ExporterForPage>();


    }

    public class Dnd_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }


    }

    public class Dnd_CommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class Dnd_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public string PartyCode { get; set; }
    }

    public class ExporterForPage
    {
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }

        public string PartyCode { get; set; }
    }
    public class Dnd_ImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyCode { get; set; }
    }

    public class ForwarderForPage
    {
        public string Forwarder { get; set; }
        public int ForwarderId { get; set; }
        public string PartyCode { get; set; }
    }

    public class MainlineForPage
    {
        public string Mainline { get; set; }
        public int MainlineId { get; set; }
        public string PartyCode { get; set; }
    }
}
