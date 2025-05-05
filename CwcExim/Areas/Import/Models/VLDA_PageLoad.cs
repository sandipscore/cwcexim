using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VLDA_PageLoad
    {
        public IList<VLDACHAForPage> lstCHA { get; set; } = new List<VLDACHAForPage>();

        public IList<VLDACommodityForPage> lstCommodity { get; set; } = new List<VLDACommodityForPage>();

        public IList<VLDAShippingLineForPage> lstShippingLine { get; set; } = new List<VLDAShippingLineForPage>();

        public IList<VLDAImporterForPage> lstImporter { get; set; } = new List<VLDAImporterForPage>();
    }

    public class VLDACHAForPage
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

    public class VLDACommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class VLDAShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }

    public class VLDAImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public string PartyCode { get; set; }
    }

    public class VLDAImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyCode { get; set; }
        public bool IsInsured { get; set; }
        public bool IsTransporter { get; set; }
        public string InsuredFrmDate { get; set; }
        public string InsuredToDate { get; set; }
    }

    public class VLDA_PartyForPage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
}