using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class DND_PageLoad
    {
        public IList<Dnd_CHAForPage> lstCHA { get; set; } = new List<Dnd_CHAForPage>();

        public IList<Dnd_CommodityForPage> lstCommodity { get; set; } = new List<Dnd_CommodityForPage>();

        public IList<Dnd_ShippingLineForPage> lstShippingLine { get; set; } = new List<Dnd_ShippingLineForPage>();

        public IList<Dnd_ImporterForPage> lstImporter { get; set; } = new List<Dnd_ImporterForPage>();

    }

    public class Dnd_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }
        public bool BillToParty { get; set; }
        public bool IsInsured { get; set; }
        public string InsuredFrmdate { get; set; }
        public string InsuredTodate { get; set; }
        public bool IsTransporter { get; set; }


    }

    public class Dnd_CommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class Dnd_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }

    public class Dnd_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

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
}