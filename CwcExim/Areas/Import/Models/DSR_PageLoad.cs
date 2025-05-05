using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_PageLoad
    {
        public IList<DSRCHAForPage> lstCHA { get; set; } = new List<DSRCHAForPage>();

        public IList<DSRCommodityForPage> lstCommodity { get; set; } = new List<DSRCommodityForPage>();

        public IList<DSRShippingLineForPage> lstShippingLine { get; set; } = new List<DSRShippingLineForPage>();

        public IList<DSRImporterForPage> lstImporter { get; set; } = new List<DSRImporterForPage>();

    }
    public class DSRCHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }

        public bool BillToParty { get; set; }
        public bool IsInsured { get; set; }
        public string InsuredFrmdate { get; set; }
        public string InsuredTodate { get; set; }
        public bool IsTransporter { get; set; }

        public int rows { get; set; }
    }

    public class DSRCommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class DSRShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }

    public class DSRImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public string PartyCode { get; set; }
    }

    public class DSRImpPartyForpage
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
        public string  InsuredFrmDate { get; set; }
        public string InsuredToDate { get; set; }

        public int rows { get; set; }
    }

    public class DSR_PartyForPage
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